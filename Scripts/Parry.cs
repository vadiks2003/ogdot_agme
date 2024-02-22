using Godot;
using System;

public partial class Parry 
{
    public Player player;
    double Delay = 0.4;
    double ParryDuration = 0.7;
    double LastTimePressed = 0;
    struct ParryInfo{
        public double angle;
        public double clickTime;
        public bool isActive;
    } 
    AudioStreamPlayer2D succ;
    AudioStreamPlayer2D unsheathe;
    ParryInfo[] parrylist;

    public Parry()
    {
        parrylist = new ParryInfo[3];
    }
    public void SetAudio(AudioStreamPlayer2D audiostreamplayer, AudioStreamPlayer2D audiostreamplayer2)
    {
        succ = audiostreamplayer;
        unsheathe = audiostreamplayer2;
    }
    public void doParry()
    {
        if(Input.IsActionJustPressed("rightclick"))
        {
            UpdateParryList();
            for(int i = 1; i < parrylist.Length; i++)
            {
                if(!parrylist[i].isActive && Time.GetUnixTimeFromSystem() > parrylist[i].clickTime + Delay)
                {
                    LastTimePressed = Time.GetUnixTimeFromSystem();
                    parrylist[i].isActive = true;
                    parrylist[i].clickTime = Time.GetUnixTimeFromSystem();
                    //later check
                    parrylist[i].angle = -Mathf.RadToDeg(player.Rotation);
                    unsheathe.Play();
                    return;
                }
            }

            int rawstlii = ReturnActiveWithSmallestTimeLeftIndex();
            if(parrylist[rawstlii].clickTime + Delay < Time.GetUnixTimeFromSystem())
            {
                // if delay isnt big enough yet there is already parry info somewhere occupied, simply update the oldest one!
                parrylist[rawstlii].clickTime = Time.GetUnixTimeFromSystem();
                parrylist[rawstlii].angle = Mathf.RadToDeg(player.Rotation);
                
                unsheathe.Play();
                return;
            }
        }
    }

    int ReturnActiveWithSmallestTimeLeftIndex(){
        int minIndex = 0;
        for(int i = 1; i < parrylist.Length; i++)
        {
            if(parrylist[i].clickTime < parrylist[minIndex].clickTime){
                minIndex = i;
            }
        }
        return minIndex;
        // should return parryinfo with smallest time
    }

    void UpdateParryList()
    {
        for(int i = 1; i < parrylist.Length; i++)
        {
            if(Time.GetUnixTimeFromSystem() - parrylist[i].clickTime >= ParryDuration)
            {
                parrylist[i].isActive = false;
            }
        }
    }
    
    // returns true if you get hit
    public bool GetHitBitch(Vector2 source)
    {
        return CheckParryTime(source);
    }

//  angle required for player to look at to see the enemy
    double GetRotationReq(Vector2 viewer, Vector2 target)
    {
        return Mathf.RadToDeg(target.AngleToPoint(viewer));
    }


    /*bool AngledRight(Vector2 source, double angle){
        double rotationreq = GetRotationReq(player.Position, source);
        angle = -angle;
        double min = angle-40;
        double max = angle+40;
        GD.Print("before min ", min, " max ",max, " required ", rotationreq, " angle ", angle);
        if(angle < 0 && rotationreq > 0){
            GD.Print("if statement called");
            angle = 360-Math.Abs(angle);
            min = angle-40;
            max = angle+40;
        }
        GD.Print("after min ", min, " max ",max, " required ", rotationreq, " angle ", angle);
        if(rotationreq >= min && rotationreq <= max) return true;
        return false;
    }*/

    // i think it works? since -180 180 system makes left weird i also made 0-360 check where right is weird
    bool AngledRight(Vector2 source, double angle){
        double rotationreq = GetRotationReq(player.Position,source);
        double angleLimit = 80;
        double min180 = angle-angleLimit;
        double max180 = angle+angleLimit;
        if(rotationreq >= min180 & rotationreq <= max180) return true;

        if(angle < 0){
            angle = 360-Math.Abs(angle);
        }
        if(rotationreq < 0)
        {
            rotationreq = 360-Math.Abs(rotationreq);
        }
        double min360 = angle-angleLimit;
        double max360 = angle+angleLimit;
        if(rotationreq >= min360 & rotationreq <= max360) return true;
        return false;
    }

    bool CheckParryTime(Vector2 source){
        UpdateParryList();
        for(int i = 1; i < parrylist.Length; i++)
        {
            if(parrylist[i].isActive && AngledRight(source, parrylist[i].angle))
            {
                parrylist[i].isActive = false;
                succ.Play();
                return false;
            }
        }
        return true;
    }
}

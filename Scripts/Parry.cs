using Godot;
using System;

public class Parry 
{
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
            for(int i = 0; i < 3; i++)
            {
                if(!parrylist[i].isActive && Time.GetUnixTimeFromSystem() > parrylist[i].clickTime + ParryDuration + Delay)
                {
                    LastTimePressed = Time.GetUnixTimeFromSystem();
                    parrylist[i].isActive = true;
                    parrylist[i].clickTime = Time.GetUnixTimeFromSystem();
                    MakeParryCollider(i);
                    unsheathe.Play();
                    return;
                }
            }
        }
    }
    void UpdateParryList()
    {
        for(int i = 0; i < 3; i++)
        {
            if(Time.GetUnixTimeFromSystem() - parrylist[i].clickTime >= ParryDuration)
            {
                parrylist[i].isActive = false;
            }
        }
    }
    // based on angle, make a collider with front and back boxes. if back collider has been hit first, hurt the player.
    // if front collider has been hit first, don't hurt the player
    void MakeParryCollider(int i)
    {
        // later find pointer to player's rotation and set angle to player's angle
        parrylist[i].angle = 0;
    }
    
    // returns true if you get hit
    public bool GetHitBitch()
    {
        return CheckParryTime();
    }

    bool CheckParryTime(){
        UpdateParryList();
        for(int i = 0; i < 3; i++)
        {
            if(parrylist[i].isActive){
                parrylist[i].isActive = false;
                succ.Play();
                return false;
            }
        }
        return true;
    }
}

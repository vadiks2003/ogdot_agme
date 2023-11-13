using System.IO.Pipes;
using Godot;
public class Player : KinematicBody2D
{
    AudioStreamPlayer2D[] audiostreams;
    Movement movement = new Movement();
    Parry parry = new Parry();
    static KeyManager keymanager = new KeyManager();
    Camera camera;
    int health = 100;
    public bool AllowedToWalk = true;
    double iframes = 1;
    double lastTimeHit = Time.GetUnixTimeFromSystem();

    public override void _Ready()
    {
        audiostreams = new AudioStreamPlayer2D[4];
        audiostreams[0] = GetNode<AudioStreamPlayer2D>("Hit");
        audiostreams[1] = GetNode<AudioStreamPlayer2D>("Parry");
        audiostreams[2] = GetNode<AudioStreamPlayer2D>("Dodge");
        audiostreams[3] = GetNode<AudioStreamPlayer2D>("ParryAdded");
        parry.SetAudio(audiostreams[1], audiostreams[3]);
        camera = GetNode<Camera>("../Camera2D");

    }
    public override void _PhysicsProcess(float delta)
    {
        ProcessMovement();
        ProcessParry();
    }

    void ProcessMovement(){
        LookAt(GetGlobalMousePosition());
        movement.GetInput();
        // originally on their website it says velocity = move_and_slide(velocity). move and slide returns velocity. i dont know why but ok
        if(AllowedToWalk) movement.velocity = MoveAndSlide(movement.velocity);
    }

    void ProcessParry(){
        parry.doParry();
    }

// null question mark is questionable but i am too lazy to check now
    public bool Damage(int health, Vector2? source = null)
    {
        if(lastTimeHit + iframes >= Time.GetUnixTimeFromSystem()) return false;
        if(source == null) source = Vector2.Zero;
        if(!parry.GetHitBitch()) return false;
        if(movement.isDashing) {
            audiostreams[2].Play();
            return false;
        };
        this.health -= health;   
        camera.ShakeCamera(source.Value, health);
        movement.pushVelocity = -(source.Value - Position) * health * 10;
        audiostreams[0].Play();
        lastTimeHit = Time.GetUnixTimeFromSystem();
        return true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("click"))
        {
            // hit
                        
        }
    }



    // honestly why am i putting it inside player if i can make keymanager just entirely static
    public static bool isThereKey(int id){
        return keymanager.DoesHaveKey(id);
    }
    public static void CreateKey(int id)
    {
        keymanager.CreateKey(id);
    }

    public delegate void CallBackKey();
    public static void UseKey(int id, CallBackKey callback)
    {
        if(keymanager.UseKey(id)){
            callback();
        }
    }
}
using System.Linq.Expressions;
using Godot;

class Movement{
    private int Speed = 30;
    private int normalSpeed = 30;
    private int shiftSpeed = 60;

    // dash related below
    Vector2 DashVelocity = new Vector2();

    public Vector2 pushVelocity = new Vector2();
    private float DoubleTapSpeed = 0.22f;
    private double LastTimePressed;
    private double lastTimeDashed;
    private double DashDelay = 0.22f;
    private int DashPower = 150;
    private float DashDecay = 1.1f;

    public bool isDashing = false;
    enum buttons{
        Up,Left,Down,Right
    }
    buttons LastButtonPressed;
    
    // used for moveandslide since it returns for some reason
    public Vector2 velocity = new Vector2();

    void DashChecks(buttons button)
    {
        if(LastTimePressed != 0 &&
         Time.GetUnixTimeFromSystem() - LastTimePressed <= DoubleTapSpeed && 
         Time.GetUnixTimeFromSystem() - lastTimeDashed >= DashDelay &&
          LastButtonPressed == button)
        {
            if(button == buttons.Up) Dash(Vector2.Up);
            if(button == buttons.Down) Dash(Vector2.Down);
            if(button == buttons.Left) Dash(Vector2.Left);
            if(button == buttons.Right) Dash(Vector2.Right);
        }
        
        LastTimePressed = Time.GetUnixTimeFromSystem();
        LastButtonPressed = button;
    }
    void Dash(Vector2 direction)
    {
        isDashing = true;
        lastTimeDashed = Time.GetUnixTimeFromSystem();
        DashVelocity = direction * DashPower;
    }
    public void GetInput()
    {

        // movement
        velocity = new Vector2();

        if (Input.IsActionPressed("right")) velocity.X += 1;
        if (Input.IsActionPressed("left")) velocity.X -= 1;
        if (Input.IsActionPressed("down")) velocity.Y += 1;
        if (Input.IsActionPressed("up")) velocity.Y -= 1;

        // dash checks
        if(Input.IsActionJustPressed("right")) DashChecks(buttons.Right);
        if(Input.IsActionJustPressed("left")) DashChecks(buttons.Left);
        if(Input.IsActionJustPressed("down")) DashChecks(buttons.Down);
        if(Input.IsActionJustPressed("up")) DashChecks(buttons.Up);

        // decay velocity of dash
        if(DashVelocity.Length() >= 10f)/*the following is shit formula*/ DashVelocity = DashVelocity / DashDecay;
        if(DashVelocity.Length() < 10f)
        {
            isDashing = false;
            DashVelocity = Vector2.Zero;
        }

        // is dash decay good for that shit
        if(pushVelocity.Length() >= 50f) pushVelocity = pushVelocity / DashDecay;
        if(pushVelocity.Length() < 50f) pushVelocity = Vector2.Zero;

        // what does it look like
        if(Input.IsActionJustPressed("run")) Speed = shiftSpeed;
        if(Input.IsActionJustReleased("run")) Speed = normalSpeed;

        velocity = velocity * Speed + DashVelocity + pushVelocity;
    }
}
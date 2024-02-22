using Godot;
using System;

public partial class Camera : Camera2D
{
    Node2D target;
    Vector2 targetPosition;
    Vector2 _offset = Vector2.Zero;
    public override void _Ready()
    {
        target = GetNode<Node2D>("../Player");
    }

    //todo
    public void ShakeCamera(Vector2 source, int strength){
        _offset = (source - targetPosition) * (float)Math.Sqrt(strength);
    }
    public override void _PhysicsProcess(double delta)
    {
        if(_offset.Length() >= 0.1f)
        {
            // doesnt wobble, think later; think of physics ball swinging thing
            _offset *= 0.95f;
        }
        targetPosition = target.Position;
        Position = targetPosition - _offset;
    }
}

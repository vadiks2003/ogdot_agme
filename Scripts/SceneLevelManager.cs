using Godot;
using System;
using System.Drawing;

// use it to make use of physicsprocess and physics and other Node2D abilities

public partial class SceneLevelManager : Node2D
{
    bool ispaused = false;
    public override void _Ready()
    {
        Engine.MaxFps = 60;
    }
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("esc")){
            if(ispaused){
                ispaused = false;
                GetTree().Paused = false;
                return;
            }
            ispaused = true;
            GetTree().Paused = true;
        }
    }
}

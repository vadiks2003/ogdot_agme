using Godot;
using System;

public partial class settings : VBoxContainer
{
    public void onBack()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
    }
}

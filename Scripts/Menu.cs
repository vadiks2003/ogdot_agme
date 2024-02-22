using Godot;
using System;

public partial class Menu : VBoxContainer
{
    public void onStart()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Level.tscn");
    }
    public void onSettings(){
        GetTree().ChangeSceneToFile("res://Scenes/settings.tscn");
    }
}

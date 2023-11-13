using Godot;
using System;
using System.Diagnostics;

public class GenericNPC : StaticBody2D
{
    protected bool PlayerInsideTrigger = false;
    protected DialogueManager dialogman;

    public override void _Process(float delta)
    {
        // check inputs that remove the dialogue and if so, RemoveText();

    }
    public override void _Ready()
    {
        dialogman = GetNode<DialogueManager>("../DialogueManager");
    }

// i forgot what f stands for
    protected virtual void OnEnterf(Node node)
    {
        if(node.Name == "Player")
        {
            PlayerInsideTrigger = true;
            return;
        }
    }
    protected virtual void OnExitf(Node node)
    {
        if(node.Name == "Player")
        {
            PlayerInsideTrigger = false;
            dialogman.RemoveText();
        }
    }

}

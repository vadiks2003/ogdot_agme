using Godot;
using System;
using System.Diagnostics;

public partial class GenericNPC : StaticBody2D
{
    protected bool PlayerInsideTrigger = false;
    protected DialogueManager dialogman;

    public override void _Ready()
    {
        dialogman = GetNode<DialogueManager>("../DialogueManager");
    }

// i forgot what f stands for
    protected virtual void OnEnter(Node2D node)
    {
        if(node.Name == "Player")
        {
            PlayerInsideTrigger = true;
            return;
        }
    }
    protected virtual void OnExit(Node2D node)
    {
        if(node.Name == "Player")
        {
            PlayerInsideTrigger = false;
            dialogman.RemoveText();
        }
    }

}

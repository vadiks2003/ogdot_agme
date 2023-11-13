using Godot;
using System;

public class DialogueManager : Node
{
    Panel panel;
    RichTextLabel dialogueText;
    Vector2 DialogueWindowOff_Pos;
    Vector2 DialogueWindowOn_Pos;

    public override void _Ready()
    {
        panel = GetNode<Panel>("../CanvasLayer/Control/Panel");
        dialogueText = GetNode<RichTextLabel>("../CanvasLayer/Control/Panel/RichTextLabel");
        DialogueWindowOff_Pos = panel.RectPosition;
        DialogueWindowOn_Pos = DialogueWindowOff_Pos - new Vector2(0f,200f);
    }

    public void Say(string text)
    {
        panel.SetPosition(DialogueWindowOn_Pos);
        dialogueText.Text = text;
    }
    public void RemoveText()
    {
        panel.SetPosition(DialogueWindowOff_Pos);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;

public partial class DialogueManager : Node
{
    Panel panel;
    RichTextLabel dialogueText;
    Vector2 DialogueWindowOff_Pos;
    Vector2 DialogueWindowOn_Pos;
    public bool dialogueopened;

    public override void _Ready()
    {
        panel = GetNode<Panel>("../CanvasLayer/Control/Panel");
        dialogueText = GetNode<RichTextLabel>("../CanvasLayer/Control/Panel/RichTextLabel");
        DialogueWindowOff_Pos = panel.Position;
        DialogueWindowOn_Pos = DialogueWindowOff_Pos - new Vector2(0f,200f);
    }

    public void Say(string text)
    {
        dialogueopened = true;
        panel.SetPosition(DialogueWindowOn_Pos);
        dialogueText.Text = text;
    }
    public void RemoveText()
    {
        dialogueopened = false;
        panel.SetPosition(DialogueWindowOff_Pos);
    }
}

using System.Data.Odbc;
using Godot;

public partial class InteractionNPC : GenericNPC
{
    bool gaveKey = false;
    bool dialogueopened  = false;
    int line = 0;
    string[] lines = {
        "welcome to the game.",
        "i am your tutorial. i will teach you the basics of controls",
        "press right mouse to PARRY",

        "nice. what you did is activate parry for a bit here.",
        "when you parry, you save your parry angle at the moment",
        "so you can parry several targets at the same time with different angles",
        "you can also dodge by double tapping any direction key.",
        "some attacks damage through dodge and parry, some can only be",
        "parried while dodging. but these should be rare.",
        "rest of controls should be intuitive.",
        "congrats, i give you the key, now go.",
        "i have nothing else to tell you. go."
    };
    bool WaitingForParry = false;
    bool hasParried =  false;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if(WaitingForParry && Input.IsActionJustPressed("rightclick")){
            hasParried = true;
        }
        if(line == 2)
        {
            WaitingForParry = true;

            if(hasParried){
                line++;
                dialogman.Say(lines[line]);
                
            }
        }
        if(PlayerInsideTrigger && Input.IsActionJustPressed("use")){
            if(line == 9) Player.CreateKey(0);
            if(line != 2) {
                bool isdialogueopen = dialogman.dialogueopened;
                
                if(line < lines.Length-1 && isdialogueopen)
                {
                    line++;
                }
                dialogman.Say(lines[line]);
            }
        }
    }
}
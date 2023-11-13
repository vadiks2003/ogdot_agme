using Godot;

public partial class InteractionNPC : GenericNPC
{
    bool gaveKey = false;
    int line = 0;
    string[] lines = {
        "hello. you need the key? come get the key *gives you the key*",
        "you might now know me, and i don't know you, but i feel like you must run",
        "RUN"
    };

    public override void _Process(float delta)
    {
        base._Process(delta);
        if(PlayerInsideTrigger && Input.IsActionJustPressed("use")){
            if(!gaveKey)
            {
                Player.CreateKey(0);
                gaveKey = true;

                dialogman.Say(lines[line]);
                line++;
                return;
            }
            dialogman.Say(lines[line]);
            if(line < lines.Length-1)
            {
                line++;
            }
            
        }
    }
}
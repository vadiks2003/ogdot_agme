using Godot;
using System;

public class Boss : KinematicBody2D
{
	Timer timeanager = new Timer();
	DialogueManager dialman;
	bool entered = false;
	Vector2 movement;
    Vector2 aimove;
    Vector2 push;
	Node2D player;
	Player playerscr;
	Area2D bossArea;
	public void OnTriggerEnter(Node node){
		if(!entered){
			// entrance scene
			entered = true;
			playerscr = (Player)node;
			playerscr.AllowedToWalk = false;
			timeanager.StartCountDown();
			timeanager.AddEvent(()=>dialman.Say("poopie fart doopie shmoopie") ,2.0);
			timeanager.AddEvent(()=>dialman.Say("weeweewoowoo") , 2.5);
			timeanager.AddEvent(()=>dialman.Say("erg erg erg") , 3.0);
			timeanager.AddEvent(()=>playerscr.AllowedToWalk = true, 4.5);
			timeanager.AddEvent(()=>dialman.RemoveText(), 4.0);
			timeanager.AddEvent(()=>InitiateBossBattle(), 6.0);
			timeanager.AddEvent(()=>bossArea.Scale = new Vector2(1.5f, 1.5f), 1.0);
			timeanager.SortTimes();
		}
		else if(node.Name=="Player"){
			
			if(!playerscr.Damage(3, Position)){
                GD.Print("no playerscr damage1");
                push = new Vector2(Position - player.Position).Normalized() * 2000;
            }
		}
	}

	public override void _Ready()
	{
		bossArea = GetNode<Area2D>("./Trigger");
		bossArea.Scale = new Vector2(10,10);
		dialman = GetNode<DialogueManager>("../DialogueManager");
		player = GetNode<Node2D>("../Player");
	}
	void InitiateBossBattle(){
		// god help me with ai and other stuff
		timeanager.AddLoopedEvent(()=>aimove = new Vector2(player.Position-Position).Normalized() * 400, 2.0);
	}
	
	public override void _PhysicsProcess(float delta)
	{
        movement = aimove + push;
        if(push.Length() > 10) push = push/1.08f;
        if(push.Length() < 10) push = Vector2.Zero;
		movement = MoveAndSlide(movement);
	}
	public override void _Process(float delta)
	{   
		// probs should remove and put it into the special class on SceneManager game object
		timeanager.Check();
	}
}

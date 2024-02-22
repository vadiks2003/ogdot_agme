using Godot;
using System;

public partial class Boss : CharacterBody2D
{
	Timer timeanager = new Timer();
	DialogueManager dialman;
	bool entered = false;
	Vector2 movement;
    Vector2 aimove;
	AI_movement aimovevemnt = new AI_movement();
	bool readyBoss = false;
    Vector2 push;
	Node2D player;
	Player playerscr;
	Area2D bossArea;

	enum Mode{
		PathFollowing,
		PlayerFollowing
	};
	Mode mode;
	public void OnTriggerEnter(Node node){
		if(!entered){
			// entrance scene
			entered = true;
			playerscr = (Player)node;
			
			timeanager.StartCountDown();
			timeanager.AddEvent(()=>dialman.Say("Stop right there!") ,0.1);
			timeanager.AddEvent(()=> playerscr.AllowedToWalk = false, 0.2);
			timeanager.AddEvent(()=>dialman.Say("You are not supposed to be here. This is a closed property.") , 2);
			timeanager.AddEvent(()=>dialman.Say("How did you even get here? This place is closed as hell.") , 5.0);
			timeanager.AddEvent(()=>dialman.Say("You look suspicious too. I should get rid of you.") , 8.0);
			timeanager.AddEvent(()=>{
				playerscr.AllowedToWalk = true;
				dialman.RemoveText();
				InitiateBossBattle();
			}, 9);

			// me from future here. WTF?
			timeanager.AddEvent(()=>bossArea.Scale = new Vector2(1.5f, 1.5f), 1.0);
			timeanager.SortTimes();
			return;
		}
		if(node.Name=="Player"){
			Player.DamageType damagetype = playerscr.Damage(3, Position);
			if(damagetype == Player.DamageType.Parried){
                push = new Vector2(Position.X - player.Position.X, Position.Y - player.Position.Y).Normalized() * 20;
            }
		}
	}

	public override void _Ready()
	{
		mode = Mode.PathFollowing;
		// i wanted to make entrance trigger and boss damage trigger as 2 separate things but didn't figure out how so i made the trigger just become boss sized when you enter =)
		// edit: later change the dialogue to another script to the special trigger entity
		bossArea = GetNode<Area2D>("./Trigger");
		bossArea.Scale = new Vector2(10,10);

		dialman = GetNode<DialogueManager>("../DialogueManager");
		player = GetNode<Node2D>("../Player");
	}
	void InitiateBossBattle(){
		// god help me with ai and other stuff
		//timeanager.AddLoopedEvent(()=>aimove = new Vector2(player.Position-Position).Normalized() * 400, 2.0);
		aimovevemnt.MoveTowards(Position, Position+(Vector2.Up*100));
		aimovevemnt.MoveTowards(Position, Position+(Vector2.Right*100+ Vector2.Up*100));
		aimovevemnt.MoveTowards(Position, Position+(Vector2.Right*100));
		aimovevemnt.MoveTowards(Position, Position);
		readyBoss = true;
	}
	
	public override void _PhysicsProcess(double delta)
	{
        movement = aimove + push;
		if(readyBoss)
		{
			if(mode == Mode.PathFollowing && !aimovevemnt.Stop)
			{
				aimove = (aimovevemnt.GetCurrentGoal() - Position).Normalized(); 
				aimovevemnt.CheckIfReached(Position);
			}
			if(mode == Mode.PlayerFollowing){
				aimove = (player.Position-Position).Normalized();
			}
			
		} 
        if(push.Length() > 10) push = push/1.08f;
        if(push.Length() < 10) push = Vector2.Zero;
		MoveAndCollide(movement);
	}
	public override void _Process(double delta)
	{   
		// probs should remove and put it into the special class on SceneManager game object
		timeanager.Check();
	}
}

using System.Collections;
using System.ComponentModel;
using Godot;

public partial class Door : StaticBody2D
{
	[Export]
	int id = 0;
	
	bool hasEntered = false;
	public void OnDoorEntered(KinematicBody2D body){
		if(body.Name == "Player")
		{
			CheckKey();
		}
		
	}

    void DeleteSelf()
	{
		hasEntered = true;
		QueueFree();
	}

	void CheckKey(){
		Player.UseKey(id, DeleteSelf);
	}
}

using Godot;
using System;

public class PlayerSub : KinematicBody2D
{
	private const int MOVE_SPEED = 250;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	  public override void _Process(float delta)
	{
		Vector2 playerVec = new Vector2();

		if(Input.IsActionPressed("ui_left"))
		{
			playerVec = new Vector2(-1, 0);
		}
		else
			if(Input.IsActionPressed("ui_right"))
			{
				playerVec = new Vector2(1, 0);
			}

		MoveAndCollide(playerVec * delta * MOVE_SPEED);
	}
}

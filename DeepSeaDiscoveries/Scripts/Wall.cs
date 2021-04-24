using Godot;
using System;

public class Wall : KinematicBody2D
{
	private const int MOVE_SPEED = 100;
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
		var moveVec = new Vector2(0, -1f);
		MoveAndCollide(moveVec * delta * MOVE_SPEED);
	}
}

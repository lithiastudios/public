using Godot;
using System;

public class GameWorld : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private Wall LeftWall;
	private Wall LeftWall2;

	private Wall RightWall;
	private Wall RightWall2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LeftWall = GetNode<Wall>("LeftWall");
		LeftWall2 = GetNode<Wall>("LeftWall2");

		RightWall = GetNode<Wall>("RightWall");
		RightWall2 = GetNode<Wall>("RightWall2");
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}

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

		LeftWall.GlobalPosition = new Vector2(LeftWall.GlobalPosition.x, 0);
		RightWall.GlobalPosition = new Vector2(RightWall.GlobalPosition.x, 0);

		LeftWall2.GlobalPosition = new Vector2(LeftWall.GlobalPosition.x, LeftWall2.CollisionHeight*2);
		RightWall2.GlobalPosition = new Vector2(RightWall.GlobalPosition.x, RightWall2.CollisionHeight * 2);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}

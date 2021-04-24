using Godot;
using System;

public class Wall : KinematicBody2D
{
	private const int MOVE_SPEED = 100;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private Rect2 ViewPortSize;

	private CollisionShape2D CollisionShape;
	public float CollisionHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ViewPortSize = GetViewportRect();
		GD.Print("CurrentPos GLobal:" + GlobalPosition);

		GD.Print("CurrentPos local:" + Position);
		CollisionShape = GetNode<CollisionShape2D>("Collision");
		var shape = CollisionShape.Shape as RectangleShape2D;
		CollisionHeight = shape.Extents.y;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	  public override void _Process(float delta)
	{
		var moveVec = new Vector2(0, -1f);
		MoveAndCollide(moveVec * delta * MOVE_SPEED);
	///	GD.Print("VPRect:" + ViewPortSize);

		if(GlobalPosition.y <= -ViewPortSize.Size.y)
		{
			var resetPosition = new Vector2(GlobalPosition.x, ViewPortSize.Size.y + CollisionHeight + 100);// ViewPortSize.Size.y) ;
			GD.Print("Resetting wall to bottom: " + resetPosition + ": WAS: " + GlobalPosition.y);

		  GlobalPosition = resetPosition;
		}
	}
}

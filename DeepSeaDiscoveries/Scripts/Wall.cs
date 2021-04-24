using Godot;
using System;

public class Wall : StaticBody2D
{
	private const int MOVE_SPEED = 100;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	
	private CollisionShape2D CollisionShape;
	public float CollisionHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionShape = GetNode<CollisionShape2D>("Collision");
		var shape = CollisionShape.Shape as RectangleShape2D;
		CollisionHeight = shape.Extents.y;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	  public override void _Process(float delta)
	{
		var moveVec = new Vector2(0, -1f);
	
		Translate(moveVec * delta * MOVE_SPEED);
		///	GD.Print("VPRect:" + ViewPortSize);

		if (GlobalPosition.y <= -CollisionHeight) 
		{
			var resetPosition = new Vector2(GlobalPosition.x, GetViewportRect().Size.y + CollisionHeight );// ViewPortSize.Size.y) ;
			GD.Print(DateTime.Now + "Resetting wall to bottom: " + resetPosition + ": WAS: " + GlobalPosition.y);

		  GlobalPosition = resetPosition;
		}
	}
}

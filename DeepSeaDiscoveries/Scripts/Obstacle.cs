using DeepSeaDiscoveries;
using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;

public class Obstacle : Node2D
{
	private CollisionShape2D CollisionShape;
	public float CollisionHeight;
	public float CollisionWidth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionShape = GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
		var shape = CollisionShape.Shape as RectangleShape2D;
		CollisionHeight = shape.Extents.y;
		CollisionWidth = shape.Extents.x;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
  {
		if (!GlobalManager.IsGameStopped(this))
		{
			var moveVec = new Vector2(0, -1f);

			Translate(moveVec * delta * GameConstants.BACKGROUND_MOVE_SPEED);
		}
	}
}

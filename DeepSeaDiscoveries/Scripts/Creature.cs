using Godot;
using System;
using DeepSeaDiscoveries;
using DeepSeaDiscoveries.Scripts.Managers;

public class Creature : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private CollisionShape2D CollisionShape;
	private float CollisionHeight;
	private bool Caught;

	private Sprite CreatureSprite;

	public int Cost;
	public string SpriteName;

	public void Init(int cost, string spriteName)
	{
		Cost = cost;
		SpriteName = spriteName;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CreatureSprite = GetNode<Sprite>("Sprite");
		var tex = ResourceLoader.Load<Texture>("res://Sprites/" + SpriteName + ".png");
		CreatureSprite.Texture = tex;
		CollisionShape = GetNode<CollisionShape2D>("HurtBox/Collision");
		var shape = CollisionShape.Shape as RectangleShape2D;
		CollisionHeight = shape.Extents.y;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!Caught && !GlobalManager.IsGameStopped(this))
		{
			var moveVec = new Vector2(0f, -.25f);

			MoveAndSlide(moveVec * GameConstants.BACKGROUND_MOVE_SPEED);

			if (GlobalPosition.y <= -CollisionHeight)
			{
				GD.Print("Creature moved offscreen.");
				QueueFree();
			}
		}
	}

  private void _on_HurtBox_area_entered(object area)
  {
	GD.Print("Creature GET!");

		Caught = true;

		var playerSub = GlobalManager.GetGlobalGameVariables(this).PlayerSub;
		playerSub.CreatureGet(this, Cost, SpriteName);

		// Replace with function body.
	}

}



using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;

public class GameWorld : Node2D
{
	private const int TimeBetweenObstaclesSeconds = 2;
	private const int TimeBetweenCreaturesSeconds = 2;

	private Wall LeftWall;
	private Wall LeftWall2;

	private Wall RightWall;
	private Wall RightWall2;

	private float GameTime;

	RandomNumberGenerator RandomNumberGenerator;

	PlayerSub PlayerSub;

	DateTime LastObstacle;
	DateTime LastCreature;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OS.CenterWindow();
		LastObstacle = DateTime.Now;
		LastCreature = DateTime.Now;

		PlayerSub = (PlayerSub)GetNode("PlayerSub");

		var globalVars = GlobalManager.GetGlobalGameVariables(this);
		globalVars.PlayerSub = PlayerSub;

		RandomNumberGenerator = new RandomNumberGenerator();
		RandomNumberGenerator.Randomize();

		LeftWall = GetNode<Wall>("LeftWall");
		LeftWall2 = GetNode<Wall>("LeftWall2");

		RightWall = GetNode<Wall>("RightWall");
		RightWall2 = GetNode<Wall>("RightWall2");

		LeftWall.GlobalPosition = new Vector2(LeftWall.GlobalPosition.x, 0);
		RightWall.GlobalPosition = new Vector2(RightWall.GlobalPosition.x, 0);

		LeftWall2.GlobalPosition = new Vector2(LeftWall.GlobalPosition.x, LeftWall2.CollisionHeight * 2);
		RightWall2.GlobalPosition = new Vector2(RightWall.GlobalPosition.x, RightWall2.CollisionHeight * 2);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var randomNumber = RandomNumberGenerator.RandiRange(0, 100);

		if (randomNumber == 0 && (DateTime.Now - LastObstacle).Seconds >= TimeBetweenObstaclesSeconds)
		{
			var leftRightRandom = RandomNumberGenerator.RandiRange(0, 1);
			var obstacle = GD.Load<PackedScene>("res://Scenes/Obstacle.tscn");
			var instance = obstacle.Instance() as Obstacle;
			if (leftRightRandom == 0)
			{
				instance.Position = new Vector2(LeftWall.GlobalPosition.x + 75, GetViewportRect().Size.y);
			}
			else
			{
				instance.Position = new Vector2(RightWall.GlobalPosition.x - 75, GetViewportRect().Size.y);
			}
			AddChild(instance);
			LastObstacle = DateTime.Now;
		}

		if (randomNumber > 99 && (DateTime.Now - LastCreature).Seconds >= TimeBetweenCreaturesSeconds)
		{
			var creature = GD.Load<PackedScene>("res://Scenes/Creature.tscn");
			var instance = creature.Instance() as Creature;
			instance.Position = new Vector2(LeftWall.GlobalPosition.x + 150, GetViewportRect().Size.y - 120);
			AddChild(instance);
			LastCreature = DateTime.Now;
		}
		//GameTime += delta;
	}
}

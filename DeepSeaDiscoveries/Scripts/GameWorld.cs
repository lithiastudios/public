using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;

public class GameWorld : Node2D
{
	private const int TimeBetweenObstaclesSeconds = 2;
	private const int TimeBetweenCreaturesSeconds = 2;

	private Label DepthLabel;

	
	private CanvasLayer ScoreCanvas;

	private Wall LeftWall;
	private Wall LeftWall2;

	private Wall RightWall;
	private Wall RightWall2;

	private Timer GameOverTimer;
	
	private DateTime TimeStarted;
	RandomNumberGenerator RandomNumberGenerator;

	PlayerSub PlayerSub;

	DateTime LastObstacle;
	DateTime LastCreature;

	private AnimationPlayer CircleWipeAnimationPlayer;
	private int Depth;

	private bool GameIsStarting;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TimeStarted = DateTime.Now;
		OS.CenterWindow();
		LastObstacle = DateTime.Now;
		LastCreature = DateTime.Now;

		ScoreCanvas = GetNode<CanvasLayer>("ScoreCanvas");

		DepthLabel = GetNode<Label>("ScoreCanvas/DepthLabel");
		GameOverTimer = GetNode<Timer>("GameOverTimer");
		PlayerSub = (PlayerSub)GetNode("PlayerSub");
		CircleWipeAnimationPlayer = (AnimationPlayer)GetNode("CircleWipe/ColorRect/AnimationPlayer");

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
		
		GameIsStarting = true;
		CircleWipeAnimationPlayer.Play("circle_out");
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!GlobalManager.IsGameStopped(this))
		{
			var randomNumber = RandomNumberGenerator.RandiRange(0, 100);

			if (randomNumber == 0 && (DateTime.Now - LastObstacle).Seconds >= TimeBetweenObstaclesSeconds)
			{
				var leftRightRandom = RandomNumberGenerator.RandiRange(0, 1);

				AddObstacle(leftRightRandom == 0);
				LastObstacle = DateTime.Now;
			}

			if (randomNumber > 99 && (DateTime.Now - LastCreature).Seconds >= TimeBetweenCreaturesSeconds)
			{
				var creature = GD.Load<PackedScene>("res://Scenes/Creature.tscn");
				var instance = creature.Instance() as Creature;
				
				instance.Position = new Vector2(LeftWall.GlobalPosition.x + 150, GetViewportRect().Size.y);
				instance.Init(45, "fish1");

				AddChild(instance);
				LastCreature = DateTime.Now;
			}
	
			var numSecs = (DateTime.Now - TimeStarted).Seconds;
			Depth =  (int)(numSecs * 1.5f);
			DepthLabel.Text = Depth.ToString() + "m";
		}

		//GameTime += delta;
	}

	private void AddObstacle(bool isLeft)
	{
		var obstacleName = isLeft ? "LeftObstacle" : "RightObstacle";
		var obstacle = GD.Load<PackedScene>("res://Scenes/" + obstacleName + ".tscn");
		var instance = obstacle.Instance() as Obstacle;
		if (isLeft)
		{
			instance.Position = new Vector2(LeftWall.GlobalPosition.x + LeftWall.CollisionWidth/2 + instance.CollisionWidth/2, GetViewportRect().Size.y);
		}
		else
		{
			instance.Position = new Vector2(RightWall.GlobalPosition.x - RightWall.CollisionWidth/2 + instance.CollisionWidth/2, GetViewportRect().Size.y);
		}
		AddChild(instance);
	}
	
	
	private void _on_PlayerSub_SubIsDead()
	{
		GameOverTimer.Start();
	}
	
	
private void _on_GameOverTimer_timeout()
{
		CircleWipeAnimationPlayer.Play("circle_in");

	// Replace with function body.
}
	private void _on_CircleWipe_CircleWipeComplete()
	{
		if (GameIsStarting)
		{
			GlobalManager.StartGame(this);
			GD.Print("Starting game");
			GD.Print("is game stopped:" + GlobalManager.IsGameStopped(this));
			GameIsStarting = false;
		}
		else
		{
			// level over..

			GetTree().ChangeScene("res://Scenes/Surface.tscn");
		}

	}

}





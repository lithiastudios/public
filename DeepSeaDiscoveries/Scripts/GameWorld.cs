using Godot;
using System;

public class GameWorld : Node2D
{
	private const int TimeBetweenObstaclesSeconds = 5;

	private Wall LeftWall;
	private Wall LeftWall2;

	private Wall RightWall;
	private Wall RightWall2;

	private float GameTime;

	RandomNumberGenerator RandomNumberGenerator;

	DateTime LastObstacleLeft;
	DateTime LastObstacleRight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator = new RandomNumberGenerator();
		RandomNumberGenerator.Randomize();

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
	  public override void _Process(float delta)
	  {
		  var randomNumber = RandomNumberGenerator.RandiRange(0, 100);

		  if(randomNumber == 0 && (DateTime.Now - LastObstacleLeft).Seconds >= TimeBetweenObstaclesSeconds)
		  {
			var obstacle = GD.Load<PackedScene>("res://Scenes/Obstacle.tscn");
			var instance = obstacle.Instance() as Obstacle;
			instance.Position = new Vector2(LeftWall.GlobalPosition.x + 75, GetViewportRect().Size.y);
			AddChild(instance);
			LastObstacleLeft = DateTime.Now;
		  }
  		  //GameTime += delta;
	  }
}

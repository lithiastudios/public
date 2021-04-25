using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;

public class Surface : Node2D
{
	private  Array Water1MarginTops = new int[] { 410, 405 };
	private  Array Water2MarginTops = new int[] { 420, 415 };
	private  Array Water3MarginTops = new int[] { 434, 429 };

	private Array SubBobPositions = new float[] { 400, 395 };

	private AnimationPlayer CircleWipeAnimationPlayer;
	private PlayerSub PlayerSub;

	private Tween SubBobTween;

	private ColorRect Water1Rect;
	private Tween Water1Tween;

	private ColorRect Water2Rect;
	private Tween Water2Tween;

	private ColorRect Water3Rect;
	private Tween Water3Tween;  // Called when the node enters the scene tree for the first time.

	private Timer LaunchTimer;

	private Label LaunchLabel;

	private int TimeOut = 3;

	public override void _Ready()
	{
		LaunchTimer = GetNode<Timer>("LaunchTimer");

		LaunchLabel = GetNode<Label>("CanvasLayer2/Label");
		SubBobTween = GetNode<Tween>("SubBobTween");
		Water1Tween = GetNode<Tween>("Water1GrowTween");
		Water1Rect = GetNode<ColorRect>("Water1Rect");

		Water2Tween = GetNode<Tween>("Water2GrowTween");
		Water2Rect = GetNode<ColorRect>("Water2Rect");

		Water3Tween = GetNode<Tween>("Water3GrowTween");
		Water3Rect = GetNode<ColorRect>("Water3Rect");

		PlayerSub = GetNode<PlayerSub>("PlayerSub");

		StartPlayerBobTween();
		StartWater1Tween();
		StartWater2Tween();
		StartWater3Tween();

		PlayerSub.OnSurface = true;
		PlayerSub.StartBubbler(false);

		CircleWipeAnimationPlayer = (AnimationPlayer)GetNode("CircleWipe/ColorRect/AnimationPlayer");
	}

	private void StartPlayerBobTween()
	{
		SubBobTween.InterpolateProperty(PlayerSub, "position:y", SubBobPositions.GetValue(0), SubBobPositions.GetValue(1), 1);
		SubBobTween.Start();
	}

	private void StartWater1Tween()
	{
		Water1Tween.InterpolateProperty(Water1Rect, "margin_top", Water1MarginTops.GetValue(0), Water1MarginTops.GetValue(1), 1);
		Water1Tween.Start();
	}

	private void StartWater2Tween()
	{
		Water2Tween.InterpolateProperty(Water2Rect, "margin_top", Water2MarginTops.GetValue(0), Water2MarginTops.GetValue(1), 1.1f);
		Water2Tween.Start();
	}

	private void StartWater3Tween()
	{
		Water3Tween.InterpolateProperty(Water3Rect, "margin_top", Water3MarginTops.GetValue(0), Water3MarginTops.GetValue(1), 1.25f);
		Water3Tween.Start();
	}   //  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }


	private void _on_Water1GrowTween_tween_completed(Godot.Object @object, NodePath key)
   {
		Array.Reverse(Water1MarginTops);
		StartWater1Tween();
	}


	private void _on_Water2GrowTween_tween_completed(Godot.Object @object, NodePath key)
	{
		Array.Reverse(Water2MarginTops);
		StartWater2Tween();
	}


	private void _on_Water3GrowTween_tween_completed(Godot.Object @object, NodePath key)
{
		Array.Reverse(Water3MarginTops);
		StartWater3Tween();
	}

	private void _on_SubBobTween_tween_completed(Godot.Object @object, NodePath key)
	{
		Array.Reverse(SubBobPositions);
		StartPlayerBobTween();
		// Replace with function body.
	}


	private void _on_LaunchTimer_timeout()
	{
		if (TimeOut > 0)
		{
			LaunchTimer.Start();
			TimeOut--;
			LaunchLabel.Text = "Launching In  " + TimeOut + " !";
		}
		else
		{
			LaunchSub();
		}			
		// Replace with function body.
	}

	private void LaunchSub()
	{
		LaunchTimer.Stop();
		GlobalManager.StopGame(this);

		CircleWipeAnimationPlayer.Play("circle_in");

	}

	private void _on_Area2D_area_entered(object area)
	{
		LaunchTimer.Start();
		TimeOut = 3;
		LaunchLabel.Text = "Launching In   3 !";
		// Replace with function body.
	}


	private void _on_Area2D_area_exited(object area)
	{
		LaunchTimer.Stop();
		TimeOut = 3;
		LaunchLabel.Text = "Sub  Launch";
		// Replace with function body.
	}
}








using Godot;
using System;

public class PlayerSub : KinematicBody2D
{
	private const int MOVE_SPEED = 250;

	private Node2D Hook;
	private Tween HookLaunch;
	private Tween HookRetract;

	private bool Moveable;
	private Vector2 CurrentHookPosition;
	
	public override void _Ready()
	{
		Moveable = true;
		Hook = GetNode<Node2D>("Hook");
		HookLaunch = GetNode<Tween>("HookLaunch");
		HookRetract = GetNode<Tween>("HookRetract");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	  public override void _Process(float delta)
	{
		Vector2 playerVec = new Vector2();

		if(Input.IsActionPressed("ui_left") && Moveable)
		{
			playerVec = new Vector2(-1, 0);
		}
		else
			if(Input.IsActionPressed("ui_right")  && Moveable)
			{
				playerVec = new Vector2(1, 0);
			}
			else
			if(Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("ui_select") && Moveable)
			{
			GD.Print("activated");
			var currentHookPos = Hook.Position;

			CurrentHookPosition = currentHookPos;
			HookLaunch.InterpolateProperty(Hook, "position", currentHookPos, new Vector2(currentHookPos.x, currentHookPos.y + 100), 1, Tween.TransitionType.Linear, Tween.EaseType.InOut);
			HookLaunch.Start();
			}
		MoveAndCollide(playerVec * delta * MOVE_SPEED);
	}
	
	
	private void _on_HookLaunch_tween_completed(Godot.Object @object, NodePath key)
	{
		GD.Print("retracting..");
		var currentHookPos = Hook.Position;
		HookRetract.InterpolateProperty(Hook, "position", currentHookPos, new Vector2(CurrentHookPosition.x, CurrentHookPosition.y), 1, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		HookRetract.Start();
		// Replace with function body.
	}
	
	
private void _on_HookRetract_tween_completed(Godot.Object @object, NodePath key)
{
		Moveable = true;
}

}




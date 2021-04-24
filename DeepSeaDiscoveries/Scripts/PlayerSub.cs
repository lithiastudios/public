using Godot;
using System;

public class PlayerSub : KinematicBody2D
{
	private const int MOVE_SPEED = 250;

	private Node2D Hook;
	private Tween HookLaunch;
	private Tween HookRetract;

	private bool Moveable;
	private Vector2 OriginalHookPosition;
	
	public override void _Ready()
	{
		Moveable = true;
		Hook = GetNode<Node2D>("Hook");
		HookLaunch = GetNode<Tween>("HookLaunch");
		HookRetract = GetNode<Tween>("HookRetract");
		OriginalHookPosition = Hook.Position;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	  public override void _Process(float delta)
	{
		Vector2 playerVec = new Vector2();

		if(Input.IsActionPressed("ui_left"))
		{
			playerVec = new Vector2(-1, 0);
		}
		
			if(Input.IsActionPressed("ui_right"))
			{
				playerVec = new Vector2(1, 0);
			}
			
			if(Input.IsActionPressed("ui_accept") || Input.IsActionPressed("ui_select") && Moveable)
			{
				Moveable = false;
			GD.Print("activated");

			HookLaunch.InterpolateProperty(Hook, "position", OriginalHookPosition, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y + 100), 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
			HookLaunch.Start();
			}
		MoveAndCollide(playerVec * delta * MOVE_SPEED);
	}
	
	
	private void _on_HookLaunch_tween_completed(Godot.Object @object, NodePath key)
	{
		GD.Print("retracting..");
		var currentHookPos = Hook.Position;
		HookRetract.InterpolateProperty(Hook, "position", currentHookPos, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y), 1, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		HookRetract.Start();
		// Replace with function body.
	}
	
	
private void _on_HookRetract_tween_completed(Godot.Object @object, NodePath key)
{
		Moveable = true;
}

}




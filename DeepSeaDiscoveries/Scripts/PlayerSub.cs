using Godot;
using System;

public class PlayerSub : KinematicBody2D
{
	private const int MOVE_SPEED = 250;

	private Node2D Hook;
	private Tween HookLaunch;
	private Tween HookRetract;

	private bool Hookable;
	private bool HasCreature;

	private Creature CurrentCreature;

	private Vector2 OriginalHookPosition;
	
	public override void _Ready()
	{
		Hookable = true;
		Hook = GetNode<Node2D>("Hook");
		HookLaunch = GetNode<Tween>("HookLaunch");
		HookRetract = GetNode<Tween>("HookRetract");
		OriginalHookPosition = Hook.Position;
	}


	public void CreatureGet(Creature creature)
	{
		GD.Print("Hook retracting due to creature get!");

		HasCreature = true;
		CurrentCreature = creature;
		var oldParent = GetParent();
		oldParent.RemoveChild(creature);

		HookLaunch.Stop(Hook);
		Hook.AddChild(creature);
		creature.GlobalPosition = Hook.GlobalPosition;
		RetractHook();
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
			
			if((Input.IsActionPressed("ui_accept") || Input.IsActionPressed("ui_select")) && Hookable)
			{
				Hookable = false;
			GD.Print("activated");

			HookLaunch.InterpolateProperty(Hook, "position", OriginalHookPosition, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y + 100), 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
			HookLaunch.Start();
			}
		MoveAndCollide(playerVec * delta * MOVE_SPEED);
	}
	
	private void RetractHook()
	{
		GD.Print("retracting..");
		var currentHookPos = Hook.Position;
		HookRetract.InterpolateProperty(Hook, "position", currentHookPos, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y), 1, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		HookRetract.Start();
	}

	private void _on_HookLaunch_tween_completed(Godot.Object @object, NodePath key)
	{
		RetractHook();
	}
	
	
	private void _on_HookRetract_tween_completed(Godot.Object @object, NodePath key)
	{
		Hookable = true;

		if(HasCreature && CurrentCreature != null)
		{
			Hook.RemoveChild(CurrentCreature);
			CurrentCreature.QueueFree();
		}
		HasCreature = false;
	}

}




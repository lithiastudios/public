using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;
using System.Collections.Generic;

public class PlayerSub : KinematicBody2D
{
	[Signal]
	public delegate void SubIsDead();

	private const int MOVE_SPEED = 250;

	private Node2D Hook;
	private ColorRect HookChain;

	private Tween HookLaunch;
	private Tween HookRetract;


	private Tween HookChainLaunch;
	private Tween HookChainRetract;

	private bool Hookable;
	private bool HasCreature;

	private Particles2D CrashParticles;
	private Particles2D Bubbler;

	private Creature CurrentCreature;

	private float OriginalHookChainHeight;
	private Vector2 OriginalHookPosition;

	private bool GameIsStopped;

	private Dictionary<string, int> CreaturesCaught;
	private Dictionary<string, int> CreaturesCost;


	public override void _Ready()
	{
		CreaturesCaught = new Dictionary<string, int>();
		CreaturesCost = new Dictionary<string, int>();
		Hookable = true;
		Hook = GetNode<Node2D>("Hook");
		HookChain = GetNode<ColorRect>("HookChain");
		OriginalHookChainHeight = HookChain.GetRect().Size.y;

		HookLaunch = GetNode<Tween>("HookLaunch");
		HookRetract = GetNode<Tween>("HookRetract");

		HookChainLaunch = GetNode<Tween>("HookChainLaunch");
		HookChainRetract = GetNode<Tween>("HookChainRetract");

		OriginalHookPosition = Hook.Position;
		CrashParticles = GetNode<Particles2D>("CrashParticles");
		Bubbler = GetNode<Particles2D>("Bubbler");
	}


	public void CreatureGet(Creature creature, int cost, string name)
	{
		if(!CreaturesCaught.ContainsKey(name))
		{
			CreaturesCaught.Add(name, 1);
		}
		else
		{
			CreaturesCaught[name] = CreaturesCaught[name]++;
		}

		if(!CreaturesCost.ContainsKey(name))
		{
			CreaturesCost[name] = cost;
		}

		GD.Print("Hook retracting due to creature get!");

		HasCreature = true;
		CurrentCreature = creature;
		var oldParent = GetParent();
		oldParent.RemoveChild(creature);

		HookLaunch.Stop(Hook);
		HookChainLaunch.Stop(this);
		Hook.AddChild(creature);
		creature.GlobalPosition = Hook.GlobalPosition;
		RetractHook();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Vector2 playerVec = new Vector2();

		if (!GameIsStopped)
		{
			if (Input.IsActionPressed("ui_left"))
			{
				playerVec = new Vector2(-1, 0);
			}

			if (Input.IsActionPressed("ui_right"))
			{
				playerVec = new Vector2(1, 0);
			}

			if ((Input.IsActionPressed("ui_accept") || Input.IsActionPressed("ui_select")) && Hookable)
			{
				Hookable = false;
				GD.Print("activated");

				int hookLength = 100;
				HookChainLaunch.InterpolateProperty(HookChain, "rect_size:y", HookChain.GetRect().Size.y, HookChain.GetRect().Size.y + hookLength, 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
				HookChainLaunch.Start();

				HookLaunch.InterpolateProperty(Hook, "position", OriginalHookPosition, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y + hookLength), 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
				HookLaunch.Start();
			}
			MoveAndCollide(playerVec * delta * MOVE_SPEED);
		}
	}
	
	private void RetractHook()
	{
		GD.Print("retracting..");
		var currentHookPos = Hook.Position;
		HookChainRetract.InterpolateProperty(HookChain, "rect_size:y", HookChain.GetRect().Size.y, OriginalHookChainHeight, 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
		HookChainRetract.Start();

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
	
	
private void _on_HitBox_area_entered(object area)
{

	GD.Print("SUB IS DEAD!");
		CrashParticles.Emitting = true;
		GlobalManager.StopGame(this);
		GameIsStopped = true;
		Bubbler.Emitting = false;

		EmitSignal(nameof(SubIsDead));

		foreach(var creature in CreaturesCaught)
		{
			GD.Print("Caught: " + creature.Value + " of " + creature.Key + " at $" + CreaturesCost[creature.Key]);
		}

		// Replace with function body.
	}

}






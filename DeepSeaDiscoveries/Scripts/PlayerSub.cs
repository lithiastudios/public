using DeepSeaDiscoveries.Scripts.Managers;
using Godot;
using System;
using System.Collections.Generic;

public class PlayerSub : KinematicBody2D
{
	[Signal]
	public delegate void SubIsDead();

	private const int MOVE_SPEED = 400;

	private CollisionShape2D HookHitBoxCollision;

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

	public bool OnSurface;
	private Creature CurrentCreature;

	private float OriginalHookChainHeight;
	private Vector2 OriginalHookPosition;

	private bool GameIsStopped;

	public Godot.Collections.Dictionary<string, int> CreaturesCaught;
	public Godot.Collections.Dictionary<string, int> CreaturesCost;

	private AudioStreamPlayer2D CrashAudio;
	private AudioStreamPlayer2D HookLaunchAudio;
	private AudioStreamPlayer2D PickupCreatureAudio;

	public override void _Ready()
	{
		CrashAudio = GetNode<AudioStreamPlayer2D>("CrashAudio");
		HookLaunchAudio = GetNode<AudioStreamPlayer2D>("HookLaunchAudio");
		PickupCreatureAudio = GetNode<AudioStreamPlayer2D>("PickupCreatureAudio");

		CreaturesCaught = new Godot.Collections.Dictionary<string, int>();
		CreaturesCost = new Godot.Collections.Dictionary<string, int>();
		Hookable = true;
		Hook = GetNode<Node2D>("Hook");
		HookChain = GetNode<ColorRect>("HookChain");
		OriginalHookChainHeight = HookChain.GetRect().Size.y;

		HookHitBoxCollision = GetNode<CollisionShape2D>("Hook/Claw/Area2D/CollisionShape2D");
		HookHitBoxCollision.Disabled = true;

		HookLaunch = GetNode<Tween>("HookLaunch");
		HookRetract = GetNode<Tween>("HookRetract");

		HookChainLaunch = GetNode<Tween>("HookChainLaunch");
		HookChainRetract = GetNode<Tween>("HookChainRetract");

		OriginalHookPosition = Hook.Position;
		CrashParticles = GetNode<Particles2D>("CrashParticles");
		Bubbler = GetNode<Particles2D>("Bubbler");
	}

	public void StartBubbler(bool started)
	{
		Bubbler.Emitting = started;
	}

	public void CreatureGet(Creature creature, int cost, string name)
	{
		PickupCreatureAudio.Play();
		if(!CreaturesCaught.ContainsKey(name))
		{
			GD.Print("Couldn't find:" + name + " setting to 1.");
			CreaturesCaught.Add(name, 1);
		}
		else
		{
			var current = CreaturesCaught[name];
			 current++;

			CreaturesCaught[name] = current++;
			GD.Print("New value: " + current);

			GD.Print("Did find:" + name + " setting to " + CreaturesCaught[name]);
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

			if (!OnSurface)
			{
				if ((Input.IsActionPressed("ui_accept") || Input.IsActionPressed("ui_select")) && Hookable)
				{
					Hookable = false;
					GD.Print("activated");

					int hookLength = 100;
					HookChainLaunch.InterpolateProperty(HookChain, "rect_size:y", HookChain.GetRect().Size.y, HookChain.GetRect().Size.y + hookLength, 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
					HookChainLaunch.Start();

					HookLaunch.InterpolateProperty(Hook, "position", OriginalHookPosition, new Vector2(OriginalHookPosition.x, OriginalHookPosition.y + hookLength), 1, Tween.TransitionType.Quad, Tween.EaseType.InOut);
					HookLaunch.Start();

					HookLaunchAudio.Play();
					HookHitBoxCollision.Disabled = false;
				}
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
		HookHitBoxCollision.Disabled = true;
	}
	
	
private void _on_HitBox_area_entered(object area)
{
		if (!OnSurface)
		{
			CrashAudio.Position = Position;
			CrashAudio.Play();
			GD.Print("SUB IS DEAD!");
			CrashParticles.Emitting = true;
			GlobalManager.StopGame(this);
			GameIsStopped = true;
			Bubbler.Emitting = false;

			EmitSignal(nameof(SubIsDead));
		}
		// Replace with function body.
	}

}








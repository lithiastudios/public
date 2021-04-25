using Godot;
using System;

public class CircleWipe : CanvasLayer
{
	[Signal]
	public delegate void CircleWipeComplete();

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void OnAnimationComplete()
	{
		GD.Print("Circle wipe complete!");
		EmitSignal(nameof(CircleWipeComplete));
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeaDiscoveries.Scripts
{
	public class GlobalGameVariables : Node2D
	{
		public PlayerSub PlayerSub;
		public bool IsGameStopped;
		public bool LevelHasBeenPlayed;

		public override void _Ready()
		{

		}
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSeaDiscoveries.Scripts.Managers
{
    public static class GlobalManager
    {
		public static GlobalGameVariables GetGlobalGameVariables(Node2D currentNode)
		{
			var globalVars = (GlobalGameVariables)currentNode.GetNode("/root/GlobalGameVariables");

			return globalVars;
		}

		public static void SetLevelHasBeenPlayed(Node2D currentNode)
        {
			var globalVars = GetGlobalGameVariables(currentNode);
			globalVars.LevelHasBeenPlayed = true;
		}

		public static bool LevelHasBeenPlayed(Node2D currentNode)
		{
			var globalVars = GetGlobalGameVariables(currentNode);
			return globalVars.LevelHasBeenPlayed;
		}

		public static void SetPlayerMoney(Node2D currentNode, int money)
		{
			var globalVars = GetGlobalGameVariables(currentNode);
			globalVars.PlayerMoney = money;
		}

		public static int GetPlayerMoney(Node2D currentNode)
		{
			var globalVars = GetGlobalGameVariables(currentNode);
			return globalVars.PlayerMoney;
		}


		public static void StopGame(Node2D currentNode)
        {
            SetGameStatus(currentNode, true);
        }

        private static void SetGameStatus(Node2D currentNode, bool isGameStopped)
        {
			var globalVars = GetGlobalGameVariables(currentNode);
			globalVars.IsGameStopped = isGameStopped;
        }

        public static void StartGame(Node2D currentNode)
		{
			SetGameStatus(currentNode, false);
		}


		public static bool IsGameStopped(Node2D currentNode)
		{
			var globalVars = GetGlobalGameVariables(currentNode);
			return globalVars.IsGameStopped;
		}
	}
}

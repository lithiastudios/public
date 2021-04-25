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

		public static void StopGame(Node2D currentNode)
        {
            SetGameStatus(currentNode, false);
        }

        private static void SetGameStatus(Node2D currentNode, bool isGameStopped)
        {
            var globalVars = (GlobalGameVariables)currentNode.GetNode("/root/GlobalGameVariables");
            globalVars.IsGameStopped = isGameStopped;
        }

        public static void StartGame(Node2D currentNode)
		{
			SetGameStatus(currentNode, true);
		}


		public static bool IsGameStopped(Node2D currentNode)
		{
			var globalVars = (GlobalGameVariables)currentNode.GetNode("/root/GlobalGameVariables");

			return globalVars.IsGameStopped;
		}
	}
}

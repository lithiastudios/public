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
	}
}

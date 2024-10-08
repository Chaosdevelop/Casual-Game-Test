using System;
using Game.Resources;

namespace Game.Buildings
{
	[Serializable]
	public struct ResourceRecipe
	{
		public ResourceType[] input;
		public ResourceType[] output;
		public float time;
	}
}
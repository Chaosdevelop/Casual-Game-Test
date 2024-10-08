using UnityEngine;

namespace Game.Buildings
{
	[CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData")]
	public class BuildingData : ScriptableObject
	{
		[field: SerializeField]
		public Building BuildingViewPrefab { get; private set; }
		[field: SerializeField]
		public ResourceRecipe ResourceRecipe { get; private set; }

	}
}
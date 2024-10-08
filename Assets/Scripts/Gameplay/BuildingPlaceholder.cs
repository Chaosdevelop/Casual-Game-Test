using UnityEngine;

namespace Game.Buildings
{
	/// <summary>
	/// Represents a placeholder for a building in the scene.
	/// </summary>
	public class BuildingPlaceholder : MonoBehaviour
	{

		[field: SerializeField]
		public BuildingData BuildingData { get; private set; }

		public Transform SpawnPoint => transform;
	}
}
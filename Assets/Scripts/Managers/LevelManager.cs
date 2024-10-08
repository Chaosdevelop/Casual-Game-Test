using Game.Buildings;
using UnityEngine;
using Zenject;

namespace Game.Levels
{
	/// <summary>
	/// Manages the initialization of buildings in the level.
	/// </summary>
	public class LevelManager : MonoBehaviour
	{
		[SerializeField]
		private BuildingPlaceholder[] _buildingPlaceholders;

		[Inject]
		private IFactory<BuildingPlaceholder, Building> _buildingFactory;

		private void Start()
		{
			foreach (var placeholder in _buildingPlaceholders)
			{
				_buildingFactory.Create(placeholder);
			}
		}
	}
}
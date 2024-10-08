using UnityEngine;
using Zenject;

namespace Game.Buildings
{
	/// <summary>
	/// Factory for creating building instances.
	/// </summary>
	public class BuildingFactory : IFactory<BuildingPlaceholder, Building>
	{
		[Inject]
		private DiContainer _container;

		/// <inheritdoc/>
		public Building Create(BuildingPlaceholder buildingPlaceholder)
		{
			Building instance = Object.Instantiate(buildingPlaceholder.BuildingData.BuildingViewPrefab,
				buildingPlaceholder.SpawnPoint);

			_container.Inject(instance);
			instance.name = buildingPlaceholder.BuildingData.BuildingViewPrefab.name;
			instance.Initialize(buildingPlaceholder.BuildingData);

			return instance;
		}
	}
}
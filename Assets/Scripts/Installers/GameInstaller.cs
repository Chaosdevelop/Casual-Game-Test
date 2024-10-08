using Game.Buildings;
using Game.Core;
using Game.Levels;
using Game.Resources;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
	/// <summary>
	/// Installs game dependencies using Zenject.
	/// </summary>
	public class GameInstaller : MonoInstaller
	{
		[SerializeField]
		private ResourceBlock _resourceBlockPrefab;
		[SerializeField]
		private Warehouse _warehousePrefab;

		private const int ResourceBlockPoolSize = 10;
		public override void InstallBindings()
		{

			Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<IGameLog>().FromComponentInHierarchy().AsSingle();
			Container.Bind<IFactory<BuildingPlaceholder, Building>>().To<BuildingFactory>().AsSingle();
			Container.BindFactory<ResourceType, ResourceBlock, PlaceholderFactory<ResourceType, ResourceBlock>>().FromComponentInNewPrefab(_resourceBlockPrefab);
			Container.BindFactory<Transform, ResourceType[], WarehouseType, Warehouse, PlaceholderFactory<Transform, ResourceType[], WarehouseType, Warehouse>>().FromComponentInNewPrefab(_warehousePrefab);
		}
	}
}
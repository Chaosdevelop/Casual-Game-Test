using System;
using System.Collections.Generic;
using System.Linq;
using Game.Animations;
using Game.Resources;
using UnityEngine;
using Zenject;


namespace Game.Buildings
{

	/// <summary>
	/// Represents a warehouse for storing resources.
	/// </summary>
	public class Warehouse : MonoBehaviour, IResourceStorage
	{
		[field: SerializeField]
		public int Capacity { get; private set; }
		[SerializeField]
		private float _subcontainersHeight = 1;

		public int Quantity => _resourceBlocks.Count;

		public WarehouseType WarehouseType { get; private set; }


		private HashSet<ResourceType> _allowedResources;
		private List<ResourceBlock> _resourceBlocks;
		private Transform[] _subcontainers;


		public event Action OnResourcesChanged;

		[Inject]
		private void Initialize(Transform parent, ResourceType[] allowedResources, WarehouseType warehouseType)
		{
			transform.SetParent(parent, false);
			_resourceBlocks = new List<ResourceBlock>(Capacity);
			_allowedResources = new HashSet<ResourceType>(allowedResources);
			WarehouseType = warehouseType;
			CreateSubcontainers();
		}


		private void CreateSubcontainers()
		{
			_subcontainers = new Transform[Capacity];
			for (int i = 0; i < Capacity; i++)
			{
				var subcontainer = new GameObject($"Subcontainer {i}").transform;
				subcontainer.SetParent(transform, false);
				subcontainer.localPosition = new Vector3(0, (i + 0.5f) * _subcontainersHeight, 0);
				_subcontainers[i] = subcontainer;
			}
		}


		/// <inheritdoc/>
		public bool CanReceive(ResourceType resourceValue)
		{
			int freeSpace = Capacity - Quantity;
			return freeSpace > 0 && _allowedResources.Contains(resourceValue);
		}



		/// <inheritdoc/>
		public bool Contains(ResourceType resourceValue)
		{
			return _resourceBlocks.Any(block => block.Resource == resourceValue);
		}


		/// <inheritdoc/>
		public ITransferOperation TryTransferTo(IResourceStorage otherStorage)
		{
			foreach (var block in _resourceBlocks)
			{
				if (otherStorage.CanReceive(block.Resource))
				{
					return TransferTo(otherStorage, block.Resource);
				}
			}

			return null;
		}


		/// <inheritdoc/>
		public ITransferOperation TransferTo(IResourceTransferPoint otherPoint, ResourceType resourceValue)
		{
			var block = _resourceBlocks.LastOrDefault(arg => arg.Resource == resourceValue);

			if (block == null) return null;

			RemoveBlock(block);


			var transfer = new LerpTransferAnimation();
			transfer.SetTransferData(() => block.transform, otherPoint.GetDestination, 0.5f, block,
				() => otherPoint.AddBlock(block));


			return transfer;
		}


		/// <inheritdoc/>
		public void AddBlock(ResourceBlock resourceBlock)
		{
			_resourceBlocks.Add(resourceBlock);
			OnResourcesChanged?.Invoke();
		}


		/// <inheritdoc/>
		public void RemoveBlock(ResourceBlock resourceBlock)
		{
			_resourceBlocks.Remove(resourceBlock);
			OnResourcesChanged?.Invoke();
		}

		/// <inheritdoc/>
		public Transform GetDestination()
		{
			return _subcontainers[Quantity];
		}

		public class Factory : PlaceholderFactory<Transform, ResourceType[], WarehouseType, Warehouse> { }

	}
}
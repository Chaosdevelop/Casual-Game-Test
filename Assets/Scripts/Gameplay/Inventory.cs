using System;
using System.Collections.Generic;
using System.Linq;
using Game.Animations;
using Game.Buildings;
using Game.Resources;
using UnityEngine;


namespace Game.Player
{
	/// <summary>
	/// Represents an inventory for storing resources.
	/// </summary>
	public class Inventory : MonoBehaviour, IResourceStorage
	{
		[field: SerializeField]
		public int Capacity { get; private set; }
		[SerializeField]
		private float _subcontainersHeight = 1;

		public int Quantity => _resourceBlocks.Count;

		private List<ResourceBlock> _resourceBlocks;
		private Transform[] _subcontainers;

		private ITransferOperation _transferOperation;

		/// <summary>
		/// Initializes the inventory.
		/// </summary>
		public void Initialize()
		{
			_resourceBlocks = new List<ResourceBlock>(Capacity);
			CreateSubcontainers();
		}

		private void Update()
		{
			if (_transferOperation != null)
			{
				_transferOperation.Update(Time.deltaTime);

				if (_transferOperation.IsCompleted)
				{
					_transferOperation = null;
				}
			}
		}

		/// <inheritdoc/>
		public bool CanReceive(ResourceType resourceValue)
		{
			int freeSpace = Capacity - Quantity;
			return freeSpace > 0;
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
		public bool Contains(ResourceType resourceValue)
		{
			return _resourceBlocks.Any(block => block.Resource == resourceValue);
		}

		/// <inheritdoc/>
		public event Action OnResourcesChanged;

		/// <inheritdoc/>
		public ITransferOperation TransferTo(IResourceTransferPoint otherPoint, ResourceType resourceValue)
		{
			var block = _resourceBlocks.LastOrDefault(arg => arg.Resource == resourceValue);
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
		public ITransferOperation TryTransferTo(IResourceStorage otherStorage)
		{
			foreach (var block in _resourceBlocks)
			{
				if (otherStorage.CanReceive(block.Resource))
					return TransferTo(otherStorage, block.Resource);
			}


			return null;
		}

		/// <summary>
		/// Tries to transfer the first allowed resource to the given warehouse.
		/// </summary>
		/// <param name="warehouse">The warehouse to transfer to.</param>
		public void TryTransferFirstAllowed(Warehouse warehouse)
		{
			if (_transferOperation != null) return;

			if (warehouse.WarehouseType == WarehouseType.Input)
			{
				_transferOperation = TryTransferTo(warehouse);
			}
			else if (warehouse.WarehouseType == WarehouseType.Output)
			{
				_transferOperation = warehouse.TryTransferTo(this);
			}
		}

		/// <inheritdoc/>
		public Transform GetDestination()
		{
			return _subcontainers[Quantity];
		}
	}
}
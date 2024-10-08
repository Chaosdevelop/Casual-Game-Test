using System.Collections.Generic;
using System.Linq;
using Game.Animations;
using Game.Core;
using Game.Resources;
using UnityEngine;
using Zenject;

namespace Game.Buildings
{
	/// <summary>
	/// Represents a building that processes resources.
	/// </summary>
	public class Building : MonoBehaviour, IResourceTransferPoint
	{

		[SerializeField]
		private Transform _inputWarehousePoint;
		[SerializeField]
		private Transform _outputWarehousePoint;
		[SerializeField]
		private Transform _inputResourcePoint;
		[SerializeField]
		private Transform _outputResourcePoint;


		private Warehouse _inputWarehouse;
		private Warehouse _outputWarehouse;

		private List<ResourceBlock> _inputBlocks;
		private List<ResourceBlock> _outputBlocks;


		[Inject]
		private PlaceholderFactory<ResourceType, ResourceBlock> _resourceFactory;
		[Inject]
		private PlaceholderFactory<Transform, ResourceType[], WarehouseType, Warehouse> _warehouseFactory;
		[Inject]
		private IGameLog _log;

		private ResourceProcessing _resourceProcessing;
		private List<ITransferOperation> _transferOperations;

		private List<ResourceType> _requestedResources;
		private List<ResourceType> _producedResources;

		private bool IsRequiredResourcesEnough => _requestedResources.Count == 0;
		private bool IsOutputResourcesFull => _outputWarehouse.Quantity == _outputWarehouse.Capacity;


		/// <summary>
		/// Reasons for production to stop.
		/// </summary>
		enum StopReason
		{
			NotEnoughResources,
			FullOutput
		}


		private void Start()
		{

			StartNewProcess();

		}

		/// <summary>
		/// Initializes the building with the given building data.
		/// </summary>
		/// <param name="buildingData">The building data.</param>
		public void Initialize(BuildingData buildingData)
		{
			_resourceProcessing = new ResourceProcessing(buildingData.ResourceRecipe);

			_inputBlocks = new List<ResourceBlock>(_resourceProcessing.GetRequired().Length);
			_outputBlocks = new List<ResourceBlock>(_resourceProcessing.GetResult().Length);
			_transferOperations = new List<ITransferOperation>(2);


			_resourceProcessing.OnCompleted += StartNewProcess;
			_resourceProcessing.OnCompleted += FinishProduction;

			_inputWarehouse = _warehouseFactory.Create(_inputWarehousePoint, buildingData.ResourceRecipe.input,
				WarehouseType.Input);
			_outputWarehouse = _warehouseFactory.Create(_outputWarehousePoint, buildingData.ResourceRecipe.output,
				WarehouseType.Output);

			_inputWarehouse.OnResourcesChanged += OnInputResourcesChanged;
			_outputWarehouse.OnResourcesChanged += OnOutputResourcesChanged;

		}


		private void OnInputResourcesChanged()
		{
			if (!IsOutputResourcesFull && _resourceProcessing.IsStopped)
			{
				RequestRequiredResources();
			}
		}

		private void OnOutputResourcesChanged()
		{
			if (!IsOutputResourcesFull && _resourceProcessing.IsStopped)
			{
				if (IsRequiredResourcesEnough)
				{
					StartProduction();
				}
				else if (_resourceProcessing.GetRequired().Length != _requestedResources.Count)
				{
					RequestRequiredResources();
				}
				else
				{
					StartNewProcess();
				}
			}
		}


		private void StartNewProcess()
		{
			_requestedResources = new List<ResourceType>(_resourceProcessing.GetRequired());
			RequestRequiredResources();
		}

		private void FinishProduction()
		{
			_producedResources = new List<ResourceType>(_resourceProcessing.GetResult());

			foreach (var resourceType in _producedResources)
			{
				var block = _resourceFactory.Create(resourceType);
				block.transform.position = _outputResourcePoint.position;
				_outputBlocks.Add(block);
			}

			SendProduction();
		}


		private void Update()
		{
			_resourceProcessing.UpdateProcessing(Time.deltaTime);

			for (int i = _transferOperations.Count - 1; i >= 0; i--)
			{
				_transferOperations[i].Update(Time.deltaTime);
				if (_transferOperations[i].IsCompleted)
					_transferOperations.RemoveAt(i);
			}
		}


		/// <inheritdoc/>
		public ITransferOperation TransferTo(IResourceTransferPoint otherPoint, ResourceType resourceValue)
		{
			var block = _outputBlocks.LastOrDefault(arg => arg.Resource == resourceValue);
			RemoveBlock(block);

			var transfer = new LerpTransferAnimation();
			transfer.SetTransferData(() => _outputResourcePoint, otherPoint.GetDestination, 1.0f, block,
				() => otherPoint.AddBlock(block));
			return transfer;
		}


		/// <inheritdoc/>
		public void AddBlock(ResourceBlock resourceBlock)
		{
			_inputBlocks.Add(resourceBlock);

			_requestedResources.Remove(resourceBlock.Resource);

			if (IsRequiredResourcesEnough)
			{
				StartProduction();
			}
			else
			{
				_resourceProcessing.StopProcessing();
				_log.Log($"Production stopped: {name} reason: {StopReason.NotEnoughResources}");
			}
		}

		/// <inheritdoc/>
		public void RemoveBlock(ResourceBlock resourceBlock)
		{
			_outputBlocks.Remove(resourceBlock);
		}


		private void RequestRequiredResources()
		{
			if (_requestedResources.Count > 0)
			{
				var resource = _requestedResources.FirstOrDefault(_inputWarehouse.Contains);

				if (resource != 0)
				{
					var operation = _inputWarehouse.TransferTo(this, resource);
					_transferOperations.Add(operation);
				}
				else
				{
					_log.Log($"Production stopped: {name} reason: {StopReason.NotEnoughResources}");
				}
			}
			else
			{
				StartProduction();
			}
		}

		private void StartProduction()
		{
			_inputBlocks.Clear();
			_resourceProcessing.StartProcessing();
		}


		private void SendProduction()
		{
			if (_producedResources.Count > 0)
			{
				var firstResource = _producedResources.First();

				if (_outputWarehouse.CanReceive(firstResource))
				{
					_transferOperations.Add(TransferTo(_outputWarehouse, firstResource));
					if (_resourceProcessing.IsStopped && IsRequiredResourcesEnough)
					{
						_resourceProcessing.ResumeProcessing();
					}
				}
				else
				{
					_resourceProcessing.StopProcessing();
					_log.Log($"Production stopped: {name} reason: {StopReason.FullOutput}");
				}


			}

		}


		/// <inheritdoc/>
		public Transform GetDestination()
		{
			return _inputResourcePoint;
		}


	}
}
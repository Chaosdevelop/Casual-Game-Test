using System;
using Game.Animations;
using Game.Resources;

namespace Game
{
	/// <summary>
	/// Interface for resource storage locations.
	/// </summary>
	public interface IResourceStorage : IResourceTransferPoint
	{

		/// <summary>
		/// Gets the maximum capacity of the storage.
		/// </summary>
		int Capacity { get; }


		/// <summary>
		/// Gets the current quantity of resources in the storage.
		/// </summary>
		int Quantity { get; }


		/// <summary>
		/// Returns true if the storage can receive the given resource.
		/// </summary>
		/// <param name="resourceValue">The type of resource to check.</param>
		/// <returns>True if the storage can receive the resource, false otherwise.</returns>
		bool CanReceive(ResourceType resourceValue);

		/// <summary>
		/// Returns true if the storage contains any amount of the given resource.
		/// </summary>
		/// <param name="resourceValue">The type of resource to check.</param>
		/// <returns>True if the storage contains the resource, false otherwise.</returns>
		bool Contains(ResourceType resourceValue);



		/// <summary>
		/// Event triggered when the stored resources change.
		/// </summary>
		event Action OnResourcesChanged;


		/// <summary>
		/// Attempts to transfer resources to another storage.
		/// </summary>
		/// <param name="otherStorage">The destination storage.</param>
		/// <returns>A transfer operation if a transfer was initiated, otherwise null.</returns>
		ITransferOperation TryTransferTo(IResourceStorage otherStorage);
	}
}

using Game.Animations;
using Game.Resources;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Interface for resource transfer points.
	/// </summary>
	public interface IResourceTransferPoint
	{
		/// <summary>
		/// Transfers a specific resource to another transfer point.
		/// </summary>
		/// <param name="otherPoint">The destination transfer point.</param>
		/// <param name="resourceValue">The type of resource to transfer.</param>
		/// <returns>The transfer operation, or null if the transfer cannot be initiated.</returns>
		ITransferOperation TransferTo(IResourceTransferPoint otherPoint, ResourceType resourceValue);

		/// <summary>
		/// Adds a resource block to the transfer point.
		/// </summary>
		/// <param name="resourceBlock">The resource block to add.</param>
		void AddBlock(ResourceBlock resourceBlock);

		/// <summary>
		/// Removes a resource block from the transfer point.
		/// </summary>
		/// <param name="resourceBlock">The resource block to remove.</param>
		void RemoveBlock(ResourceBlock resourceBlock);

		/// <summary>
		/// Returns the destination transform for resource transfers.
		/// </summary>
		/// <returns>The destination transform.</returns>
		Transform GetDestination();
	}
}

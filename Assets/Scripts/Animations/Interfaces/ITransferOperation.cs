using System;
using UnityEngine;

namespace Game.Animations
{
	/// <summary>
	/// Interface for a resource transfer operation.
	/// </summary>
	public interface ITransferOperation
	{
		/// <summary>
		/// Sets up the transfer operation.
		/// </summary>
		/// <param name="source">A function returning the transform of the source of the transfer.</param>
		/// <param name="destination">A function returning the transform of the destination of the transfer.</param>
		/// <param name="duration">The duration of the transfer in seconds.</param>
		/// <param name="transferable">The object being transferred.</param>
		/// <param name="onTransferEnd">An action to invoke when the transfer is complete.</param>
		void SetTransferData(Func<Transform> source, Func<Transform> destination, float duration,
			ITransferable transferable, System.Action onTransferEnd);


		/// <summary>
		/// Updates the transfer operation.
		/// </summary>
		/// <param name="deltaTime">The time since the last frame.</param>
		void Update(float deltaTime);


		/// <summary>
		/// Indicates whether the transfer operation is complete.
		/// </summary>
		bool IsCompleted { get; }
	}
}

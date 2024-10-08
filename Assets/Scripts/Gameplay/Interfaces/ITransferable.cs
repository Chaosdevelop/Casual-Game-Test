using UnityEngine;

namespace Game
{
	/// <summary>
	/// Interface for any objects that can be transferred in the game.
	/// </summary>
	public interface ITransferable
	{
		/// <summary>
		/// Returns the transform of the transferable object.
		/// </summary>
		/// <returns>The transform of the transferable object.</returns>
		Transform GetTransform();
	}
}

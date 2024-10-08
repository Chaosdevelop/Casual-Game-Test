namespace Game.Core
{
	/// <summary>
	/// Interface for logging game events.
	/// </summary>
	public interface IGameLog
	{
		/// <summary>
		/// Logs a message to the game log.
		/// </summary>
		/// <param name="message">The message to log.</param>
		void Log(string message);

		/// <summary>
		/// Clears the game log.
		/// </summary>
		void Clear();
	}
}
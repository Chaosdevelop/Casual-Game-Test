using System;
using Game.Resources;

namespace Game.Buildings
{
	/// <summary>
	/// Handles the resource processing logic.
	/// </summary>
	[Serializable]
	public class ResourceProcessing
	{
		private readonly ResourceRecipe _resourceRecipe;

		private float _processingTimer;
		public bool IsCompleted { get; private set; }
		public bool IsStopped { get; private set; } = true;


		public event Action OnCompleted;

		public ResourceProcessing(ResourceRecipe resourceRecipe)
		{
			_resourceRecipe = resourceRecipe;
		}

		/// <summary>
		/// Starts the resource processing.
		/// </summary>
		public void StartProcessing()
		{
			_processingTimer = 0;
			IsCompleted = false;
			IsStopped = false;
		}

		/// <summary>
		/// Stops the resource processing.
		/// </summary>
		public void StopProcessing()
		{
			IsStopped = true;
		}

		/// <summary>
		/// Resumes the resource processing.
		/// </summary>
		public void ResumeProcessing()
		{
			IsStopped = false;
		}

		/// <summary>
		/// Updates the resource processing.
		/// </summary>
		/// <param name="deltaTime">The time since the last frame.</param>
		public void UpdateProcessing(float deltaTime)
		{
			if (IsCompleted || IsStopped) return;


			_processingTimer += deltaTime;
			IsCompleted = _processingTimer >= _resourceRecipe.time;

			if (IsCompleted)
			{
				IsStopped = true;
				OnCompleted?.Invoke();
			}
		}

		/// <summary>
		/// Gets the required input resource types.
		/// </summary>
		/// <returns>An array of required resource types.</returns>
		public ResourceType[] GetRequired()
		{
			return _resourceRecipe.input;
		}

		/// <summary>
		/// Gets the resulting output resource types.
		/// </summary>
		/// <returns>An array of resulting resource types.</returns>
		public ResourceType[] GetResult()
		{
			return _resourceRecipe.output;
		}
	}
}
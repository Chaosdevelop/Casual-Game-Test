using UnityEngine;
using Zenject;


namespace Game.Resources
{

	// <summary>
	/// Represents a physical block of a resource.
	/// </summary>
	public class ResourceBlock : MonoBehaviour, ITransferable
	{

		[SerializeField]
		Renderer _renderer;

		public ResourceType Resource { get; private set; }


		[Inject]
		void Initialize(ResourceType resourceType)
		{
			SetResource(resourceType);
		}

		/// <summary>
		/// Sets the resource type for this block.
		/// </summary>
		/// <param name="resourceType">The resource type to set.</param>
		public void SetResource(ResourceType resourceType)
		{
			Resource = resourceType;
			switch (resourceType)
			{
				case ResourceType.Ore:
					_renderer.material.color = Color.black;
					break;
				case ResourceType.Ingot:
					_renderer.material.color = Color.gray;
					break;
				case ResourceType.Alloy:
					_renderer.material.color = Color.yellow;
					break;
			}
			name = $"ResourceBlock {resourceType}";
		}

		/// <inheritdoc/>
		public Transform GetTransform()
		{
			return transform;
		}
	}

}
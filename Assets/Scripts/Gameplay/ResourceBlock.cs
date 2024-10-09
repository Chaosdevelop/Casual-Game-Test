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
		[SerializeField]
		Color _oreColor;
		[SerializeField]
		Color _ingotColor;
		[SerializeField]
		Color _alloyColor;

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
					_renderer.material.color = _oreColor;
					break;
				case ResourceType.Ingot:
					_renderer.material.color = _ingotColor;
					break;
				case ResourceType.Alloy:
					_renderer.material.color = _alloyColor;
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
using Game.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
	/// <summary>
	/// Controls the player character.
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private InputActionReference _moveAction;
		[SerializeField]
		private Transform _viewTransform;
		[SerializeField]
		private Rigidbody _rigidbody;
		[SerializeField]
		private float _moveSpeed = 10.0f;
		[SerializeField]
		private float _rotationSpeed = 5.0f;
		[SerializeField]
		private Inventory _inventory;
		[SerializeField]
		private LayerMask _warehouseLayer;

		private void Start()
		{
			_inventory.Initialize();
		}

		private void FixedUpdate()
		{
			Vector2 moveDirection = _moveAction.action.ReadValue<Vector2>();
			Vector3 step = moveDirection * _moveSpeed * Time.fixedDeltaTime;
			Vector3 targetPosition = _rigidbody.position + new Vector3(step.x, 0, step.y);
			_rigidbody.MovePosition(targetPosition);


			if (moveDirection != Vector2.zero)
			{
				float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
				_viewTransform.rotation = Quaternion.RotateTowards(_viewTransform.rotation, targetRotation, _rotationSpeed);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			TryInteractWithWarehouse(other);
		}

		private void OnTriggerStay(Collider other)
		{
			TryInteractWithWarehouse(other);
		}

		private void TryInteractWithWarehouse(Collider other)
		{
			if (((1 << other.gameObject.layer) & _warehouseLayer) != 0)
			{
				Warehouse warehouse = other.GetComponent<Warehouse>();
				if (warehouse != null)
				{
					_inventory.TryTransferFirstAllowed(warehouse);
				}
			}
		}
	}
}
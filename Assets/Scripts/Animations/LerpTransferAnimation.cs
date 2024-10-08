using System;
using UnityEngine;

namespace Game.Animations
{
	/// <summary>
	/// Implementation of ITransferOperation using a lerp animation.
	/// </summary>
	public class LerpTransferAnimation : ITransferOperation
	{

		public bool IsCompleted { get; private set; }

		private Func<Transform> _source;
		private Func<Transform> _destination;
		private ITransferable _transferable;
		private Action _onTransferEnd;
		private float _duration;
		private float _time;


		/// <inheritdoc/>
		public void SetTransferData(Func<Transform> source, Func<Transform> destination, float duration,
			ITransferable transferable, Action onTransferEnd)
		{
			_source = source;
			_destination = destination;
			_transferable = transferable;
			_onTransferEnd = onTransferEnd;
			_duration = duration;
		}


		/// <inheritdoc/>
		public void Update(float deltaTime)
		{
			if (IsCompleted) return;

			_time += deltaTime;
			float interpolation = _time / _duration;


			var sourceTransform = _source.Invoke();
			var destinationTransform = _destination.Invoke();
			var tranferableTransform = _transferable.GetTransform();

			var interpolatedPos = Vector3.Lerp(sourceTransform.position, destinationTransform.position, interpolation);
			var interpolatedRot = Quaternion.Slerp(sourceTransform.rotation, destinationTransform.rotation, interpolation);

			tranferableTransform.position = interpolatedPos;
			tranferableTransform.rotation = interpolatedRot;


			if (_time >= _duration)
			{
				IsCompleted = true;
				tranferableTransform.SetParent(destinationTransform);
				_onTransferEnd?.Invoke();
			}
		}


	}
}
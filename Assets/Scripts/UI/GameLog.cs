using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Core
{

	/// <summary>
	/// Implementation of IGameLog using a Unity UI Text component.
	/// </summary>
	public class GameLog : MonoBehaviour, IGameLog
	{
		[SerializeField]
		private Text _logText;
		[SerializeField]
		private int _maxLogs = 10;

		private Queue<string> _logs;

		[Inject]
		private void Initialize()
		{
			_logs = new Queue<string>();
			Clear();
		}

		/// <inheritdoc/>
		public void Log(string message)
		{
			_logs.Enqueue(message);

			while (_logs.Count > _maxLogs)
			{
				_logs.Dequeue();
			}

			UpdateText();
		}

		/// <inheritdoc/>
		public void Clear()
		{
			_logs.Clear();
			UpdateText();
		}

		private void UpdateText()
		{
			_logText.text = string.Join("\n", _logs);
		}
	}
}
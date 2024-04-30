using System;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace _App.Scripts.Lobby
{
	internal class RegisterPopup : MonoBehaviour
	{
		[SerializeField] private UIPopup _instance;
		[SerializeField] private TMP_InputField _nameInput;
		[SerializeField] private UIButton _createUserButton;

		public event Action<RegisterPopup, string> OnUserNameRegistered;

		private void Start()
		{
			_createUserButton.Interactable = false;
			_nameInput.onValueChanged.AddListener(OnChangeValue);
			_createUserButton.OnClick.OnTrigger.Action = CreateUserButtonPressed;
		}

		private void OnChangeValue(string arg0)
		{
			if (arg0.Length < 4 || arg0.Length > 9)
			{
				_createUserButton.Interactable = false;
			}
			else
			{
				_createUserButton.Interactable = true;
			}
		}

		private void CreateUserButtonPressed(GameObject obj)
		{
			_instance.Hide();
			OnUserNameRegistered?.Invoke(this, _nameInput.text);
		}
	}
}
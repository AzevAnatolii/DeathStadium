using System;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using _App.Scripts.Network;
using _App.Scripts.Popups;

namespace _App.Scripts.Lobby
{
	internal class LobbyController : MonoBehaviour
	{
		public static LobbyController Instance { get; private set; }

		[SerializeField] private TMP_Text _userNameText;
		[SerializeField] private UIButton _logInButton;
		[SerializeField] private UIButton _playButton;
		[SerializeField] private UIButton _exitButton;

		private LogInPopup _logInPopup;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			_logInButton.OnClick.OnTrigger.Action = OnLogInButtonClicked;
			_playButton.OnClick.OnTrigger.Action = OnPlayButtonClicked;
			_exitButton.OnClick.OnTrigger.Action = OnExitButtonClicked;
			DebugExt.Log(this, "Awake");
		}

		public void OnAuthorizationResponse(int errorCode, string message)
		{
			Debug.Log($"{(ErrorCode)errorCode} - {message}");
			if (errorCode == (int) ErrorCode.Ok)
			{
				if (_logInPopup)
				{
					_logInPopup.Hide();
				}
				Client.LocalClient.SetLoggedIn(message);
				UpdateView();
			}
			else
			{
				if (_logInPopup)
				{
					_logInPopup.ShowErrorMessage(message);
				}
			}
		}

		private void UpdateView()
		{
			bool isClientLoggedIn = IsClientLoggedIn(Client.LocalClient);
			if (isClientLoggedIn)
			{
				_userNameText.text = Client.LocalClient.Name;
			}
			_userNameText.transform.parent.gameObject.SetActive(isClientLoggedIn);
			_playButton.gameObject.SetActive(isClientLoggedIn);
			_logInButton.gameObject.SetActive(!isClientLoggedIn);
		}

		private bool IsClientLoggedIn(Client client)
		{
			return client != null && client.IsLoggedIn && NameChecker.IsNameValid(client.Name);
		}

		private void OnLogInButtonClicked(GameObject obj)
		{
			if (_logInPopup)
			{
				_logInPopup.Hide();
			}

			PopupController.TryShowPopup("LogInPopup", out _logInPopup);
		}

		private void OnPlayButtonClicked(GameObject obj)
		{
			if (PopupController.TryShowPopup("MatchListPopup", out MatchListPopup popup))
			{
				popup.Init(new Action<string>(OnCreateMatchButtonClicked), new Action<string>(OnJoinMatchButtonClicked));
			}
		}

		private void OnCreateMatchButtonClicked(string levelName)
		{
				
		}

		private void OnJoinMatchButtonClicked(string matchName)
		{
			
		}

		private void OnExitButtonClicked(GameObject obj)
		{
			Application.Quit();
		}
	}
}
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
		}

		public void OnAuthorizationResponse(int errorCode, string message)
		{
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
			string popupName = "MatchListPopup";
			Action<string> onCreate = OnCreateMatchButtonClicked;
			Action<string> onJoin = OnJoinMatchButtonClicked;
			PopupController.TryShowPopup(popupName, out MatchListPopup popup, onCreate, onJoin);
		}

		private void OnCreateMatchButtonClicked(string levelName)
		{
			Client.LocalClient.CreateMatch(levelName);
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
using Doozy.Engine.UI;
using Mirror;
using TMPro;
using UnityEngine;

namespace _App.Scripts.Lobby
{
    internal class LobbyController : NetworkBehaviour
    {
        private const string USER_NAME_KEY = "user_name";
        
        [SerializeField] private UIButton _startButton;
        [SerializeField] private UIButton _registerButton;
        [SerializeField] private UIButton _exitButton;
        [SerializeField] private TMP_Text _userName;

        private void Start()
        {
            string userName = PlayerPrefs.GetString(USER_NAME_KEY);
            bool isUserRegistered = IsUserNameValid(userName);
            _startButton.gameObject.SetActive(isUserRegistered);
            _registerButton.gameObject.SetActive(!isUserRegistered);
            if (isUserRegistered)
            {
                _userName.text = userName;
            }

            _startButton.OnClick.OnTrigger.Action = OnStartButtonClick;
            _registerButton.OnClick.OnTrigger.Action = OnRegisterButtonClick;
            _exitButton.OnClick.OnTrigger.Action = OnExitButtonClick;
        }

        private bool IsUserNameValid(string userName)
        {
            return string.IsNullOrWhiteSpace(userName) == false && userName.Length is >= 4 and < 10;
        }

        private void OnRegisterButtonClick(GameObject obj)
        {
            UIPopup popup = UIPopupManager.ShowPopup("RegisterPopup", false, false);
            if (popup.TryGetComponent(out RegisterPopup registerPopup))
            {
                registerPopup.OnUserNameRegistered += OnUserNameRegistered;
            }
        }

        private void OnUserNameRegistered(RegisterPopup popup, string userName)
        {
            popup.OnUserNameRegistered -= OnUserNameRegistered;
            if (IsUserNameValid(userName))
            {
                PlayerPrefs.SetString(USER_NAME_KEY, userName);
                PlayerPrefs.Save();
                _userName.text = userName;
                _registerButton.gameObject.SetActive(false);
                _startButton.gameObject.SetActive(true);
            }
        }

        private void OnStartButtonClick(GameObject obj)
        {
        }

        private void OnExitButtonClick(GameObject obj)
        {
            Application.Quit();
        }
    }
}

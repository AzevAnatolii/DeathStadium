using _App.Scripts.Network;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace _App.Scripts.Popups
{
    internal class LogInPopup : PopupBase
    {
        [SerializeField] private UIPopup _popup;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private UIButton _loggInButton;

    
        private void Start()
        {
            _loggInButton.Interactable = false;
            _loggInButton.OnClick.OnTrigger.Action = OnLogInButtonClicked;
            _nameInput.onValueChanged.AddListener(OnChangeValue);
        }

        private void OnDestroy()
        {
            _popup.HideBehavior.OnStart.Action = null;
        }

        public void Hide()
        {
            _popup.Hide();
        }

        public void ShowErrorMessage(string message)
        {
            //TODO: show message
        }

        private void OnChangeValue(string userName)
        {
            _loggInButton.Interactable = NameChecker.IsNameValid(userName);
        }

        private void OnLogInButtonClicked(GameObject obj)
        {
            DebugExt.Log(this, $"LogIn {_nameInput.text}");
            Client.LocalClient.LogIn(_nameInput.text);
        }
    }
}

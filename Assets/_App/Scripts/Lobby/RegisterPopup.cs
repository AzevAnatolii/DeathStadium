using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class RegisterPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private UIButton _createUserButton;
    [SerializeField] private UIPopup _popup;

    private void Start()
    {
        _createUserButton.Interactable = false;
        _createUserButton.OnClick.OnTrigger.Action = OnCreateUserButtonClicked;
        _nameInput.onValueChanged.AddListener(OnChangeValue);
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

    private void OnCreateUserButtonClicked(GameObject obj)
    {
        Player.LocalPlayer.SetPlayerName(_nameInput.text);
        _popup.Hide();
    }
}

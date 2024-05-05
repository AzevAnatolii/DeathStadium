using Doozy.Engine.UI;
using Mirror;
using TMPro;
using UnityEngine;

internal class LobbyController : NetworkBehaviour
{
    private const string NAME_KEY = "player_name";
    
    public static LobbyController Instance { get; private set; }

    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private UIButton _registerButton;
    [SerializeField] private UIButton _playButton;
    [SerializeField] private UIButton _exitButton;

    private string _userName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
                
        Instance = this;
        Init();
    }

    private void Init()
    {                                                                  
        string userName = PlayerPrefs.GetString(NAME_KEY);
        bool isNameValid = IsNameValid(userName);
        _userNameText.text = userName;
        _userNameText.transform.parent.gameObject.SetActive(isNameValid);

        _playButton.gameObject.SetActive(isNameValid);
        _registerButton.gameObject.SetActive(!isNameValid);

        _registerButton.OnClick.OnTrigger.Action = OnRegisterButtonClicked;
        _playButton.OnClick.OnTrigger.Action = OnPlayButtonClicked;
        _exitButton.OnClick.OnTrigger.Action = OnExitButtonClicked;        
    }

    private bool IsNameValid(string s)
    {
        return s.Length > 3 && s.Length < 10;
    }

    private void OnRegisterButtonClicked(GameObject obj)
    {
        var registerPopup = UIPopupManager.ShowPopup("RegisterPopup", false, false);
        if (registerPopup.TryGetComponent(out RegisterPopup popup))
        {
            popup.Init(OnCreateUserButtonClicked);            
        }
        else
        {
            Destroy(registerPopup.gameObject);
        }        
    }

    private void OnCreateUserButtonClicked(string userName)
    {
        _userName = userName;
        PlayerPrefs.SetString(NAME_KEY, userName);
        Player.LocalPlayer.SetName(userName);
        bool isNameValid = IsNameValid(userName);
        _userNameText.text = userName;
        _userNameText.transform.parent.gameObject.SetActive(isNameValid);
    }

    private void OnPlayButtonClicked(GameObject obj)
    {
        var matchListPopup = UIPopupManager.ShowPopup("MatchListPopup", false, false);
        if (matchListPopup.TryGetComponent(out MatchListPopup popup))
        {
            popup.Init(OnCreateMatchButtonClicked, OnJoinMatchButtonClicked);            
        }
        else
        {
            Destroy(matchListPopup.gameObject);
        }
    }

    private void OnCreateMatchButtonClicked(string levelName)
    {
        Player.LocalPlayer.CreateMatch(levelName);
    }

    private void OnJoinMatchButtonClicked(string matchName)
    {
        Player.LocalPlayer.JoinMatch(matchName);
    }

    private void OnExitButtonClicked(GameObject obj)
    {
        Application.Quit();
    }
}

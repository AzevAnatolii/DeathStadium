using Doozy.Engine.UI;
using Mirror;
using UnityEngine;

internal class LobbyController : NetworkBehaviour
{
    public static LobbyController Instance { get; private set; }

    [SerializeField] private UIButton _playButton;
    [SerializeField] private UIButton _exitButton;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _playButton.OnClick.OnTrigger.Action = OnPlayButtonClicked;
        _exitButton.OnClick.OnTrigger.Action = OnExitButtonClicked;
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

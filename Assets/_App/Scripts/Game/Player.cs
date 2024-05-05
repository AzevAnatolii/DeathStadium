using System;
using Mirror;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer { get; private set; }
    public static event Action<Player> OnLocalPlayerStart; 
    
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _nameText;
    
    [SyncVar][SerializeField] private string _name;
    public string Name => _name;

    private void Start()
    {
        _view.SetActive(false);
        
        if (!isOwned)
        {
            //_nameText.text = _name;
        }
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        
        DebugExt.Log(this, $"OnStartAuthority {isLocalPlayer}");
        if (isLocalPlayer)
        {
            LocalPlayer = this;
            OnLocalPlayerStart?.Invoke(this);    
        }
    }

    public void SetName(string playerName)
    {
        if (!isOwned) return;
        //_nameText.text = playerName;
        SetPlayerNameOnServer(playerName);
        DebugExt.Log(this, $"SetName {playerName}");
    }

    public void SetViewEnable()
    {
        _view.SetActive(true);
    }
    
    public void CreateMatch(string levelName)
    {
        if (!isOwned) return;
        CommandCreateMatch(levelName);
        _view.SetActive(true);
        DebugExt.Log(this, $"CreateMatch name {_name}");
    }
    
    public void JoinMatch(string matchName)
    {
        if (!isOwned) return;
        CommandConnectToMatch(matchName);
        _view.SetActive(true);
        DebugExt.Log(this, $"JoinMatch name {_name}   match {matchName}");
    }
    
    [Command]
    private void CommandCreateMatch(string levelName)
    {
        StartCoroutine(NetworkSceneManager.Instance.ServerCreateSubScene(this, levelName));
    }

    [Command]
    private void CommandConnectToMatch(string matchName)
    {
        NetworkSceneManager.Instance.ServerConnectPlayerToMatch(this, matchName);
    }

    [Command]
    private void SetPlayerNameOnServer(string newName)
    {
        _name = newName;
    }
}
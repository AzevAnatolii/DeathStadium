using Mirror;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _nameText;
    
    [SyncVar] private string _name;
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
        
        if (isLocalPlayer)
        {
            LocalPlayer = this;
        }
    }

    public void SetName(string playerName)
    {
        if (!isOwned) return;
        //_nameText.text = playerName;
        SetPlayerNameOnServer(playerName);
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
    }
    
    public void JoinMatch(string matchName)
    {
        if (!isOwned) return;
        CommandConnectToMatch(matchName);
        _view.SetActive(true);
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
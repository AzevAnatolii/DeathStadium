using System;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchListPopup : MonoBehaviour
{    
    [SerializeField] private UIPopup _popup;
    [SerializeField] private UIButton _createMatchButton;
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private Transform _listParent;
    [SerializeField] private MatchItem _matchItemPrefab;
    
    private const string GAME_SCENE_NAME = "GameScene";
    
    private List<MatchItem> _matchItems;
    private Action<string> _createMatchCallback;
    private Action<string> _joinMatchCallback;


    private void OnDestroy()
    {
        _matchItems.Clear();
        NetworkSceneManager.Instance.AvailableScenes.Callback -= MatchesOnCallback;
    }

    public void Init(Action<string> createMatchCallback, Action<string> joinMatchCallback)
    {
        _createMatchCallback = createMatchCallback;
        _joinMatchCallback = joinMatchCallback;
        _closeButton.OnClick.OnTrigger.Action = OnCloseButtonClicked;
        _createMatchButton.OnClick.OnTrigger.Action = OnCreateMatchButtonClicked;

        _matchItems = new List<MatchItem>();        
        foreach (var match in NetworkSceneManager.Instance.AvailableScenes)
        {
            CreateMatchItem(match.Key);
        }
        NetworkSceneManager.Instance.AvailableScenes.Callback -= MatchesOnCallback;
        NetworkSceneManager.Instance.AvailableScenes.Callback += MatchesOnCallback;
    }

    private void CreateMatchItem(string matchName)
    {
        var item = Instantiate(_matchItemPrefab, _listParent, false);
        item.Init(matchName, OnJoinMatchButtonClicked);
        _matchItems.Add(item);
    }
    
    private void MatchesOnCallback(SyncIDictionary<string, Scene>.Operation op, string key, Scene item)
    {
        switch (op)
        {
            case SyncIDictionary<string, Scene>.Operation.OP_ADD:
                CreateMatchItem(key);
                break;
            case SyncIDictionary<string, Scene>.Operation.OP_CLEAR:
                break;
            case SyncIDictionary<string, Scene>.Operation.OP_REMOVE:
                foreach (var match in _matchItems)
                {
                    if (match.Name == key)
                    {
                        Destroy(match.gameObject);
                    }
                }
                break;
            case SyncIDictionary<string, Scene>.Operation.OP_SET:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(op), op, null);
        }
    }

    private void OnCreateMatchButtonClicked(GameObject obj)
    {
        _popup.Hide(true);
        _createMatchCallback?.Invoke(GAME_SCENE_NAME);
    }
    
    private void OnJoinMatchButtonClicked(string matchName)
    {
        _popup.Hide(true);
        _joinMatchCallback?.Invoke(matchName);
    }
        
    private void OnCloseButtonClicked(GameObject obj)
    {
        _popup.Hide(true);
    }
}

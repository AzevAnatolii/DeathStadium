using System;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using _App.Scripts.Lobby.Matches;
using _App.Scripts.Network;

namespace _App.Scripts.Popups
{
    internal class MatchListPopup : PopupBase
    {    
        [SerializeField] private UIButton _createMatchButton;
        [SerializeField] private UIButton _closeButton;
        [SerializeField] private Transform _listParent;
        [SerializeField] private MatchItem _matchItemPrefab;
    
        private const string GAME_SCENE_NAME = "GameScene";
    
        private List<MatchItem> _matchItems;
        private Action<string> _createMatchCallback;
        private Action<string> _joinMatchCallback;


        protected override void InitInternal(object[] args)
        {
            if (args.Length > 1)
            {
                if (args[0] is Action<string> createMatchCallback)
                {
                    _createMatchCallback = createMatchCallback;
                }
                if (args[1] is Action<string> joinMatchCallback)
                {
                    _joinMatchCallback = joinMatchCallback;
                }
            }
            
            _closeButton.OnClick.OnTrigger.Action = OnCloseButtonClicked;
            _createMatchButton.OnClick.OnTrigger.Action = OnCreateMatchButtonClicked;
            MatchMaker.Instance.OnListUpdated += OnListUpdated;
            
            _matchItems = new List<MatchItem>();
            Client.LocalClient.UpdateMatches();
        }

        protected override void OnDestroyInternal()
        {
            _matchItems.Clear();
        }

        private void OnListUpdated(List<Match> list)
        {
            foreach (Match match in list)
            {
                if (_matchItems.Exists(x => x.HostName == match.hostName))
                {
                    continue;
                }
                
                CreateMatchItem(match);
            }
        }

        private void CreateMatchItem(Match match)
        {
            MatchItem item = Instantiate(_matchItemPrefab, _listParent, false);
            item.Init(match.hostName, OnJoinMatchButtonClicked);
            _matchItems.Add(item);
        }

        private void RemoveMatchItem(Match match)
        {
            MatchItem item = _matchItems.Find(x => x.HostName == match.hostName);
            if (item)
            {
                _matchItems.Remove(item);
                Destroy(item.gameObject);
            }
        }

        private void OnCreateMatchButtonClicked(GameObject obj)
        {
            Hide();
            _createMatchCallback?.Invoke(GAME_SCENE_NAME);
        }
    
        private void OnJoinMatchButtonClicked(string matchName)
        {
            Hide();
            _joinMatchCallback?.Invoke(matchName);
        }
        
        private void OnCloseButtonClicked(GameObject obj)
        {
            Hide();
        }
    }
}

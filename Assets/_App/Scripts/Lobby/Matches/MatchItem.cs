using System;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace _App.Scripts.Lobby.Matches
{
    internal class MatchItem : MonoBehaviour
    {
        [SerializeField] private UIButton _button;
        [SerializeField] private TextMeshProUGUI _viewMatchName;

        public string HostName { get; private set; }

        public void Init(string hostName, Action<string> clickCallback)
        {
            HostName = hostName;
            _viewMatchName.text = $"{hostName}'s room";
            _button.OnClick.OnTrigger.Action = _ => clickCallback?.Invoke(HostName);
        }

        private void OnDestroy()
        {
            _button.OnClick.OnTrigger.Action = null;
        }
    }
}

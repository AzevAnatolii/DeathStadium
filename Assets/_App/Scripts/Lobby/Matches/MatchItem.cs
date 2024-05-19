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

        public string Name { get; private set; }

        public void Init(string matchName, Action<string> clickCallback)
        {
            Name = matchName;
            _viewMatchName.text = $"{matchName}'s room";
            _button.OnClick.OnTrigger.Action = _ => clickCallback?.Invoke(Name);
        }

        private void OnDestroy()
        {
            _button.OnClick.OnTrigger.Action = null;
        }
    }
}

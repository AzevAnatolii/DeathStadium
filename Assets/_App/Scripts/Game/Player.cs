using Mirror;
using TMPro;
using UnityEngine;
using _App.Scripts.Core;

namespace _App.Scripts.Game
{
    internal class Player : NetworkBehaviour
    {
        public static Player LocalPlayer { get; private set; }
    
        [SerializeField] private GameObject _view;
    
        [SyncVar][SerializeField] private string _name;
        public string Name => _name;

        private void Start()
        {
            _view.SetActive(false);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
        
            DebugExt.Log(this, $"OnStartAuthority {isLocalPlayer}");
            if (isLocalPlayer)
            {
                LocalPlayer = this;
            }
        }

        public void Show()
        {
            _view.SetActive(true);
        }
    }
}
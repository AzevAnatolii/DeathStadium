using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _App.Scripts.Network
{
    internal class Network : NetworkManager
    {
        private const string IP = "192.168.1.34";
    
        [SerializeField] private bool _connectToAnotherLaptop;
        [SerializeField] private bool _isServer;
        private List<Client> _clients = new();

        public override void Start()
        {
            if (_connectToAnotherLaptop)
            {
                networkAddress = IP;
            }
        
#if UNITY_EDITOR
            if (_isServer)
            {
                StartServer();
            }
            else
            {
                StartClient();
            }
#else
            base.Start();
#endif
        }

        public override void OnServerConnect(NetworkConnectionToClient connection)
        {
            DebugExt.Log(this, "OnServerConnect");
            base.OnServerConnect(connection);
        }

        public override void OnServerReady(NetworkConnectionToClient connection)
        {
            DebugExt.Log(this, "OnServerReady");
            base.OnServerReady(connection);
            InstantiateClient(connection);
        }

        public override void OnClientConnect()
        {
            DebugExt.Log(this, "OnClientConnect");
            base.OnClientConnect();
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnClientError(TransportError error, string reason)
        {
            DebugExt.Log(this, $"OnClientError : {error} > {reason}");
            base.OnClientError(error, reason);
        }

        private void InstantiateClient(NetworkConnectionToClient connection)
        {
            GameObject clientObj = Instantiate(playerPrefab, Server.Instance.transform);
            NetworkServer.AddPlayerForConnection(connection, clientObj);
            if (clientObj.TryGetComponent(out Client client))
            {
                _clients.Add(client);
            }
        }
    }
}

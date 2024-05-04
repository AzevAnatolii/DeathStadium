using Mirror;
using UnityEngine;

public class Network : NetworkManager
{
    [SerializeField] private bool _isServer;
    
    public override void Start()
    {
        base.Start();
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
        if (Application.isBatchMode)
        {
            StartServer();
        }
        else
        {
            StartClient();
        }
#endif
    }

    public override void OnServerConnect(NetworkConnectionToClient connection)
    {
        base.OnServerConnect(connection);
        var player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(connection, player);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client Connected");
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);
        Debug.Log($"Client Error : {error} > {reason}");
    }
}

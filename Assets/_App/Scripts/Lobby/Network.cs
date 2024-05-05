using Mirror;
using UnityEngine;

public class Network : NetworkManager
{    
    [SerializeField] private bool _isServer;
    
    public override void Start()
    {
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
        base.OnServerConnect(connection);
        GameObject playerObj = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(connection, playerObj);
        DebugExt.Log(this, "Server Connected");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        DebugExt.Log(this, "Client Connected");
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);
        DebugExt.Log(this, $"Client Error : {error} > {reason}");
    }
}

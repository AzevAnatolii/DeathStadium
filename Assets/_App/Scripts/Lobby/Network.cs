using Mirror;
using UnityEngine;

public class Network : NetworkManager
{
    [SerializeField] private bool isServer;
    [Scene] public string gameScene;
    
    public override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        if (isServer)
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

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        var player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn ,player);
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

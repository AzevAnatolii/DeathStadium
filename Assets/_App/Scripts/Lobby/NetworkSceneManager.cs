using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : NetworkBehaviour
{
    public static NetworkSceneManager Instance;

    private readonly LoadSceneParameters _sceneLoadParams = new (LoadSceneMode.Single, LocalPhysicsMode.Physics3D);
    private readonly SyncDictionary<string, Scene> _hostedGames = new ();
    public SyncDictionary<string, Scene> HostedGames => _hostedGames;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ServerCallback]
    public IEnumerator ServerLoadScene(Player player, string sceneName)
    {
        DontDestroyOnLoad(player.connectionToClient.identity.gameObject);
        
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, _sceneLoadParams);
        
        yield return new WaitUntil(() => asyncLoad.isDone);
        
        if (!_hostedGames.ContainsKey(player.Name))
        {
            _hostedGames.Add(player.Name, SceneManager.GetActiveScene());
            player.connectionToClient.Send(new SceneMessage { sceneName = sceneName , sceneOperation = SceneOperation.Normal });
            SceneManager.MoveGameObjectToScene(player.connectionToClient.identity.gameObject, _hostedGames[player.Name]);
            
            DebugExt.Log(this, $"Scene {sceneName} loaded. Player {player.Name} moved to the scene. (Active scene {SceneManager.GetActiveScene().name})");
        }
        else
        {
            DebugExt.Log(this, $"Scene {sceneName} loaded, but it's already in the list.");
        }
    }

    [ServerCallback]
    public void ServerConnectPlayerToHost(Player player, string hostName)
    {
        if (_hostedGames.TryGetValue(hostName, out Scene scene))
        {
            player.connectionToClient.Send(new SceneMessage { sceneName = scene.name , sceneOperation = SceneOperation.Normal });
            SceneManager.MoveGameObjectToScene(player.gameObject, scene);
            RpcSetPlayerActive(player);
            DebugExt.Log(this, $"Connect player {player.Name} to {hostName}. (Active scene {SceneManager.GetActiveScene().name})");
        }
        else
        {
            DebugExt.Log(this, $"Can't connect player {player.Name} to {hostName}. No such host");
        }
    }

    [ClientRpc]
    private void RpcSetPlayerActive(Player player)
    {
        player.SetViewEnable();
    }
}

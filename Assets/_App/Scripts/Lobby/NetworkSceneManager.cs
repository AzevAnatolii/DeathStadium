using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : NetworkBehaviour
{
    public static NetworkSceneManager Instance;

    private readonly SyncDictionary<string, Scene> _availableScenes = new ();
    public SyncDictionary<string, Scene> AvailableScenes => _availableScenes;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [ServerCallback]
    public IEnumerator ServerCreateSubScene(Player player)
    {
        var asyncLoad = SceneManager.LoadSceneAsync("GameScene", new LoadSceneParameters
        {
            loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D
        });
        
        yield return new WaitUntil(() => asyncLoad.isDone);
        
        var lastLoadedSceneIndex = SceneManager.sceneCount - 1;
        var lastLoadedScene = SceneManager.GetSceneAt(lastLoadedSceneIndex);

        if (!_availableScenes.ContainsKey(player.Name))
        {
            _availableScenes.Add(player.Name, lastLoadedScene);
            player.connectionToClient.Send(new SceneMessage { sceneName = lastLoadedScene.name , sceneOperation = SceneOperation.LoadAdditive });
            SceneManager.MoveGameObjectToScene(player.connectionToClient.identity.gameObject, _availableScenes[player.Name]);
            
            Debug.Log($"Scene for {player.Name} Created!");
        }
    }

    [ServerCallback]
    public void ServerConnectPlayerToMatch(Player player, string matchName)
    {
        if (_availableScenes.ContainsKey(matchName))
        {
            var scene = _availableScenes[matchName];
            player.connectionToClient.Send(new SceneMessage { sceneName = scene.name , sceneOperation = SceneOperation.LoadAdditive });
            SceneManager.MoveGameObjectToScene(player.gameObject, scene);
            //NetworkServer.AddPlayerForConnection(player.connectionToClient, player.gameObject);
            RpcSetPlayerActive(player);
            _availableScenes.Remove(matchName);
        }
    }

    [ClientRpc]
    private void RpcSetPlayerActive(Player player)
    {
        player.SetViewEnable();
    }
}
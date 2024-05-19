using System.Collections;
using _App.Scripts.Game;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _App.Scripts.Core
{
    internal class SceneManagement : NetworkBehaviour
    {
        public static SceneManagement Instance { get; private set; }

        private readonly LoadSceneParameters _sceneLoadParams = new (LoadSceneMode.Additive);

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
        public IEnumerator ServerLoadScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, _sceneLoadParams);
            yield return new WaitUntil(() => asyncLoad.isDone);
            Scene scene = SceneManager.GetSceneByName(sceneName);
            //player.connectionToClient.Send(new SceneMessage { sceneName = sceneName , sceneOperation = SceneOperation.LoadAdditive });
            //SceneManager.MoveGameObjectToScene(player.connectionToClient.identity.gameObject, scene);
        }

        [ServerCallback]
        public void ServerConnectPlayerToHost(string hostName)
        {
            //player.connectionToClient.Send(new SceneMessage { sceneName = scene.name , sceneOperation = SceneOperation.LoadAdditive });
            //SceneManager.MoveGameObjectToScene(player.gameObject, scene);
            //DebugExt.Log(this, $"Connect player {player.Name} to {hostName}. (Active scene {SceneManager.GetActiveScene().name})");
        }
    }
}

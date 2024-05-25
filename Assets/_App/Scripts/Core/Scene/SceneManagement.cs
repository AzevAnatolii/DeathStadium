using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _App.Scripts.Core
{
    internal class SceneManagement : NetworkSingleton<SceneManagement>
    {
        private readonly LoadSceneParameters _sceneLoadParams = new (LoadSceneMode.Additive);

        public void LoadLobby(LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(1, mode);
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

using Mirror;
using UnityEngine;

namespace _App.Scripts
{
    internal class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
    {
	    public static T Instance { get; private set; }
	    
        [SerializeField] private bool _doNotDestroyOnLoad;
        
        private void Awake()
        {
	        if (Instance != null)
	        {
		        Destroy(gameObject);
	        }
	        
	        Instance = this as T;
	        if (_doNotDestroyOnLoad)
	        {
		        DontDestroyOnLoad(gameObject);
	        }
        }
    }
}

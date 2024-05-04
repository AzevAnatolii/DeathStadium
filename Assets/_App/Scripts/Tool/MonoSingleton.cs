using UnityEngine;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	#region Singleton
	private static T _instance;
	private static readonly object _lock = new ();
	private static bool _isShuttingDown;

	public static T Instance
	{
		get
		{
			if (_isShuttingDown)
			{
				//return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					InitInstance();
				}

				return _instance;
			}
		}
	}
	#endregion

	private void Awake()
	{
		lock (_lock)
		{
			if (_instance == this)
			{
				return;
			}
			
			if (_instance != null)
			{
				Debug.LogError($"There is more than one objects of singletone type '{typeof(T).Name} on the scene'");
				enabled = false;
				return;
			}
			
			InitInstance(this as T);
		}
	}

	private void OnDestroy()
	{
		_instance = null;
		_isShuttingDown = true;

		OnDestroySingleton();
	}

	private void OnApplicationQuit()
	{
		_isShuttingDown = true;
	}

	private static void InitInstance(T singleton = null)
	{
		if (singleton != null)
		{
			_instance = singleton;
		}
		else
		{
			_instance = FindObjectOfType<T>();
			if (_instance == null)
			{
				_instance = new GameObject(typeof(T).Name).AddComponent<T>();
			}
		}

		_instance.InitSingleton();
	}

	protected virtual void InitSingleton() { }

	protected virtual void OnDestroySingleton() { }
}
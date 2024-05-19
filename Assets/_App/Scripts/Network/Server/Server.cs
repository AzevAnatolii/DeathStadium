using System.Collections.Generic;
using Mirror;

namespace _App.Scripts.Network
{
	internal class Server : NetworkBehaviour
	{
		public static Server Instance { get; private set; }

		private Dictionary<string, Client> _authorizedClients = new();

		
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

		[Server]
		public void LogInClient(Client client, string clientName)
		{
			DebugExt.Log(this, $"LogIn {clientName}");
			if (_authorizedClients.ContainsKey(clientName))
			{
				client.SendAuthorizationResponse(client.connectionToClient, (int)ErrorCode.BadRequest, $"{clientName} is already exist");
				return;
			}
			if (NameChecker.IsNameValid(clientName) == false)
			{
				client.SendAuthorizationResponse(client.connectionToClient, (int)ErrorCode.BadRequest, $"{clientName} is not valid name");
				return;
			}
			
			_authorizedClients.Add(clientName, client);
			client.SendAuthorizationResponse(client.connectionToClient, (int)ErrorCode.Ok, clientName);
		}
	}
}
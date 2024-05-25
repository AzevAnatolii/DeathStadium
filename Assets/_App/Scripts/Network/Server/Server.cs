using System.Collections.Generic;
using Mirror;

namespace _App.Scripts.Network
{
	internal class Server : NetworkSingleton<Server>
	{
		public Dictionary<string, Client> AuthorizedClients { get; } = new();


		[Server]
		public void TryLogInClient(Client client, string clientName)
		{
			ErrorCode errorCode;
			string message;
			if (NameChecker.IsNameValid(clientName) == false)
			{
				errorCode = ErrorCode.BadRequest;
				message = $"{clientName} is not valid name";
			}
			else if (AuthorizedClients.ContainsKey(clientName))
			{
				errorCode = ErrorCode.BadRequest;
				message = $"{clientName} is already exist";
			}
			else
			{
				errorCode = ErrorCode.Ok;
				message = clientName;
				client.Name = clientName;
				AuthorizedClients.Add(clientName, client);
			}
			
			DebugExt.Log(this, $"TryLogInClient {clientName}  {errorCode}  {message}");
			client.SendAuthorizationResponse(client.connectionToClient, (int)errorCode, message);
		}
	}
}
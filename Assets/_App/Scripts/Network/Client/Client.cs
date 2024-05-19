using _App.Scripts.Lobby;
using Mirror;
using UnityEngine;

namespace _App.Scripts.Network
{
	internal partial class Client : NetworkBehaviour
	{
		public static Client LocalClient { get; private set; }

		[SerializeField] private string _name;

		public bool IsLoggedIn { get; private set; }
		public string Name => _name;

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();

			DebugExt.Log(this, $"OnStartAuthority {isLocalPlayer}");
			if (isLocalPlayer)
			{
				LocalClient = this;
			}
		}
				
		public void SetLoggedIn(string clientName)
		{
			IsLoggedIn = true;
			_name = clientName;
		}

		[Command]
		public void LogIn(string clientName)
		{
			DebugExt.Log(this, $"LogIn {clientName}");
			Server.Instance.LogInClient(this, clientName);
		}
		
		
		[TargetRpc]
		public void SendAuthorizationResponse(NetworkConnectionToClient conn, int errorCode, string message)
		{
			DebugExt.Log(this, $"SendResponse {(ErrorCode)errorCode} - {message}");
			LobbyController.Instance.OnAuthorizationResponse(errorCode, message);
		}
	}
}
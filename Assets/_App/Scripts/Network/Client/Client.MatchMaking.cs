using _App.Scripts.Lobby.Matches;
using Mirror;

namespace _App.Scripts.Network
{
	internal partial class Client
	{
		[Command]
		public void CreateMatch(string levelName)
		{
			MatchMaker.Instance.CreateMatch(this, levelName);
		}
		
		[Command]
		public void UpdateMatches()
		{
			string json = MatchMaker.Instance.GetMatchesJson();	
			DebugExt.Log(this, $"UpdateMatches {json}");
			UpdateMatches(connectionToClient, json);
		}
		
		[TargetRpc]
		public void UpdateMatches(NetworkConnectionToClient conn, string json)
		{
			DebugExt.Log(this, $"UpdateMatches {json}");
			MatchMaker.Instance.UpdateMatchesFromJson(json);
		}
				
		[TargetRpc]
		public void SendMatchMakingResponse(NetworkConnectionToClient conn, string message)
		{
			DebugExt.Log(this, $"MatchMakingResponse {message}");
		}
	}
}
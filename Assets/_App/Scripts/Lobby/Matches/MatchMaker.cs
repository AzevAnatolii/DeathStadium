using System;
using System.Collections.Generic;
using Mirror;
using Newtonsoft.Json;
using _App.Scripts.Network;

namespace _App.Scripts.Lobby.Matches
{
	internal class MatchMaker : NetworkSingleton<MatchMaker>
	{
		private List<Match> _matches = new();

		public event Action<List<Match>> OnListUpdated;


		[Server]
		public void CreateMatch(Client host, string levelName)
		{
			if (_matches.Exists(x => x.hostName == host.Name))
			{
				host.SendMatchMakingResponse(host.connectionToClient, "This client already has match");
				DebugExt.LogWarning(this, $"Can't create match. {host.Name} already has match");
				return;
			}

			DebugExt.Log(this, $"CreateMatch {host.Name}");
			Match match = new(host.Name, levelName);
			_matches.Add(match);

			foreach (var kvp in Server.Instance.AuthorizedClients)
			{
				kvp.Value.UpdateMatches(kvp.Value.connectionToClient, GetMatchesJson());
			}
		}

		[Server]
		public string GetMatchesJson()
		{
			return JsonConvert.SerializeObject(_matches);
		}

		[Client]
		public void UpdateMatchesFromJson(string json)
		{
			_matches = JsonConvert.DeserializeObject<List<Match>>(json);
			OnListUpdated?.Invoke(_matches);
		}
	}
}
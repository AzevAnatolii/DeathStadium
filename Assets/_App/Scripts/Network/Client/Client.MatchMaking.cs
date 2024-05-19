using _App.Scripts.Lobby.Matches;
using Mirror;

namespace _App.Scripts.Network
{
	internal partial class Client
	{
		[Command]
		public void CreateMatch(string levelName)
		{
			MatchMaker.Instance.CreateMatch(levelName, this);
		}
	}
}
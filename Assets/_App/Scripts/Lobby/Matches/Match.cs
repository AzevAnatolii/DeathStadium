using System.Collections.Generic;
using _App.Scripts.Game;

namespace _App.Scripts.Lobby.Matches
{
    internal class Match
    {
        public readonly List<Player> players = new ();

        public Match() { }
    
        public Match(Player player)
        {
            players.Add(player);
        }
    }
}

using System.Collections.Generic;

public class Match
{
    public readonly List<Player> players = new ();

    public Match() { }
    
    public Match(Player player)
    {
        players.Add(player);
    }
}

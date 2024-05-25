namespace _App.Scripts.Lobby.Matches
{
    internal struct Match
    {
        public readonly string levelName;
        public readonly string hostName;
        
        public Match(string hostName, string levelName)
        {
            this.hostName = hostName;
            this.levelName = levelName;
        }
    }
}

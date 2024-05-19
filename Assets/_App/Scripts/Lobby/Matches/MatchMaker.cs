using _App.Scripts.Network;
using Mirror;

namespace _App.Scripts.Lobby.Matches
{
    internal class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker Instance { get; private set; }

        private readonly SyncList<Match> _matches = new ();


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void CreateMatch(string levelName, Client client)
        {
            
        }
    }
}

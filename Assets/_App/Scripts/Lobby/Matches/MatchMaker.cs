using Mirror;

public class MatchMaker : NetworkBehaviour
{
    public static MatchMaker Instance { get; private set; }

    private readonly SyncList<Match> _matches = new ();
    private readonly SyncList<string> _matchIDs = new ();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}

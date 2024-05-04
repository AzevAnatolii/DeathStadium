using Mirror;
using UnityEngine;

public class PlayerMover : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        enabled = true;
    }
    
    public override void OnStopAuthority()
    {
        enabled = false;
    }
}

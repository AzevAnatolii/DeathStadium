using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed = 8f;
    
    private Vector2 _moveVector;

    private void Start()
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = true;
    }

    public override void OnStartAuthority()
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = true;
    }
    
    public override void OnStopAuthority()
    {
        enabled = false;
    }

    private void Update()
    {
        if (enabled)
        {
            HandleMove(); 
        }
    }

    public void Unfreeze()
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    private void HandleMove()
    {
        _moveVector.x = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(_moveVector.x * speed, rigidbody2D.velocity.y);
    }
}

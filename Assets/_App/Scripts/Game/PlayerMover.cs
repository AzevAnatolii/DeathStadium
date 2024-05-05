using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : NetworkBehaviour
{
    [SerializeField] private float _speed;
    private PlayerInput _input;

    private void Start()
    {
        _input = new PlayerInput();
        _input.Enable();
    }

    private void Update()
    {
        if (_input.Input.WASD.IsPressed())
        {
            Vector2 vector = _input.Input.WASD.ReadValue<Vector2>();
            transform.position += new Vector3(vector.x * _speed, 0, vector.y * _speed);
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }
    
    public override void OnStopAuthority()
    {
        enabled = false;
    }
}

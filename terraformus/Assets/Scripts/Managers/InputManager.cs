using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 movement;
    public static Vector2 zoom;

    private InputAction _moveaction;
    private InputAction _zoomaction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _moveaction = playerInput.actions["Move"];
        _zoomaction = playerInput.actions["Zoom"];
    } 

    private void Update() {
        movement = _moveaction.ReadValue<Vector2>();
        zoom = _zoomaction.ReadValue<Vector2>();
    }
}

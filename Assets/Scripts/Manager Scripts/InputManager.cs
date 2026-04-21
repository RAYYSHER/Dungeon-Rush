using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool MenuOpenCloseInput { get; private set;}

    private PlayerInput _playerInput;
    private InputAction _menuOpenCloseAction;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];

    }

    void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame(); 
    }
}

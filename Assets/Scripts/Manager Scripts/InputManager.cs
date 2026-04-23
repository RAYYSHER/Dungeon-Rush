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

        Debug.Log($"PlayerInput null: {_playerInput == null}");
        Debug.Log($"PlayerInput enabled: {_playerInput?.enabled}");

        _playerInput.actions.Enable();
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];

    }

    void Start()
    {
        _playerInput.actions.Enable();
        Debug.Log($"[Start] Action enabled: {_menuOpenCloseAction.enabled}");
    }

    void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame(); 

        Debug.Log($"Action enabled: {_menuOpenCloseAction.enabled}");
        Debug.Log($"ActionMap: {_playerInput.currentActionMap?.name}");
    }
}

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

        else
        {
            Destroy(gameObject); // เพิ่มบรรทัดนี้
            return;
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
        // ดึงใหม่ทุก frame แทนที่จะใช้ cached reference
        var action = _playerInput.actions["MenuOpenClose"];
        action.Enable();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame(); 

        Debug.Log($"Action enabled: {_menuOpenCloseAction.enabled}");
        Debug.Log($"ActionMap: {_playerInput.currentActionMap?.name}");

        // ชั่วคราว
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            Debug.Log($"Any key pressed: {keyboard.anyKey.isPressed}");
        }
    }
}

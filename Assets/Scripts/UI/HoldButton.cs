using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HoldButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    ISelectHandler, IDeselectHandler
{
    [SerializeField] private UnityEvent onAction;
    [SerializeField] private float holdDelay  = 0.5f;
    [SerializeField] private float repeatRate = 0.1f;

    private bool _isHeld            = false;
    private bool _repeating         = false;
    private float _holdTimer        = 0f;
    private bool _isSelected        = false;
    private bool _gamepadWasPressed = false;
    private float _selectCooldown = 0f;

void Update()
{
    if (_isSelected && Gamepad.current != null)
    {
        bool pressed = Gamepad.current.buttonSouth.isPressed;
        Debug.Log($"[HoldButton] frame:{Time.frameCount} cooldown:{_selectCooldown:F2} pressed:{pressed} wasPressed:{_gamepadWasPressed} isHeld:{_isHeld}");

        if (_selectCooldown > 0f)
        {
            _selectCooldown -= Time.unscaledDeltaTime;
            _gamepadWasPressed = pressed;
            return;
        }

        if (pressed && !_gamepadWasPressed)
            BeginHold();
        else if (!pressed && _gamepadWasPressed)
            EndHold();

        _gamepadWasPressed = pressed;
    }

    if (!_isHeld) return;

    _holdTimer += Time.unscaledDeltaTime;
    float threshold = _repeating ? repeatRate : holdDelay;

    if (_holdTimer >= threshold)
    {
        onAction.Invoke();
        _holdTimer = 0f;
        _repeating = true;
    }
}

    private void BeginHold()
    {
        _isHeld    = true;
        _holdTimer = 0f;
        _repeating = false;
        onAction.Invoke(); // fire ทันทีตอนกด
    }

    private void EndHold()
    {
        _isHeld    = false;
        _holdTimer = 0f;
        _repeating = false;
    }

    // Mouse / Touch
    public void OnPointerDown(PointerEventData e) => BeginHold();
    public void OnPointerUp(PointerEventData e)   => EndHold();

    // Track selection เฉยๆ ไม่ trigger hold
    public void OnSelect(BaseEventData e)
    {
        Debug.Log($"[HoldButton] OnSelect — frame: {Time.frameCount}");
        if (!_isSelected) // ← เพิ่ม check ตรงนี้
        {
            _gamepadWasPressed = false;
            _selectCooldown    = 0.2f;
        }
    
        _isSelected = true;
    }

    public void OnDeselect(BaseEventData e)
    {
        Debug.Log($"[HoldButton] OnDeselect — frame: {Time.frameCount}");
        _isSelected = false;
        EndHold();
    }
}
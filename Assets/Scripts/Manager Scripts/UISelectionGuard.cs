using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISelectionGuard : MonoBehaviour
{
    private GameObject _lastSelected;
    private int _suppressFrames = 0;   // ระงับ Guard ชั่วคราวระหว่าง SelectAfterFrame transition

    void Update()
    {
        if (EventSystem.current == null) return;
        
        if (!gameObject.activeInHierarchy) return;

        if (Mouse.current != null && (
            Mouse.current.delta.ReadValue() != Vector2.zero ||
            Mouse.current.leftButton.isPressed))
            return;

        // ระงับชั่วคราวระหว่าง SelectAfterFrame transition
        // ป้องกัน Guard restore _lastSelected ผิดจังหวะ
        if (_suppressFrames > 0)
        {
            _suppressFrames--;
            return;
        }

        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null && current.activeInHierarchy)
        {
            _lastSelected = current;
        }
        else if (_lastSelected != null && _lastSelected.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject != _lastSelected)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
        }
    }

    // เรียกก่อน SelectAfterFrame เพื่อระงับ Guard ชั่วคราว 1 frame
    public void SuppressFor(int frames)
    {
        _suppressFrames = Mathf.Max(_suppressFrames, frames);
    }

    // เรียกตอนปิดเมนู เพื่อ clear state
    public void ClearLastSelected()
    {
        _lastSelected = null;
    }
}
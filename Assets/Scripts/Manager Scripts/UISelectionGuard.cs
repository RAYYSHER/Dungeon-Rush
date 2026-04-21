using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionGuard : MonoBehaviour
{
    private GameObject _lastSelected;

void Update()
{
    if (EventSystem.current == null) return;

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

    // เรียกตอนปิดเมนู เพื่อ clear state
    public void ClearLastSelected()
    {
        _lastSelected = null;
    }
}
using System;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    [Header("Doors / Barriers")]
    [SerializeField] private GameObject[] barriers;
    [SerializeField] private bool lockedOnStart = false;

    public event Action OnLocked;
    public event Action OnUnlocked;

    public bool IsLocked { get; private set; }

    void Start()
    {
        if (lockedOnStart) Lock();
        else Unlock();
    }

    public void Lock()
    {
        if (IsLocked) return;
        IsLocked = true;
        SetBarriers(true);
        OnLocked?.Invoke();
        Debug.Log($"[RoomLock] {gameObject.name} — Locked");
    }

    public void Unlock()
    {
        if (!IsLocked && !lockedOnStart) return;
        IsLocked = false;
        SetBarriers(false);
        OnUnlocked?.Invoke();
        Debug.Log($"[RoomLock] {gameObject.name} — Unlocked");
    }

    private void SetBarriers(bool active)
    {
        foreach (var barrier in barriers)
            if (barrier != null)
                barrier.SetActive(active);
    }
}
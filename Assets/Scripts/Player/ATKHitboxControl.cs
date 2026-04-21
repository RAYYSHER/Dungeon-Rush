using UnityEngine;

public class ATKHitboxControl : MonoBehaviour
{
    [SerializeField] private BoxCollider Hitbox;

    public void EnableHitbox()
    {
        Hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        Hitbox.enabled = false;
    }
}

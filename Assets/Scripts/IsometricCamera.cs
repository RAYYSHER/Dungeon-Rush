using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    private Player player;
    public Vector3 CameraOffset = new Vector3();

    void Awake()
    {
        // player = GameObject.FindWithTag("Player").GetComponent<Player>(); -> better option, but now its har to read.

        player = FindFirstObjectByType<Player>();
    }
    void Update()
    {
        transform.position = player.transform.position + CameraOffset;
    }
}

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody body;                     //add player's physic (from Rigidbody's component in Inspector)
    public float moveSpeed;                     //add player's movement speed (adjustable, use for increase velocity.)
    public float speedMultiplier = 2f;
    private float currentSpeed;                  
    public int jumpForce;
    public int dashForce;
    private PlayerController controls;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        controls = GetComponent<PlayerController>();
        currentSpeed = moveSpeed;
    }


    public void Walk(Vector3 direction, bool isSprinting)
    {
        float currentSpeed = isSprinting ? moveSpeed * speedMultiplier : moveSpeed;
        
        Debug.Log($"isSprinting: {isSprinting} | moveSpeed: {moveSpeed} | multiplier: {speedMultiplier} | currentSpeed: {currentSpeed}");
        
        body.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    public void Rotate(Vector2 InputJoystick)
    {
        float angle = Mathf.Atan2(InputJoystick.x, InputJoystick.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
      //Jump keybind
    public void Jump(InputAction.CallbackContext context)
    {
        body.AddForce(Vector3.up * jumpForce);   //add push force in Y axis, making player can jump.
        Debug.Log("jumped");                    //check in console if player already jump or not.
    }

    public void Dash(InputAction.CallbackContext context)
    {
        body.AddForce(transform.forward * dashForce);   //add push force in x axis, making player can dash.
        Debug.Log("dashed");                            //check in console if player already jump or not.
    }
}

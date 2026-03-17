using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody body;                     //add player's physic (from Rigidbody's component in Inspector)
    public int moveSpeed;                       //add player's movement speed (adjustable, use for increase velocity.)
    public int jumpForce;
    public int dashForce;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Walk(Vector3 direction)
    {
        body.MovePosition(body.position + (direction * moveSpeed * Time.fixedDeltaTime));
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

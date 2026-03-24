using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    #region Groundcheck (Jump only ground)

    [Header ("Groundcheck")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;

    #endregion

    #region Attributes
    private Rigidbody body;                     //add player's physic (from Rigidbody's component in Inspector)
    public float moveSpeed;                     //add player's movement speed (adjustable, use for increase velocity.)
    public float speedMultiplier = 2f;
    private float currentSpeed;                  
    public int jumpForce;
    public int dashForce;
    private Stress stressSystem;
    private PlayerController controls;

    #endregion

    #region Build-in Function

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        controls = GetComponent<PlayerController>();
        stressSystem = GetComponent<Stress>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

    }

    #endregion

    #region Method
    public void Walk(Vector3 direction, bool isSprinting)
    {
        float currentSpeed = isSprinting ? moveSpeed * speedMultiplier : moveSpeed;
        
        // Debug.Log($"isSprinting: {isSprinting} | moveSpeed: {moveSpeed} | multiplier: {speedMultiplier} | currentSpeed: {currentSpeed}");
        
        body.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);

        if (isSprinting == true)
        {
            // sprinting = stress, let Update() handle it
            // walking — do nothing, passive regen in Update() handles decrease
            stressSystem.SetUnderStress();
        }
        
    }

    public void Rotate(Vector2 InputJoystick)
    {
        float angle = Mathf.Atan2(InputJoystick.x, InputJoystick.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
      //Jump keybind
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded == false)
        {
            return;
        }

        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);   //add push force in Y axis, making player can jump.
        
    }

    public void Dash(InputAction.CallbackContext context)
    {
        body.AddForce(transform.forward * dashForce);   //add push force in x axis, making player can dash.
        // Debug.Log("dashed");

        stressSystem.IncreaseSTS(10f);
    }

    #endregion
}

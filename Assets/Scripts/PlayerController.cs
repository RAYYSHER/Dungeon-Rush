using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    //collect neccessary variables.
    //Input from controller, there's only X and Y axis, so we need to use Vector2 instead of 3.
    private Vector2 InputJoystickLeft; 
    private Vector2 InputJoystickRight;

    //allow developer to add InputAction directly in the Inspector tab. 
    public InputActionReference move;           //add move keybind (from InputAction)
    public InputActionReference look;           //add look keybind (from InputAction)
    public InputActionReference jump;           //add jump keybind (from InputAction)
    public InputActionReference dash;
    public InputActionReference skill01;        //add skill01 keybind (from InputAction)           //add dash keybind (from InputAction)
    public InputActionReference meleeAttack;    //add melee attack keybind (from InputAction)
    private Movement movement;
    private Skill skill;
    private AnimatorController playerAC;

    void Awake()
    {
        playerAC = GetComponent<AnimatorController>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
    }

    public void Control()
    {
        Move();
        
        if(InputJoystickRight.magnitude != 0)
            ManualRotate();  
    }

    public void HandleJoystickInput()
    {
        InputJoystickLeft = move.action.ReadValue<Vector2>();
        InputJoystickRight = look.action.ReadValue<Vector2>();
    }

    private void Move()
    {
        //3D world movement -> Joystick only have 2D spaces(X for left/right and Y for up/down).
        //but we need Z-axis in case of moving front/back in the 3D space (X, Y, Z).

        //We need to add input in Y-axis to the Z-axis in the command, and game will read to
        //move player in Z-axis using Y-axis, and add 0 to Y-axis to let it read no value.

        // OLD VERSION -> transform.position += new Vector3(moveInputJoystick.x, 0, moveInputJoystick.y) * moveSpeed * Time.deltaTime;
        
        //New version
        // collect new variable, only use for "move" (shorten the code). 
        Vector3 moveDirection = new Vector3
        (
            InputJoystickLeft.x
            ,0
            ,InputJoystickLeft.y
        );
        movement.Walk(moveDirection);
        
        // Auto rotate while walking
        if(InputJoystickLeft.magnitude != 0)
        {
            movement.Rotate(InputJoystickLeft);
            playerAC.SetWalk();
        }
        else if(InputJoystickLeft.magnitude == 0) // Idle/ not moving
        {
            playerAC.SetIdle();
        }
    }
    private void ManualRotate()
    {
        movement.Rotate(InputJoystickRight);
    }
   
    private void Attack(InputAction.CallbackContext context)
    {
        playerAC.TriggerAttack();
    }


    void OnEnable()
    {
        jump.action.started += movement.Jump;
        meleeAttack.action.started += Attack;
        dash.action.started += movement.Dash;
        skill01.action.started += skill.Heal;
    }   
    void OnDisable()
    {
        jump.action.started -= movement.Jump;
        meleeAttack.action.started -= Attack;
        dash.action.started -= movement.Dash;
        skill01.action.started -= skill.Heal;
    }

}

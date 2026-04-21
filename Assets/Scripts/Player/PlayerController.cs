using System;
using System.Collections.Generic;
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
    public InputActionReference dash;           //add dash keybind (from InputAction)
    public InputActionReference sprint;         //add sprint keybind (from InputAction)
    public InputActionReference skill01;        //add skill01 keybind (from InputAction)          
    public InputActionReference meleeAttack;    //add melee attack keybind (from InputAction)
    private Movement movement;
    private Skill skill;
    private AnimatorController playerAC;
    private Player player;
    bool sprinting;
    private List<GameObject> nearbyEnemies;

    [SerializeField] private AudioClip[] attackSoundClips;

    void Awake()
    {
        playerAC = GetComponent<AnimatorController>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
        player = GetComponent<Player>();

        //Sprint button (hold)
        sprint.action.performed += SprintPerformed;
        sprint.action.canceled += SprintCanceled;

        nearbyEnemies = new List<GameObject>();
    }

    //Sprint
    public bool GetSprint()
    {
        return sprinting;
    }
    private void SprintPerformed(InputAction.CallbackContext context)
    {
        sprinting = true;
    }
    private void SprintCanceled(InputAction.CallbackContext context)
    {
        sprinting = false;
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

    public void Move()
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
        movement.Walk(moveDirection, sprinting);
        
        // Auto rotate while walking
        if(InputJoystickLeft.magnitude != 0)
        {
            if (!player.IsAttacking())
            {
                movement.Rotate(InputJoystickLeft);
            }
            playerAC.SetWalk();
        }
        else if(InputJoystickLeft.magnitude == 0) // Idle/ not moving
        {
            playerAC.SetIdle();
        }
    }
    private void ManualRotate()
    {
        if (!player.IsAttacking())
        {
            movement.Rotate(InputJoystickRight);
        }
       
    }
   
    private void Attack(InputAction.CallbackContext context)
    {
        // play sound FX at random
        SoundFXManager.instance.PlayRandomSoundFXClip(attackSoundClips, transform, 1f);

        //Rotate auto
        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            movement.RotateTo(nearestEnemy.position - this.transform.position);
        }
        playerAC.TriggerAttack();
        player.SetAttacking(true);
    }

    Transform FindNearestEnemy()
    {
        float aimRange = 5f;
        Collider[] hits = Physics.OverlapSphere(transform.position, aimRange); 


        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
            {
                continue;
            }
            
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = hit.transform;
            }
        }

        return nearestEnemy;
    
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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            nearbyEnemies.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            nearbyEnemies.Remove(other.gameObject);
        }
    }
}

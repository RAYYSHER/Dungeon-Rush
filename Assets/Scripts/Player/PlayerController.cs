using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    private Vector2 InputJoystickLeft; 
    private Vector2 InputJoystickRight;

    public InputActionReference move;
    public InputActionReference look;
    public InputActionReference dash;
    public InputActionReference sprint;
    public InputActionReference skill01;
    public InputActionReference skill02;
    public InputActionReference meleeAttack;

    private Movement movement;
    private Skill skill;
    private AnimatorController playerAC;
    private Player player;
    bool sprinting;
    private List<GameObject> nearbyEnemies;

    [SerializeField] private AudioClip[] attackSoundClips;

    private ActiveSkillExecutor activeSkillExecutor;

    void Awake()
    {
        playerAC = GetComponent<AnimatorController>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
        player = GetComponent<Player>();
        
        activeSkillExecutor = GetComponent<ActiveSkillExecutor>();

        sprint.action.performed += SprintPerformed;
        sprint.action.canceled  += SprintCanceled;

        nearbyEnemies = new List<GameObject>();
    }

    public bool GetSprint() => sprinting;

    private void SprintPerformed(InputAction.CallbackContext context) => sprinting = true;
    private void SprintCanceled(InputAction.CallbackContext context)  => sprinting = false;

    public void Control()
    {
        Move();
        if (InputJoystickRight.magnitude != 0)
            ManualRotate();  
    }

    public void HandleJoystickInput()
    {
        InputJoystickLeft  = move.action.ReadValue<Vector2>();
        InputJoystickRight = look.action.ReadValue<Vector2>();
    }

    public void Move()
    {
        Vector3 moveDirection = new Vector3(InputJoystickLeft.x, 0, InputJoystickLeft.y);
        movement.Walk(moveDirection, sprinting);
        
        if (InputJoystickLeft.magnitude != 0)
        {
            if (!player.IsAttacking())
                movement.Rotate(InputJoystickLeft);
            playerAC.SetWalk();
        }
        else
        {
            if (!movement.IsDashing)
                movement.Stop();
            playerAC.SetIdle();
        }
    }

    private void ManualRotate()
    {
        if (!player.IsAttacking())
            movement.Rotate(InputJoystickRight);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(attackSoundClips, transform, 1f);

        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
            movement.RotateTo(nearestEnemy.position - this.transform.position);

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
            if (!hit.CompareTag("Enemy")) continue;
            
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance  = distance;
                nearestEnemy = hit.transform;
            }
        }
        return nearestEnemy;
    }

    void OnEnable()
    {
        meleeAttack.action.started += Attack;
        dash.action.started        += movement.Dash;
        skill01.action.started     += _ => activeSkillExecutor.UseSkill(0);
        skill02.action.started     += _ => activeSkillExecutor.UseSkill(1);
    }

    void OnDisable()
    {
        meleeAttack.action.started -= Attack;
        dash.action.started        -= movement.Dash;
        skill01.action.started     -= _ => activeSkillExecutor.UseSkill(0);
        skill02.action.started     -= _ => activeSkillExecutor.UseSkill(1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            nearbyEnemies.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            nearbyEnemies.Remove(other.gameObject);
    }
}
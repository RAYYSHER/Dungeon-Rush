using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region Groundcheck

    [Header("Groundcheck")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;

    #endregion

    #region Attributes

    private Rigidbody body;
    public float moveSpeed;
    public float speedMultiplier = 2f;
    public int jumpForce;
    private Stress stressSystem;
    private PlayerController controls;
    private Vector3 _lastMoveDirection;

    #endregion

    #region Dash Settings

    [Header("Dash")]
    public float dashingPower    = 24f;
    public float dashingTime     = 0.2f;
    public float dashingCooldown = 0.7f;

    [Header("Penalty")]
    [Range(0.1f, 1f)]
    public float penaltySpeedMultiplier = 0.5f;  // 0.5 = เดินได้ครึ่งความเร็ว

    public bool IsDashing { get; private set; }
    private bool _canDash = true;

    #endregion

    #region Unity Methods

    void Awake()
    {
        body         = GetComponent<Rigidbody>();
        controls     = GetComponent<PlayerController>();
        stressSystem = GetComponent<Stress>();

        body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    #endregion

    #region Public Methods

    public void Walk(Vector3 direction, bool isSprinting)
    {
        if (IsDashing) return;

        float speed;
        if (stressSystem.IsInPenalty)
        {
            // penalty — เดินได้แต่ช้าลง, sprint ปิด
            speed = moveSpeed * penaltySpeedMultiplier;
        }
        else
        {
            speed = isSprinting ? moveSpeed * speedMultiplier : moveSpeed;
            if (isSprinting)
                stressSystem.SetUnderStress();
        }

        body.MovePosition(transform.position + direction * speed * Time.deltaTime);

        if (direction.magnitude > 0.01f)
            _lastMoveDirection = direction.normalized;
    }

    public void Rotate(Vector2 input)
    {
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void RotateTo(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(LockYAfterJump());
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!_canDash) return;
        if (stressSystem.IsInPenalty) return;   // penalty — dash ไม่ได้

        Vector3 dashDir = _lastMoveDirection.magnitude > 0.01f
            ? _lastMoveDirection
            : transform.forward;

        StartCoroutine(DashCoroutine(dashDir));
        stressSystem.IncreaseSTS(10f);
    }

    #endregion

    #region Coroutines

    IEnumerator DashCoroutine(Vector3 direction)
    {
        IsDashing = true;
        _canDash  = false;

        RotateTo(direction);

        float elapsed = 0f;
        while (elapsed < dashingTime)
        {
            // abort ถ้า penalty trigger ตอน mid-dash
            if (stressSystem.IsInPenalty)
                break;

            body.MovePosition(body.position + direction * dashingPower * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        IsDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    IEnumerator LockYAfterJump()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => isGrounded);
        body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    #endregion
}
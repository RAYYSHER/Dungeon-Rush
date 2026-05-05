using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region Attributes

    private Rigidbody body;
    public float moveSpeed;
    public float speedMultiplier = 2f;
    private Stress stressSystem;
    private PlayerController controls;
    private Vector3 _lastMoveDirection;
    private Vector3 _dashVelocity = Vector3.zero;

    #endregion

    #region Dash

    [Header("Dash")]
    public float dashingPower    = 32f;
    public float dashingTime     = 0.25f;
    public float dashingCooldown = 0.7f;
    public float dashSTSCost     = 10f;

    [Header("Penalty")]
    [Range(0.1f, 1f)]
    public float penaltySpeedMultiplier = 0.5f;

    public bool IsDashing { get; private set; } = false;
    private bool _canDash = true;
    private Animator animator;

    #endregion

    #region Unity Methods

    void Awake()
    {
        body         = GetComponent<Rigidbody>();
        controls     = GetComponent<PlayerController>();
        stressSystem = GetComponent<Stress>();
        animator = GetComponent<Animator>();

        body.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    #endregion

    #region Public Methods

    public void Walk(Vector3 direction, bool isSprinting)
    {
        if (IsDashing)
        {
            body.linearVelocity = new Vector3(_dashVelocity.x, body.linearVelocity.y, _dashVelocity.z);
            return;
        }

        if (stressSystem.IsInPenalty)
        {
            body.linearVelocity = new Vector3(0f, body.linearVelocity.y, 0f);

            if (direction.magnitude > 0.01f)
            {
                float penaltySpeed = moveSpeed * penaltySpeedMultiplier;
                body.MovePosition(body.position +
                    new Vector3(direction.x, 0f, direction.z) * penaltySpeed * Time.fixedDeltaTime);
                _lastMoveDirection = direction.normalized;
            }
            return;
        }

        float speed = isSprinting ? moveSpeed * speedMultiplier : moveSpeed;
        if (isSprinting)
            stressSystem.SetUnderStress();

        body.linearVelocity = new Vector3(
            direction.x * speed,
            body.linearVelocity.y,
            direction.z * speed
        );

        if (direction.magnitude > 0.01f)
            _lastMoveDirection = direction.normalized;
    }

    public void Stop()
    {
        if (IsDashing) return;
        body.linearVelocity = new Vector3(0, body.linearVelocity.y, 0);
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

    public void Dash(InputAction.CallbackContext context)
    {
        if (!_canDash) return;
        if (stressSystem.IsInPenalty) return;

        Vector3 dashDir = _lastMoveDirection.magnitude > 0.01f
            ? _lastMoveDirection
            : transform.forward;

        stressSystem.IncreaseSTS(dashSTSCost);
        StartCoroutine(DashCoroutine(dashDir));
    }

    #endregion

    #region Coroutines

    IEnumerator DashCoroutine(Vector3 direction)
    {
        IsDashing     = true;
        _canDash      = false;
        _dashVelocity = direction * dashingPower;

        RotateTo(direction);

        float elapsed = 0f;
        while (elapsed < dashingTime)
        {
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _dashVelocity = Vector3.zero;
        IsDashing     = false;

        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    #endregion
}
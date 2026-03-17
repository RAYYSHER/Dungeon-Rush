using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetWalk()
    {
        animator.SetBool("isWalk", true);
    }

    public void SetIdle()
    {
        animator.SetBool("isWalk", false);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("trAttack");
    }


    // public void SetBool(string parameter, bool value)
    // {
    //     PlayerAnimator.SetBool(parameter, value);
    // }

    // public void SetTrigger(string parameter)
    // {
    //     PlayerAnimator.SetTrigger(parameter);
    // }
}

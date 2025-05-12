using UnityEngine;

public class FlintlockUIAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        WeaponEvents.OnFlintlockFired += PlayAnimation;
    }

    void OnDisable()
    {
        WeaponEvents.OnFlintlockFired -= PlayAnimation;
    }

    void PlayAnimation()
    {
        if (animator != null && animator.HasState(0, Animator.StringToHash("Flintlock Shoot_Clip")))
        {
            animator.Play("Flintlock Shoot_Clip", 0, 0f);
        }
    }
}

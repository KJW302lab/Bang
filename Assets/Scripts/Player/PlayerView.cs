using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int Fire   = Animator.StringToHash("Fire");
    private static readonly int Hit    = Animator.StringToHash("Hit");
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _animator.SetBool(IsMove, false);
    }

    public void PlayMove()
    {
        _animator.SetBool(IsMove, true);
    }

    public void PlayFire()
    {
        _animator.SetTrigger(Fire);
    }

    public void PlayHit()
    {
        _animator.SetTrigger(Hit);
    }
}
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");
    private static readonly int Fire   = Animator.StringToHash("Fire");
    private static readonly int Dead   = Animator.StringToHash("Dead");
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _animator.SetBool(IsWalk, false);
    }

    public void PlayMove()
    {
        _animator.SetBool(IsWalk, true);
    }

    public void PlayFire()
    {
        _animator.SetTrigger(Fire);
    }

    public void PlayDead()
    {
        _animator.SetTrigger(Dead);
    }
}
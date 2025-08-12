using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;
        
        HandleMove();
        HandleFire();
    }

    private void HandleMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        if (move == Vector3.zero)
        {
            _animator.SetBool("IsWalk", false);
            return;
        }
        
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        _animator.SetBool("IsWalk", true);
    }

    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) == false) return;
        
        _animator.SetTrigger("Fire");
    }
}
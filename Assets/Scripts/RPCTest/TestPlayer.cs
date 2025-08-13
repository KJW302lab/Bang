using Photon.Pun;
using UnityEngine;

public class TestPlayer : MonoBehaviourPun
{
    [SerializeField] private RpcTarget target;
    [SerializeField] private float     jumpForce;
    
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
            photonView.RPC("Jump", target, jumpForce);
    }

    [PunRPC]
    private void Jump(float force)
    {
        print($"점프 호출되었음, FORCE : {force}");
        
        _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}
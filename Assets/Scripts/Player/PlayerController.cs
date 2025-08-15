using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player     _model;
    private PlayerView _view;
    
    private void Awake()
    {
        _model = GetComponent<Player>();
        _view  = GetComponent<PlayerView>();
    }

    private void Update()
    {
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
            _view.PlayIdle();
            return;
        }
        
        transform.Translate(move.normalized * _model.MoveSpeed * Time.deltaTime, Space.World);
        Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _model.RotateSpeed);
        _view.PlayMove();
    }

    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) == false) return;
        
        SnowBall.Spawn(transform.position + Vector3.up)
            .Fire(transform.forward, 10f);
        
        _view.PlayFire();
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player     _model;
    private PlayerView _view;
    private Camera     _cam;
    
    private void Awake()
    {
        _cam   = Camera.main;
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

        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
        {
            _view.PlayIdle();
            return;
        }

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight   = _cam.transform.right;

        camForward.y = camRight.y   = 0;

        Vector3 move = camForward * v + camRight * h;
        move.Normalize();

        transform.Translate(move * _model.MoveSpeed * Time.deltaTime, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _model.RotateSpeed);
        
        _view.PlayMove();
    }


    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) == false) return;
        
        _view.PlayFire();
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _model;
    private PlayerView _view;
    private Camera _cam;
    
    private float _xRotation = 0f;

    private void Awake()
    {
        _cam = Camera.main;
        _model = GetComponent<Player>();
        _view = GetComponent<PlayerView>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cam.transform.SetParent(transform);
        _cam.transform.localPosition = new Vector3(0f, 1.75f, 0f);
        _cam.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        HandleLook();
        HandleMove();
        HandleFire();
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _model.RotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _model.RotateSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _cam.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
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

        Vector3 move = transform.right * h + transform.forward * v;
        move.Normalize();

        transform.Translate(move * _model.MoveSpeed * Time.deltaTime, Space.World);
        _view.PlayMove();
    }

    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) == false) return;
        _view.PlayFire();
    }
}
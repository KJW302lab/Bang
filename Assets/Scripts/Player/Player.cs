using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 1f;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public float RotateSpeed
    {
        get => rotateSpeed;
        set => rotateSpeed = value;
    }

    private PlayerView _view;

    private void Awake()
    {
        _view = gameObject.AddComponent<PlayerView>();

        if (photonView.IsMine)
            gameObject.AddComponent<PlayerController>();
    }
}
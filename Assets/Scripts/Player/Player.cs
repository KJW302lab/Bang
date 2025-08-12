using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public float MoveSpeed   { get; private set; } = 2f;
    public float RotateSpeed { get; private set; } = 15f;

    private PlayerView _view;

    private void Awake()
    {
        _view = gameObject.AddComponent<PlayerView>();
        
        if (photonView.IsMine)
            gameObject.AddComponent<PlayerController>();
    }
}
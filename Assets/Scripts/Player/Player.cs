using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 1f;
    
    private PlayerView _view;

    private int _health = 3;
    
    private void Awake()
    {
        _view = gameObject.AddComponent<PlayerView>();

        if (photonView.IsMine)
            Initialize();
    }

    private void Initialize()
    {
        gameObject.AddComponent<PlayerController>();
        CameraManager.Instance.SetFollow(transform);
    }
    
    [PunRPC]
    private void GetHit()
    {
        _view.PlayHit();
    }
}
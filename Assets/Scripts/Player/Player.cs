using ExitGames.Client.Photon;
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

    private void OnEnable()
    {
        NetworkManager.Instance.OnPlayerPropertiesUpdatedEvent += OnPlayerPropsUpdated;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.OnPlayerPropertiesUpdatedEvent -= OnPlayerPropsUpdated;
    }

    private void Initialize()
    {
        gameObject.AddComponent<PlayerController>();
        CameraManager.Instance.SetFollow(transform);
        
        NetworkManager.Instance.SetPlayerProperties("Nickname", NetworkManager.Instance.LocalPlayer.NickName);
    }

    private void OnPlayerPropsUpdated(Photon.Realtime.Player player, Hashtable props)
    {
        if (photonView.Owner.ActorNumber != player.ActorNumber) return;

        foreach (var entry in props)
        {
            string key = (string)entry.Key;
            object value = entry.Value;

            switch (key)
            {
                case "Nickname":
                    _view.SetNickname((string)value);
                    break;
            }
        }
    }
    
    [PunRPC]
    private void GetHit()
    {
        _view.PlayHit();
    }
}
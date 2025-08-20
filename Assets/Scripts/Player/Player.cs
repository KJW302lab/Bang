using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviourPun
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 1f;
    
    private PlayerView       _view;
    private PlayerController _controller;

    private int _maxHealth;
    private int _health;

    private bool _isDead;
    
    private void Awake()
    {
        _view = gameObject.AddComponent<PlayerView>();

        if (photonView.IsMine)
            Initialize();
    }

    private void Start()
    {
        _health = _maxHealth = 5;
        
        _view.SetHealth(_health, _maxHealth);
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
        _controller = gameObject.AddComponent<PlayerController>();
        
        CameraManager.Instance.SetFollow(transform);
        
        NetworkManager.Instance.SetPlayerProperties("Nickname", NetworkManager.Instance.LocalPlayer.NickName);

        NetworkManager.Instance.OnOtherPlayerLeftEvent += _ => CheckWonGame();
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

        _health = Mathf.Clamp(_health - 1, 0, _maxHealth);
        
        _view.SetHealth(_health, _maxHealth);

        if (_health > 0) return;

        if (photonView.IsMine == false) return;
        
        OnDead();
    }

    private void OnDead()
    {
        if (_isDead) return;

        _isDead = true;
        
        _controller.CanMove = false;
        
        NetworkManager.Instance.LeaveRoom();

        UIManager.Instance.Open<UIGameResult>()
            .Initialize(false);
    }

    private void CheckWonGame()
    {
        int leftPlayer = NetworkManager.Instance.CurrentPlayerCount;

        if (leftPlayer == 1)
        {
            NetworkManager.Instance.LeaveRoom();
            
            UIManager.Instance.Open<UIGameResult>()
                .Initialize(true);
        }
    }
}
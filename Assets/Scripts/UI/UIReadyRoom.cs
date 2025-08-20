using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UIReadyRoom : UIBase
{
    [SerializeField] private Transform  parent;
    [SerializeField] private PlayerSlot prefab;

    [SerializeField] private Button readyButton;
    
    private void OnEnable()
    {
        NetworkManager.Instance.OnOtherPlayerJoinedEvent       += AddSlot;
        NetworkManager.Instance.OnOtherPlayerLeftEvent         += RemoveSlot;
        NetworkManager.Instance.OnPlayerPropertiesUpdatedEvent += OnPlayerPropsUpdated;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.OnOtherPlayerJoinedEvent       -= AddSlot;
        NetworkManager.Instance.OnOtherPlayerLeftEvent         -= RemoveSlot;
        NetworkManager.Instance.OnPlayerPropertiesUpdatedEvent -= OnPlayerPropsUpdated;
    }

    private Dictionary<int, PlayerSlot> _playerSlots = new();
    private bool                        _isLocalReady;

    private void Awake()
    {
        readyButton.onClick.AddListener(OnReadyButtonPressed);
    }

    private void OnReadyButtonPressed()
    {
        _isLocalReady = !_isLocalReady;
        
        NetworkManager.Instance.SetPlayerProperties("IsReady", _isLocalReady);
    }

    private void OnPlayerPropsUpdated(Photon.Realtime.Player player, Hashtable props)
    {
        if (_playerSlots.TryGetValue(player.ActorNumber, out PlayerSlot slot) == false)
            return;

        bool isReady = (bool)props["IsReady"];
        slot.SetReadyLabel(isReady);

        if (NetworkManager.Instance.LocalPlayer.IsMasterClient == false || _playerSlots.Count <= 1)
            return;
        
        CheckAllPlayersReady();
    }

    public void Initialize(Room room)
    {
        foreach (var player in room.Players.Values)
            AddSlot(player);
    }

    private void AddSlot(Photon.Realtime.Player player)
    {
        PlayerSlot slot = Instantiate(prefab, parent);
        slot.Initialize(player);
        _playerSlots.Add(player.ActorNumber, slot);
        
        RefreshOrder();
    }

    private void RemoveSlot(Photon.Realtime.Player player)
    {
        int actorNumber = player.ActorNumber;
        
        if (_playerSlots.TryGetValue(actorNumber, out PlayerSlot slot) == false)
            return;
        
        Destroy(slot.gameObject);
        
        _playerSlots.Remove(actorNumber);
        
        RefreshOrder();
    }

    private void RefreshOrder()
    {
        var slots = _playerSlots
            .OrderBy(pair => pair.Key)
            .Select(pair => pair.Value);

        foreach (PlayerSlot slot in slots)
            slot.transform.SetAsLastSibling();
    }

    private void CheckAllPlayersReady()
    {
        bool isAllReady = _playerSlots
            .Values
            .All(slot =>
            {
                Hashtable props = slot.Player.CustomProperties;

                return props.ContainsKey("IsReady") && (bool)props["IsReady"];
            });

        if (isAllReady)
            NetworkManager.Instance.LoadSceneAllPlayers("PlayScene");
    }
}
using System;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomCard : MonoBehaviour
{
    [SerializeField] private Text   roomNameLabel;
    [SerializeField] private Text   playerCountLabel;
    [SerializeField] private Button selectButton;

    private RoomInfo         _room;
    private Action<RoomInfo> _onSelected;

    private void Awake()
    {
        selectButton.onClick.AddListener(()=> _onSelected.Invoke(_room));
    }

    public void Initialize(RoomInfo room, Action<RoomInfo> onSelected)
    {
        _room = room;
        _onSelected = onSelected;
        
        roomNameLabel.text = room.Name;
        playerCountLabel.text = $"[{room.PlayerCount}/{room.MaxPlayers}]";
        gameObject.SetActive(true);
    }
}
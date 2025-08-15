using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private RoomCard  cardPrefab;

    private readonly List<RoomCard> _cardList = new();

    public event Action<RoomInfo> OnRoomSelectedEvent; 

    private void OnEnable()
    {
        NetworkManager.Instance.OnRoomListUpdatedEvent += RefreshRoomList;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.OnRoomListUpdatedEvent -= RefreshRoomList;
    }

    private void RefreshRoomList(List<RoomInfo> roomList)
    {
        roomList = roomList
            .Where(room => room.RemovedFromList == false)
            .ToList();
        
        SetCardList(roomList.Count);
        
        for (var i = 0; i < roomList.Count; i++)
        {
            RoomCard card = _cardList[i];
            RoomInfo room = roomList[i];
            
            card.Initialize(room, selectedRoom => OnRoomSelectedEvent?.Invoke(selectedRoom));
        }
    }

    private void SetCardList(int roomCount)
    {
        _cardList.ForEach(card => card.gameObject.SetActive(false));

        int needCount = Mathf.Max(0, roomCount - _cardList.Count);

        for (int i = 0; i < needCount; i++)
        {
            RoomCard card = Instantiate(cardPrefab, parent);
            _cardList.Add(card);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class RoomListLayout : MonoBehaviour
{
    [SerializeField] private RoomCard  cardPrefab;
    [SerializeField] private Transform parent;

    private readonly List<RoomCard> _cards = new();

    private void SetCardList(int roomCount)
    {
        int currentCount = _cards.Count;
        int needToSpawn  = roomCount - currentCount;

        for (int i = 0; i < needToSpawn; i++)
        {
            RoomCard card = Instantiate(cardPrefab, parent);
            _cards.Add(card);
        }
    }

    public void SetRoomList(List<RoomInfo> roomList)
    {
        int roomCount = roomList
            .Count(room => room.RemovedFromList == false);
        
        SetCardList(roomCount);

        for (var i = 0; i < _cards.Count; i++)
        {
            RoomCard card = _cards[i];

            if (i >= roomList.Count || roomList[i].RemovedFromList)
            {
                card.gameObject.SetActive(false);
                continue;
            }

            card.gameObject.SetActive(true);
            card.SetCard(roomList[i]);
        }
    }
}
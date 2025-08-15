using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private NetworkManager _networkManager;
    
    private void Start()
    {
        _networkManager = NetworkManager.Instance;

        _networkManager.OnJoinedLobbyEvent     += OnJoinedLobby;
        _networkManager.OnRoomListUpdatedEvent += OnRoomListUpdated;
        _networkManager.OnJoinedRoomEvent      += OnJoinedRoom;
        _networkManager.ConnectToMaster();
    }

    private void OnJoinedLobby()
    {
        _networkManager.CreateRoom("한겜 하실분!", 2, OnCreateRoomFailed);
    }

    private void OnCreateRoomFailed(short errorCode)
    {
        Debug.Log($"방 생성 실패 : Code[{errorCode}]");
    }

    private void OnJoinedRoom(Room room)
    {
        Debug.Log($"방에 입장 : {room.Name} [{room.PlayerCount}/{room.MaxPlayers}]");
    }

    private void OnRoomListUpdated(List<RoomInfo> roomList)
    {
        string msg = "룸 리스트 갱신됨";

        if (roomList.Count == 0)
            msg += "\n입장 가능한 방이 없습니다.";
        else
        {
            foreach (var room in roomList)
                msg += $"\n{room.Name} [{room.PlayerCount}/{room.MaxPlayers}]";
        }
        
        Debug.Log(msg);
    }
}
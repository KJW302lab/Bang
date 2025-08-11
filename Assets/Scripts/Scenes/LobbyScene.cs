using System.Collections.Generic;
using NaughtyAttributes;
using Photon.Realtime;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private string nickname;
    [SerializeField] private string roomName;
    [SerializeField] private int    maxPlayer;

    [Button]
    private void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(roomName, maxPlayer);
    }

    [Button]
    private void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(roomName);
    }
    
    private void Start()
    {
        var networkManager = NetworkManager.Instance;
        
        networkManager.OnJoinedLobbyEvent      += OnJoinedLobby;
        networkManager.OnRoomListUpdatedEvent  += OnRoomListUpdated;
        networkManager.OnJoinedRoomEvent       += OnJoinedRoom;
        networkManager.OnRoomCreateFailedEvent += OnRoomCreateFailed;
        networkManager.OnRoomJoinFailedEvent   += OnRoomJoinFailed;
        
        networkManager.ConnectToMaster(nickname);
    }

    private void OnJoinedLobby()
    {
        Debug.Log("로비 접속");
    }
    
    private void OnRoomListUpdated(List<RoomInfo> roomList)
    {
        string msg = $"방 목록\n{new string('*', 50)}";

        if (roomList.Count == 0)
            msg += "\n방 목록이 비었습니다.";
        else
        {
            foreach (var room in roomList)
                msg += $"\n이름 : {room.Name} 인원 : {room.PlayerCount}/{room.MaxPlayers}";
        }
        
        Debug.Log(msg);
    }

    private void OnJoinedRoom(Room room)
    {
        string msg = $"{room.Name} 입장\n{new string('*', 50)}";

        var players = room.Players.Values;
        
        foreach (var player in players)
            msg += $"\n플레이어[{player.ActorNumber}] 닉네임 : {player.NickName}";
        
        Debug.Log(msg);
    }

    private void OnRoomCreateFailed(short errorCode)
    {
        Debug.Log(errorCode == 32766 ? "이미 같은 이름의 방이 존재합니다." : $"방 생성 실패 : {errorCode}");
    }

    private void OnRoomJoinFailed(short errorCode)
    {
        Debug.Log(errorCode == 32765 ? "방이 가득 찼습니다." : $"방 입장 실패 : {errorCode}");
    }
}
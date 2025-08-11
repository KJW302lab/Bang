using System.Collections.Generic;
using NaughtyAttributes;
using Photon.Realtime;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private string roomName;
    [SerializeField] private int    maxPlayer;
    
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void Connect()
    {
        NetworkManager.Instance.RoomListUpdated += OnRoomListUpdated;
        NetworkManager.Instance.ConnectToMaster();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void Disconnect()
    {
        NetworkManager.Instance.Disconnect();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void CreateRoom()
    {
        if (NetworkManager.IsConnected == false)
        {
            Debug.LogError("서버에 연결되지 않았습니다.");
            return;
        }
        
        RoomOptions options = new()
        {
            MaxPlayers = (byte)maxPlayer,
            IsVisible = true,
            IsOpen = true
        };

        NetworkManager.Instance.CreateRoom(roomName, options);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void JoinRoom()
    {
        NetworkManager.Instance.JoinRoom(roomName);
    }

    private void OnRoomListUpdated(List<RoomInfo> roomList)
    {
        string roomListMsg = "";
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
                continue;
            
            roomListMsg += $"\n제목 : {room.Name}\n최대 인원 : {room.MaxPlayers} 현재 인원 : {room.PlayerCount}";
        }
        Debug.Log(roomListMsg);
    }
}
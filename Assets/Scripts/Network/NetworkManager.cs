using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public static bool IsConnected => PhotonNetwork.IsConnected;

    public event Action<List<RoomInfo>> RoomListUpdated; 

    public void ConnectToMaster()
    {
        Debug.Log("마스터 서버에 연결 시도중...");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom(string roomName, RoomOptions roomOptions = null)
    {
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    #region Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log($"마스터 서버에 연결 : {PhotonNetwork.IsConnected}");
        Debug.Log($"로비 입장 시도...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"마스터 서버와 연결 끊김 : {cause}");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장");
    }

    public override void OnJoinedRoom()
    {
        var room = PhotonNetwork.CurrentRoom;
        var joinedPlayers = room.Players.Values.ToList();
        
        Debug.Log($"방 입장 \nName : {room.Name}\nMaxPlayer:{room.MaxPlayers}");

        string playerListMsg = "플레이어 목록 :";
        
        foreach (var player in joinedPlayers)
            playerListMsg += $"\n[{player.ActorNumber}] {player.NickName}";
        
        Debug.Log(playerListMsg);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 생성 실패 : {message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomListUpdated?.Invoke(roomList);
    }

    #endregion
}

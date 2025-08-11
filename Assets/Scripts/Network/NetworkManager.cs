using System;
using System.Collections.Generic;
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

    public event Action                  OnJoinedLobbyEvent;
    public event Action<DisconnectCause> OnDisconnectedEvent; 
    public event Action<List<RoomInfo>>  OnRoomListUpdatedEvent;
    public event Action<Room>            OnJoinedRoomEvent;
    public event Action<short>           OnRoomCreateFailedEvent;
    public event Action<short>           OnRoomJoinFailedEvent;

    private bool CanCreateOrJoinRoom() =>
        PhotonNetwork.NetworkClientState is ClientState.JoinedLobby or ClientState.ConnectedToMasterServer;

    public void ConnectToMaster(string nickName)
    {
        Debug.Log("마스터 서버에 접속 시도...");
        PhotonNetwork.LocalPlayer.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom(string roomName, int maxPlayer)
    {
        if (CanCreateOrJoinRoom() == false)
            return;
        
        PhotonNetwork.CreateRoom(roomName,
            new RoomOptions() { MaxPlayers = (byte)maxPlayer, IsVisible = true, IsOpen = true });
    }

    public void JoinRoom(string roomName)
    {
        if (CanCreateOrJoinRoom() == false)
            return;

        PhotonNetwork.JoinRoom(roomName);
    }
    
    /**************************************************Callbacks*******************************************************/
    /******************************************************************************************************************/

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        Debug.Log("로비 접속중...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (PhotonNetwork.IsConnected)
            return;
        
        OnDisconnectedEvent?.Invoke(cause);
    }

    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
            return;
        
        OnJoinedLobbyEvent?.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        OnRoomListUpdatedEvent?.Invoke(roomList);
    }

    public override void OnJoinedRoom()
    {
        OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        OnRoomCreateFailedEvent?.Invoke(returnCode);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        OnRoomJoinFailedEvent?.Invoke(returnCode);
    }
}
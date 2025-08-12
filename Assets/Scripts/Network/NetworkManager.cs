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

    public string LocalNickname
    {
        get => PhotonNetwork.LocalPlayer.NickName;
        set => PhotonNetwork.LocalPlayer.NickName = value;
    }

    private event Action<short> _onRoomCreateFailed;
    private event Action<short> _onRoomJoinFailed;

    public void ConnectToMaster()
    {
        Debug.Log("마스터 서버에 접속 시도...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom(string roomName, int maxPlayer, Action<short> onFailed = null)
    {
        _onRoomCreateFailed = onFailed;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = (byte)maxPlayer });
    }

    public void JoinRoom(string roomName, Action<short> onFailed = null)
    {
        _onRoomJoinFailed = onFailed;
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
        
        Debug.Log("로비 접속");
        OnJoinedLobbyEvent?.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        OnRoomListUpdatedEvent?.Invoke(roomList);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _onRoomCreateFailed?.Invoke(returnCode);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _onRoomJoinFailed?.Invoke(returnCode);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
            return;
        
        OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom);
    }
}
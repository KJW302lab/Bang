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

    public string LocalNickname
    {
        get => PhotonNetwork.LocalPlayer.NickName;
        set => PhotonNetwork.LocalPlayer.NickName = value;
    }

    public void ConnectToMaster()
    {
        Debug.Log("마스터 서버에 접속 시도...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
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
}
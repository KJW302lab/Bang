using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton

    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<NetworkManager>();

            if (_instance == null)
            {
                GameObject go = new GameObject(name: "NetworkManager");
                _instance = go.AddComponent<NetworkManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    #endregion

    public event Action                         OnJoinedLobbyEvent;
    public event Action<DisconnectCause>        OnDisconnectedEvent;
    public event Action<List<RoomInfo>>         OnRoomListUpdatedEvent;
    public event Action<Room>                   OnJoinedRoomEvent;
    public event Action                         OnLeftRoomEvent;
    public event Action<Photon.Realtime.Player> OnOtherPlayerJoinedEvent;
    public event Action<Photon.Realtime.Player> OnOtherPlayerLeftEvent;
    public event Action<Photon.Realtime.Player, Hashtable> OnPlayerPropertiesUpdatedEvent;

    public Photon.Realtime.Player LocalPlayer => PhotonNetwork.LocalPlayer;

    private event Action<short> _onRoomCreateFailed;
    private event Action<short> _onRoomJoinFailed;

    public bool IsConnected => PhotonNetwork.IsConnected;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnectToMaster()
    {
        if (PhotonNetwork.IsConnected)
            return;
        
        Debug.Log("마스터 서버에 접속 시도...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom(string roomName, int maxPlayer, Action<short> onFailed)
    {
        _onRoomCreateFailed = onFailed;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = (byte)maxPlayer });
    }

    public void JoinRoom(string roomName, Action<short> onFailed)
    {
        _onRoomJoinFailed = onFailed;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetPlayerProperties(string key, object value)
    {
        Hashtable props = new Hashtable();
        props[key] = value;
        LocalPlayer.SetCustomProperties(props);
    }

    public void LoadSceneAllPlayers(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        
        PhotonNetwork.LoadLevel(sceneName);
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
        OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnLeftRoom()
    {
        OnLeftRoomEvent?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnOtherPlayerJoinedEvent?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnOtherPlayerLeftEvent?.Invoke(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        OnPlayerPropertiesUpdatedEvent?.Invoke(targetPlayer, changedProps);
    }
}
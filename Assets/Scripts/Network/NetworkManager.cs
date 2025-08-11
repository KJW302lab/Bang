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

    public void ConnectToMaster()
    {
        Debug.Log("마스터 서버에 연결 시도중...");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public bool CreateRoom(string roomName, RoomOptions roomOptions = null)
    {
        return PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #region Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log($"마스터 서버에 연결 : {PhotonNetwork.IsConnected}");
        Debug.Log($"로비 입장 시도...");

        PhotonNetwork.LocalPlayer.NickName = "김지웅";
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

        string playerListMsg = "플레이어 목록 :\n";
        
        foreach (var player in joinedPlayers)
            playerListMsg += $"[{player.ActorNumber}] {player.NickName}";
        
        Debug.Log(playerListMsg);
    }
    #endregion
}

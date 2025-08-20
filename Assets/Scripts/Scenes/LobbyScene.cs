using Photon.Realtime;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private void Awake()
    {
        if (string.IsNullOrEmpty(NetworkManager.Instance.LocalPlayer.NickName))
        {
            var uiSetNickname = UIManager.Instance.Open<UISetNickname>();
            uiSetNickname.Initialize(OnNicknameSet);
        }
        else
            Connect();
    }

    private void OnEnable()
    {
        NetworkManager.Instance.OnJoinedLobbyEvent += OnJoinedLobby;
        NetworkManager.Instance.OnJoinedRoomEvent  += OnJoinedRoom;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.OnJoinedLobbyEvent -= OnJoinedLobby;
        NetworkManager.Instance.OnJoinedRoomEvent  -= OnJoinedRoom;
    }

    private void OnNicknameSet(string nickname)
    {
        NetworkManager.Instance.LocalPlayer.NickName = nickname;
        Connect();
    }

    private void Connect()
    {
        if (NetworkManager.Instance.State == ClientState.JoinedLobby)
        {
            OnJoinedLobby();
            return;
        }
        
        UIDim.Set("접속중");
        NetworkManager.Instance.ConnectToMaster();
    }

    private void OnJoinedLobby()
    {
        UIDim.Release();
        UIManager.Instance.Close<UISetNickname>();
        UIManager.Instance.Open<UILobby>();
    }

    private void OnJoinedRoom(Room room)
    {
        UIManager.Instance.Close<UILobby>();
        
        var readyRoom = UIManager.Instance.Open<UIReadyRoom>();
        readyRoom.Initialize(room);
    }
}
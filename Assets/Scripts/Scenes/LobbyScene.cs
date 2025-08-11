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
        RoomOptions options = new()
        {
            MaxPlayers = (byte)maxPlayer,
            IsVisible = true,
            IsOpen = true
        };

        bool canCreate = NetworkManager.Instance.CreateRoom(roomName, options);

        if (canCreate == false)
            Debug.Log("방 생성 실패 : 이미 같은 이름의 방이 존재합니다.");
    }
}
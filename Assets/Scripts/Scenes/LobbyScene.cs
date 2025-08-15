using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Instance.OnJoinedLobbyEvent += OnJoinedLobby;
        
        if (string.IsNullOrEmpty(NetworkManager.Instance.LocalNickname))
        {
            var uiSetNickname = UIManager.Instance.Open<UISetNickname>();
            uiSetNickname.Initialize(OnNicknameSet);
        }
        else
            Connect();
    }

    private void OnNicknameSet(string nickname)
    {
        NetworkManager.Instance.LocalNickname = nickname;
        Connect();
    }

    private void Connect()
    {
        UIDim.Set("접속중");
        NetworkManager.Instance.ConnectToMaster();
    }

    private void OnJoinedLobby()
    {
        UIDim.Release();
        UIManager.Instance.Close<UISetNickname>();
    }
}
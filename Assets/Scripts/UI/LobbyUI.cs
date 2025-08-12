using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("레이아웃")]
    [SerializeField] private GameObject     nicknameLayout;
    [SerializeField] private RoomListLayout roomListLayout;
    [SerializeField] private GameObject     roomCreatePopupLayout;
    [SerializeField] private Text           logLabel;
    
    [Header("닉네임 설정")]
    [SerializeField] private InputField nicknameInput;
    [SerializeField] private Button     btnConnect;

    [Header("방 생성/참가")]
    [SerializeField] private Button btnCreateRoom;
    [SerializeField] private Button btnJoinRoom;

    [Header("방 생성 팝업")]
    [SerializeField] private InputField roomNameInput;
    [SerializeField] private Dropdown   playerCountMenu;
    [SerializeField] private Button     btnCreateConfirm;

    private void Awake()
    {
        roomCreatePopupLayout.gameObject.SetActive(false);
        roomListLayout.gameObject.SetActive(false);
        nicknameLayout.gameObject.SetActive(true);
        
        btnConnect.onClick.AddListener(OnConnectButtonPressed);
        btnCreateRoom.onClick.AddListener(OnCreateButtonPressed);
        btnJoinRoom.onClick.AddListener(OnJoinButtonPressed);
        btnCreateConfirm.onClick.AddListener(OnCreateConfirmButtonPressed);
        
        ShowLog("");
    }

    private void Start()
    {
        NetworkManager.Instance.OnJoinedLobbyEvent     += ShowLobby;
        NetworkManager.Instance.OnRoomListUpdatedEvent += RefreshRoomList;
        NetworkManager.Instance.OnJoinedRoomEvent      += OnJoinedRoom;
    }
    
    private void OnDestroy()
    {
        if (NetworkManager.Instance == null) return;
        NetworkManager.Instance.OnJoinedLobbyEvent     -= ShowLobby;
        NetworkManager.Instance.OnRoomListUpdatedEvent -= RefreshRoomList;
        NetworkManager.Instance.OnJoinedRoomEvent      -= OnJoinedRoom;
    }

    private void OnConnectButtonPressed()
    {
        if (nicknameInput.text.Length == 0)
        {
            ShowLog("닉네임을 입력해주세요");
            return;
        }

        NetworkManager.Instance.LocalNickname = nicknameInput.text;
        nicknameInput.interactable = false;
        btnConnect.interactable = false;
        ShowLog("서버에 접속중입니다...");
        NetworkManager.Instance.ConnectToMaster();
    }

    private void OnCreateButtonPressed()
    {
        roomNameInput.text = null;
        playerCountMenu.value = 0;
        
        roomListLayout.gameObject.SetActive(false);
        nicknameLayout.gameObject.SetActive(false);
        roomCreatePopupLayout.gameObject.SetActive(true);
    }

    private void OnCreateConfirmButtonPressed()
    {
        if (roomNameInput.text.Length == 0)
        {
            ShowLog("방 이름을 작성해주세요");
            return;
        }

        int maxPlayers = playerCountMenu.value + 2;
        
        NetworkManager.Instance.CreateRoom(roomNameInput.text, maxPlayers, OnCreateRoomFailed);
    }

    private void OnJoinButtonPressed()
    {
        if (RoomCard.SelectedCard == null)
        {
            ShowLog("참가할 방을 선택해주세요.");
            return;
        }
        
        NetworkManager.Instance.JoinRoom(RoomCard.SelectedCard.GetRoomName(), OnJoinRoomFailed);
    }

    private void OnCreateRoomFailed(short errorCode)
    {
        ShowLog($"방 생성 실패 : {errorCode}");
    }

    private void OnJoinRoomFailed(short errorCode)
    {
        ShowLog($"방 참가 실패 : {errorCode}");
    }

    private void ShowLog(string message)
    {
        logLabel.text = message;
    }

    private void ShowLobby()
    {
        ShowLog("로비에 입장했습니다.");
        nicknameLayout.gameObject.SetActive(false);
        roomListLayout.gameObject.SetActive(true);
        roomCreatePopupLayout.gameObject.SetActive(false);
    }

    private void RefreshRoomList(List<RoomInfo> roomList)
    {
        roomListLayout.SetRoomList(roomList);
    }

    private void OnJoinedRoom(Room room)
    {
        
    }
}
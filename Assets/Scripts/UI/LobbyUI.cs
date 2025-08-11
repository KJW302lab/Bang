using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("레이아웃")]
    [SerializeField] private GameObject nicknameLayout;
    [SerializeField] private Text       logLabel;
    
    [Header("닉네임 설정")]
    [SerializeField] private InputField nicknameInput;
    [SerializeField] private Button     btnConnect;

    private void Awake()
    {
        btnConnect.onClick.AddListener(OnConnectButtonPressed);
        ShowLog("");
    }

    private void Start()
    {
        NetworkManager.Instance.OnJoinedLobbyEvent     += ShowLobby;
        NetworkManager.Instance.OnRoomListUpdatedEvent += RefreshRoomList;
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

    private void ShowLog(string message)
    {
        logLabel.text = message;
    }

    private void ShowLobby()
    {
        nicknameLayout.gameObject.SetActive(false);
    }

    private void RefreshRoomList(List<RoomInfo> roomList)
    {
        
    }
}
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIBase
{
    [SerializeField] private RoomList        roomList;
    [SerializeField] private Button          createButton;
    [SerializeField] private JoinRoomPopup   joinRoomPopup;
    [SerializeField] private CreateRoomPopup createRoomPopup;
    [SerializeField] private SimplePopup     simplePopup;

    private void Awake()
    {
        createButton.onClick.AddListener(OnCreateButtonPressed);
        roomList.OnRoomSelectedEvent += OnRoomSelected;
    }

    protected override void OnOpen()
    {
        joinRoomPopup.gameObject.SetActive(false);
        createRoomPopup.gameObject.SetActive(false);
        simplePopup.gameObject.SetActive(false);
    }

    #region TryJoin
    private void OnRoomSelected(RoomInfo room)
    {
        joinRoomPopup.AskToJoin(onConfirm: () => JoinRoom(room.Name));
    }

    private void JoinRoom(string roomName)
    {
        NetworkManager.Instance.JoinRoom(roomName, OnJoinFailed);
    }

    private void OnJoinFailed(short errorCode)
    {
        simplePopup.ShowPopup("방에 입장할 수 없습니다.");
    }
    #endregion

    #region TryCreate
    private void OnCreateButtonPressed()
    {
        createRoomPopup.AskToCreate(onCreate: CreateRoom);
    }

    private void CreateRoom(string roomName, int playerCount)
    {
        NetworkManager.Instance.CreateRoom(roomName, playerCount, OnCreateFailed);
    }

    private void OnCreateFailed(short errorCode)
    {
        simplePopup.ShowPopup("방을 생성할 수 없습니다.");
    }
    #endregion
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPopup : MonoBehaviour
{
    [SerializeField] private InputField roomNameInput;
    [SerializeField] private Text       placeHoldLabel;
    [SerializeField] private Dropdown   playerCountMenu;
    [SerializeField] private Button     createButton;
    [SerializeField] private Button     cancelButton;

    private Action<string, int> _onCreate;

    private void Awake()
    {
        createButton.onClick.AddListener(OnCreateButtonPressed);
        cancelButton.onClick.AddListener(OnCancelButtonPressed);
    }

    public void AskToCreate(Action<string, int> onCreate)
    {
        gameObject.SetActive(true);
        _onCreate = onCreate;
    }

    private void OnCreateButtonPressed()
    {
        string roomName = roomNameInput.text;

        if (string.IsNullOrEmpty(roomName)) return;

        int playerCount = playerCountMenu.value + 2;
        
        _onCreate?.Invoke(roomName, playerCount);
        
        gameObject.SetActive(false);
    }

    private void OnCancelButtonPressed()
    {
        roomNameInput.text = null;
        placeHoldLabel.text = "방 이름을 입력해주세요";
        gameObject.SetActive(false);
    }
}
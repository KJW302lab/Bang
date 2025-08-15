using System;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPopup : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private Action _onConfirm;
    private Action _onCancel;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonPressed);
        cancelButton.onClick.AddListener(()=> gameObject.SetActive(false));
    }

    public void AskToJoin(Action onConfirm)
    {
        gameObject.SetActive(true);
        _onConfirm = onConfirm;
    }

    private void OnConfirmButtonPressed()
    {
        _onConfirm?.Invoke();
        gameObject.SetActive(false);
    }
}
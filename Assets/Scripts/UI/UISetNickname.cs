using System;
using UnityEngine;
using UnityEngine.UI;

public class UISetNickname : UIBase
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button     connectButton;
    [SerializeField] private Text       placeHolderLabel;

    private Action<string> _onNicknameSet;

    private void Awake()
    {
        connectButton.onClick.AddListener(OnNicknameSet);
        inputField.onSubmit.AddListener(_ => OnNicknameSet());
    }

    protected override void OnOpen()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void Initialize(Action<string> onNicknameSet)
    {
        _onNicknameSet = onNicknameSet;
    }

    private void OnNicknameSet()
    {
        string nickname = inputField.text;
        
        if (string.IsNullOrEmpty(nickname))
            placeHolderLabel.text = "닉네임을 입력해주세요!";
        else
            _onNicknameSet?.Invoke(nickname);
    }
}
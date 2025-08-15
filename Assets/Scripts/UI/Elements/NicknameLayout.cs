using System;
using UnityEngine;
using UnityEngine.UI;

public class NicknameLayout : MonoBehaviour
{
    [SerializeField] private InputField nicknameInput;
    [SerializeField] private Button     btnConnect;

    private Action _callback;

    private void Awake()
    {
        btnConnect.onClick.AddListener(OnNicknameSet);
        nicknameInput.onSubmit.AddListener(_ => OnNicknameSet());
    }

    private void OnNicknameSet()
    {
        var nickname = nicknameInput.text;

        if (nickname.Length == 0)
            return;
        
        gameObject.SetActive(false);
        _callback?.Invoke();
    }
}
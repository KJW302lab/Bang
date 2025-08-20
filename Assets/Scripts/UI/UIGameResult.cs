using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameResult : UIBase
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Text   label;

    private void Awake()
    {
        confirmButton.onClick.AddListener(()=> SceneManager.LoadScene("LobbyScene"));
    }

    public void Initialize(bool winGame)
    {
        label.text = winGame ? "승리" : "패배";
    }
}
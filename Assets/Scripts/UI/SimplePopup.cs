using UnityEngine;
using UnityEngine.UI;

public class SimplePopup : MonoBehaviour
{
    [SerializeField] private Text   contentLabel;
    [SerializeField] private Button confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonPressed);
    }
    
    public void ShowPopup(string content)
    {
        gameObject.SetActive(true);
        contentLabel.text = content;
    }

    private void OnConfirmButtonPressed()
    {
        gameObject.SetActive(false);
        contentLabel.text = null;
    }
}
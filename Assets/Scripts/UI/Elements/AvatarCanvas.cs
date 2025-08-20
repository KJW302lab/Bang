using UnityEngine;
using UnityEngine.UI;

public class AvatarCanvas : MonoBehaviour
{
    [SerializeField] private Text   nicknameLabel;
    [SerializeField] private Slider healthBar;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void SetNickname(string nickname)
    {
        nicknameLabel.text = nickname;
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    private void Update()
    {
        transform.forward = _mainCam.transform.forward;
    }
}
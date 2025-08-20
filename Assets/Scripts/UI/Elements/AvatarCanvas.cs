using UnityEngine;
using UnityEngine.UI;

public class AvatarCanvas : MonoBehaviour
{
    [SerializeField] private Text nicknameLabel;

    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void SetNickname(string nickname)
    {
        nicknameLabel.text = nickname;
    }

    private void Update()
    {
        transform.forward = _mainCam.transform.forward;
    }
}
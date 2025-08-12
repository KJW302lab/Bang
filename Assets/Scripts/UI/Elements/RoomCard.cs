using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomCard : MonoBehaviour
{
    public static RoomCard SelectedCard { get; private set; }

    [SerializeField] private Text roomNameLabel;
    [SerializeField] private Text playerCountLabel;

    private Button   _button;
    private RoomInfo _info;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnCardPressed);
    }

    private void OnCardPressed()
    {
        if (_info == null) return;
        if (_info.RemovedFromList) return;
        if (_info.PlayerCount >= _info.MaxPlayers) return;

        SelectedCard = this;
    }

    public string GetRoomName()
    {
        return _info.Name;
    }
    
    public void SetCard(RoomInfo info)
    {
        _info = info;

        roomNameLabel.text = info.Name;
        playerCountLabel.text = $"{info.PlayerCount}/{info.MaxPlayers}";
    }
}
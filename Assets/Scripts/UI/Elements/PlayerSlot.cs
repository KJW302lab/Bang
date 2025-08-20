using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] private Text nicknameLabel;
    [SerializeField] private Text readyLabel;
    
    public Photon.Realtime.Player Player { get; private set; }

    public void Initialize(Photon.Realtime.Player player)
    {
        Player = player;
        
        nicknameLabel.text = player.NickName;
        SetReadyLabel(false);
    }

    public void SetReadyLabel(bool isReady)
    {
        readyLabel.text = isReady ? "READY" : "NOT READY";
    }
}
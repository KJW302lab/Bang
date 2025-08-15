using Photon.Pun;
using UnityEngine;

public class PlayScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        var position = Random.insideUnitSphere * 1.5f;
        position.y = 0;
        PhotonNetwork.Instantiate("Player", position, Quaternion.identity);
    }
}
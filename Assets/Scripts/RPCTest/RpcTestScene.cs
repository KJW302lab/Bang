using Photon.Pun;
using UnityEngine;

public class RpcTestScene : MonoBehaviourPunCallbacks
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
        position.y = 1f;
        PhotonNetwork.Instantiate("TestPlayer", position, Quaternion.identity);
    }
}
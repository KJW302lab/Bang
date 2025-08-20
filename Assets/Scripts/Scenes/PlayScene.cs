using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> spawnPositions;
    
    private void Awake()
    {
        if (NetworkManager.Instance.IsConnected == false) return;

        var localPlayer = NetworkManager.Instance.LocalPlayer;

        int actorNumber = localPlayer.ActorNumber;

        Vector3 position = spawnPositions[actorNumber].position;
        
        PhotonNetwork.Instantiate("Player", position, Quaternion.identity);
    }
}
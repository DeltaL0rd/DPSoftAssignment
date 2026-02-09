using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        // For Host joined
        SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        // For late joiners/ CLients 
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        // this is not a good way to spawn players but this time it works :-P
        // best way to setup a NetworkList for joined players with there custom data like name,id,character id n all and after game start then spawn as there priority
    }

    private void SpawnPlayer(ulong clientId)
    {
        // Only server/host can spawn
        if (!NetworkManager.Singleton.IsServer) return;

        GameObject player = Instantiate(playerPrefab);

        // This line makes it THAT CLIENT'S PLAYER
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        player.transform.position = spawnPoints[NetworkManager.Singleton.ConnectedClients.Count-1].position;

        Debug.Log("Spawned player for Client: " + clientId);
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
    }
}

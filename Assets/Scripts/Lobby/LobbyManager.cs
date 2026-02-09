using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private const int MAX_PLAYERS = 5;
    public static LobbyManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void CreateGame()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApproval;

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("VRGameScene", LoadSceneMode.Single);
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
    }
    
    private void NetworkManager_ConnectionApproval(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERS)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Lobby is full!";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }
}

using UnityEngine;
using UnityEngine.UI;
public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
   
    private void Awake()
    {
        createGameButton.onClick.AddListener(() => { LobbyManager.Instance.CreateGame(); });
        joinGameButton.onClick.AddListener(() => { LobbyManager.Instance.JoinGame(); });
    }
}
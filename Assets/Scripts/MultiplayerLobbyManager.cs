using ParrelSync;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerLobbyManager : MonoBehaviour {
    private void Start() {
        SceneManager.LoadScene(ClonesManager.IsClone() ? "Client" : "Server");
    }
}
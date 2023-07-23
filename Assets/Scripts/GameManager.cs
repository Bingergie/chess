using System;
using Core;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static EventHandler<bool> OnGameStart;
    public static EventHandler<Piece[]> OnBoardUpdate;
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")] 
    public const int Width = 8, Height = 8;

    [SerializeField] private Player player;
    private ChessGame game;

    private void Start() {
        Instance = this;
        // if any other GameManager exists, destroy it
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1) {
            Debug.LogWarning("GameManager.Start: Duplicate found, destroying this");
            Destroy(gameObject);
        }
        
        // subscribe to events
        // game.OnBoardUpdate += GameOnBoardUpdate;
    }
    
    private void OnDestroy() {
        // unsubscribe from events
        // game.OnBoardUpdate -= GameOnBoardUpdate;
    }

    private void Awake() {
        game = new ChessGame();
        game.Init();
        game.OnBoardUpdate += GameOnBoardUpdate;
        OnGameStart?.Invoke(this, true); // todo: change to false when multiplayer is implemented
    }

    public void TryMakeMove(Move move) {
        game.TryMakeMove(player.isWhite, move);
    }

    private void GameOnBoardUpdate(object sender, Piece[] e) {
        Debug.Log("game manager board update");
        OnBoardUpdate?.Invoke(this, e);
    }
}
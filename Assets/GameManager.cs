using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver, Intro, EndOfGame }
    public GameState State { get; private set; }
    public GameObject VictoryText;
    public GameObject GameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        State = newState;
        // Broadcast events, notify systems, etc.
    }
    
    public void gameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        GameOver.SetActive(true);
        State = GameState.GameOver;
    }

    public void endGame()
    {
        Debug.Log("The Player Wins!");
        Time.timeScale = 0f;
        VictoryText.SetActive(true);
        State = GameState.EndOfGame;
    }
}
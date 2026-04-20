using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver, Intro, EndOfGame }
    public GameState State { get; private set; }
    public GameObject VictoryText;
    public GameObject GameOver;
    public GameObject player;
    public GameObject boss;
    public GameObject menu;
    public GameObject UI;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private BossHealth bossHealth;
    private int phase;
    //private BossHealth bossHealth;
    private int score;
    public TextMeshProUGUI menuTitle;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI time;
    private float startTime;




    public void SaveState()
    {
        playerHealth.SaveSnapshot();
        playerMovement.SaveSnapshot();
        bossHealth.SaveSnapshot();
    }

    public void LoadState()
    {
        playerMovement.RestoreSnapshot();
        playerHealth.RestoreSnapshot();
        bossHealth.RestoreBoss();
    }
    
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        bossHealth = boss.GetComponent<BossHealth>();
        SetState(GameState.Playing);

        Instance = this;
        DontDestroyOnLoad(gameObject);
        phase = 1;

    }

    public void SetState(GameState newState)
    {
        State = newState;
        HandleStateChanged(newState);
        // Broadcast events, notify systems, etc.
    }
    
    public void gameOver()
    {
        // Debug.Log("Game Over");
        // Time.timeScale = 0f;
        // GameOver.SetActive(true);
        SetState(GameState.GameOver);
    }

    public void endGame()
    {
        // Debug.Log("The Player Wins!");
        // Time.timeScale = 0f;
        // VictoryText.SetActive(true);
        SetState(GameState.EndOfGame);
    }

    public void BossHurt()
    {
        playerHealth.SpawnPopup("Hurt Boss", Color.green);
    }
    

    public void PhaseShift()
    {
        // Time.timeScale = 0f;
        // menu.SetActive(true);
        // UI.SetActive(false);
        // Cursor.visible = true;
        phase++;
        SetState(GameState.Paused);
    }

    public void GoBack()
    {
        Debug.Log("GoBack() was clicked, restting scene...");
        LoadState();
        Debug.Log("Scene reset, continuing game...");
        phase--;
        Play();
    }

    public void Play()
    {
        Debug.Log("Play() started trying SaveAll()");
        SaveState();
        Debug.Log("SaveAll() completed, continuing game...");
        SetState(GameState.Playing);
    }

    public string TimeFormat(float t)
    {
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        int milliseconds = (int)((t * 1000) % 1000);

        string formatted = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
        
        return formatted;

    }
    

    public void CalcScore()
    {
        score += 1000000;
    }

    public void UpHits()
    {
        if (score > 1000)
        {
            score -= 1000;
            scoreText.text = score.ToString("N0");
        }
    }

    public void UpEnd()
    {
        if (score > 1000)
        {
            score -= 1000;
            scoreText.text = score.ToString("N0");
        }
    }
    
    


    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                menu.SetActive(false);
                UI.SetActive(true);
                Cursor.visible = false;
                startTime = Time.time;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                int prev = phase - 1;
                string round = "Round " + prev;
                float elapsedTime = Time.time - startTime;
                menuTitle.text = round;
                time.text = TimeFormat(elapsedTime);
                float lostHP = playerHealth.MaximumHealth - playerHealth.currentHealth;
                healthText.text = lostHP.ToString("F0");
                CalcScore();
                scoreText.text = score.ToString("N0");
                menu.SetActive(true);
                UI.SetActive(false);
                Cursor.visible = true;
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                Time.timeScale = 0f;
                GameOver.SetActive(true);
                break;

            case GameState.Intro:
                //will implement something
                break;
            case GameState.EndOfGame:
                Debug.Log("The Player Wins!");
                Time.timeScale = 0f;
                VictoryText.SetActive(true);
                break;
        }
    }
}
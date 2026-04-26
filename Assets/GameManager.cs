using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int savedScore;
    public GameObject restartButton;




    public void SaveState()
    {
        playerHealth.SaveSnapshot();
        playerMovement.SaveSnapshot();
        bossHealth.SaveSnapshot();
        savedScore = score;
    }

    public void LoadState()
    {
        playerMovement.RestoreSnapshot();
        playerHealth.RestoreSnapshot();
        bossHealth.RestoreBoss();
        score = savedScore;
    }

    public int GetScore()
    {
        return score;
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
        Play();

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

    public async void endGame()
    {
        // Debug.Log("The Player Wins!");
        // Time.timeScale = 0f;
        // VictoryText.SetActive(true);
        await Task.Delay(2000);
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
    

    public void CalcScore(int round, float hits, float elapsedTime)
    {
        if (round == 1)
        {
            if (100 - hits < 1)
                return;
            if (elapsedTime < 30)
                score += (int)((100 - hits) * 1.5);
            else
                score += (int)(100 - hits);
        }
        else if (round == 2)
        {
            if (150 - hits < 1)
                return;
            if (elapsedTime < 30)
                score += (int)((150 - hits) * 1.5);
            else
                score += (int)(150 - hits);
        }
        else
        {
            if (200 - hits < 1)
                return;
            if (elapsedTime < 30)
                score += (int)((200 - hits) * 1.5);
            else
                score += (int)(200 - hits);
        }
    }

    public void UpHits()
    {
        if (score > 20)
        {
            score -= 20;
            playerHealth.increaseHealth(10);
            scoreText.text = score.ToString("N0");
        }
    }

    public void UpEnd()
    {
        if (score > 30)
        {
            score -= 30;
            playerMovement.increaseEndurance(1);
            scoreText.text = score.ToString("N0");
        }
    }
    
    


    private void HandleStateChanged(GameState state)
    {
        int prev ;
        string round ;
        float elapsedTime ;
        float lostHP;
        switch (state)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                menu.SetActive(false);
                UI.SetActive(true);
                //Cursor.visible = false;
                startTime = Time.time;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                prev = phase - 1;
                round = "Round " + prev;
                elapsedTime = Time.time - startTime;
                menuTitle.text = round;
                time.text = TimeFormat(elapsedTime);
                lostHP = playerHealth.MaximumHealth - playerHealth.currentHealth;
                healthText.text = lostHP.ToString("F0");
                CalcScore(prev, lostHP, elapsedTime);
                scoreText.text = score.ToString("N0");
                menu.SetActive(true);
                UI.SetActive(false);
                Cursor.visible = true;
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                Time.timeScale = 0f;
                GameOver.SetActive(true);
                restartButton.SetActive(true);
                Cursor.visible = true;
                break;

            case GameState.Intro:
                //will implement something
                break;
            case GameState.EndOfGame:
                
                Debug.Log("The Player Wins!");
                prev = phase - 1;
                elapsedTime = Time.time - startTime;
                lostHP = playerHealth.MaximumHealth - playerHealth.currentHealth;
                CalcScore(prev, lostHP, elapsedTime);
                Cursor.visible = true;
                //Time.timeScale = 0f;
                SceneManager.LoadScene(2);
                //VictoryText.SetActive(true);
                break;
        }
    }
}
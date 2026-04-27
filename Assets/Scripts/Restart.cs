using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Restart: MonoBehaviour
{
    public string initials;
    public int highScore;
    public GameObject ScoreText;
    public GameObject TextBox;
    public TextMeshProUGUI ScoreBox;
    public int score;
    public GameObject WarningText;
    private InputAction escapeAction;
    private bool warningshown = false;
    private float elapsedTime = 0f;
    private float startTime = 0f;

    
    void Start()
    {
        initials = PlayerPrefs.GetString("initials");
        highScore = PlayerPrefs.GetInt("HighScore");
        score = GameManager.Instance.GetScore();
        ScoreBox.text =  score.ToString();
        if (score > highScore)
        {
            highScore = score;
            ScoreText.SetActive(true);
            TextBox.SetActive(true);
        }
    }

    
    void Awake() 
    {
        escapeAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        escapeAction.Enable();
    }

    
    void Update()
    {
        if (warningshown)
        {
            if (Time.time - startTime>2)
            {
                WarningText.SetActive(false);
                warningshown = false;
            }
        }
        if (escapeAction.WasPressedThisFrame())
        {
            if (!warningshown)
            {
                WarningText.SetActive(true);
                warningshown = true;
                startTime = Time.time;
            }
            else
            {
                RestartGame();
            }
        }
    }
    

    public void updateInitials(string newInitials)
    {
        initials = newInitials;
    }
    public void RestartGame()
    {
        PlayerPrefs.SetString("initials", initials);
        PlayerPrefs.SetInt("HighScore", highScore);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}

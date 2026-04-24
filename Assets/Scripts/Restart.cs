using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart: MonoBehaviour
{
    public string initials;
    public int highScore;
    public GameObject ScoreText;
    public GameObject TextBox;
    public TextMeshProUGUI ScoreBox;
    public int score;
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

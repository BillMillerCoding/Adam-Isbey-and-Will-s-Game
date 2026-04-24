using TMPro;
using UnityEngine;

public class TitleStats: MonoBehaviour
{
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI initials;

    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        initials.text = PlayerPrefs.GetString("initials").ToUpper();
    }
}

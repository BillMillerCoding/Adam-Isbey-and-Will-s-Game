using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame: MonoBehaviour
{
    public GameObject helpMenu;

    public void showHelp()
    {
        helpMenu.SetActive(true);
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}

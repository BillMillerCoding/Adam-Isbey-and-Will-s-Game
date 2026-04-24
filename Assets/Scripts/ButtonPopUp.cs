using UnityEngine;
using UnityEngine.UI;

public class ButtonPopupSpawner : MonoBehaviour
{
    public GameObject popupPrefab;
    public Canvas canvas;
    public string popupText = "Click!";
    public Color popupColor = Color.white;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpawnPopup);
    }

    void SpawnPopup()
    {
        GameObject popup = Instantiate(popupPrefab, canvas.transform);

        // UI buttons already have a screen position
        Vector2 screenPos = transform.position;

        popup.GetComponent<PopupFollow>().InitializeFromScreen(screenPos, popupText, popupColor);
    }
}
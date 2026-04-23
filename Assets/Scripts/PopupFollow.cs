using UnityEngine;
using TMPro;

public class PopupFollow : MonoBehaviour
{
    public Camera cam;

    [Header("Animation Settings")]
    [SerializeField] float floatSpeed = 10f;
    [SerializeField] float lifetime = 1.0f;
    [SerializeField] float fadeDuration = 0.5f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI text;
    private RectTransform rect;


    void Awake()
    {
        // Grab the text component on THIS object
        text = GetComponent<TextMeshProUGUI>();

        // Add a CanvasGroup if missing (for fading)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// Initializes the popup with a world position, message text, and color.
    /// </summary>
    public void Initialize(Vector3 worldPosition, string message, Color color)
    {
        // Set text and color
        text.text = message;
        text.color = color;

        // Convert world → screen once
        Vector3 screenPos = cam.WorldToScreenPoint(worldPosition);
        transform.position = screenPos;
    }
    
    public void InitializeFromScreen(Vector3 screenPosition, string message, Color color)
    {
        text.text = message;
        text.color = color;

        transform.position = screenPosition; // no conversion needed
    }


    void Update()
    {
        timer += Time.unscaledDeltaTime;
        
        rect.anchoredPosition += Vector2.up * (floatSpeed * Time.unscaledDeltaTime);
        

        if (timer > lifetime - fadeDuration)
        {
            float t = 1 - ((timer - (lifetime - fadeDuration)) / fadeDuration);
            canvasGroup.alpha = Mathf.Clamp01(t);
        }

        if (timer >= lifetime)
            Destroy(gameObject);
    }

}
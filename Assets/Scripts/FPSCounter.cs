using UnityEngine;
using UnityEngine.UI;

// Optional FPS counter for performance monitoring
// Attach to any GameObject to display FPS in top-right corner
public class FPSCounter : MonoBehaviour
{
    [Header("Settings")]
    public bool showFPS = true;
    public float updateInterval = 0.5f;

    private Text fpsText;
    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;

    void Start()
    {
        if (!showFPS) return;

        // Create UI for FPS display
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("FPSCounter: No canvas found. FPS will not be displayed.");
            return;
        }

        GameObject fpsGO = new GameObject("FPSCounter");
        fpsGO.transform.SetParent(canvas.transform, false);
        
        fpsText = fpsGO.AddComponent<Text>();
        fpsText.text = "FPS: --";
        fpsText.fontSize = 24;
        fpsText.alignment = TextAnchor.UpperRight;
        fpsText.color = new Color(0.3f, 1f, 0.3f, 0.8f);
        fpsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        fpsText.fontStyle = FontStyle.Bold;

        // Add outline
        Outline outline = fpsGO.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.8f);
        outline.effectDistance = new Vector2(1, -1);

        RectTransform rect = fpsGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-10f, -10f);
        rect.sizeDelta = new Vector2(150f, 40f);

        timeLeft = updateInterval;
    }

    void Update()
    {
        if (!showFPS || fpsText == null) return;

        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0f)
        {
            float fps = accum / frames;
            fpsText.text = $"FPS: {fps:F0}";

            // Color code based on performance
            if (fps >= 55f)
                fpsText.color = new Color(0.3f, 1f, 0.3f, 0.8f); // Green
            else if (fps >= 30f)
                fpsText.color = new Color(1f, 0.9f, 0.3f, 0.8f); // Yellow
            else
                fpsText.color = new Color(1f, 0.3f, 0.3f, 0.8f); // Red

            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}

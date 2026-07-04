using UnityEngine;
using UnityEngine.UI;

// Enhanced SceneSetup with modern UI and game information display
[DefaultExecutionOrder(-100)]
public class SceneSetup : MonoBehaviour
{
    private Text scoreText;
    private Text comboText;
    private Text colorText;
    private BoxPlacer boxPlacer;

    void Awake()
    {
        SetupScene();
    }

    void SetupScene()
    {
        // ── 1. Camera ──────────────────────────────────────────────
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camGO = new GameObject("Main Camera");
            cam = camGO.AddComponent<Camera>();
            camGO.tag = "MainCamera";
        }
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = new Color(0.08f, 0.08f, 0.12f);
        cam.clearFlags = CameraClearFlags.SolidColor;

        // ── 2. Grid Background ─────────────────────────────────────
        GridBackground existingGrid = Object.FindFirstObjectByType<GridBackground>();
        if (existingGrid == null)
        {
            GameObject gridGO = new GameObject("GridBackground");
            GridBackground grid = gridGO.AddComponent<GridBackground>();
            grid.gridSize = 1.0f;
            grid.gridWidth = 20;
            grid.gridHeight = 16;
        }

        // ── 3. BoxPlacer Manager ───────────────────────────────────
        BoxPlacer existingPlacer = Object.FindFirstObjectByType<BoxPlacer>();
        if (existingPlacer == null)
        {
            GameObject manager = new GameObject("BoxPlacerManager");
            boxPlacer = manager.AddComponent<BoxPlacer>();
            boxPlacer.boxSize = 1.0f;
        }
        else
        {
            boxPlacer = existingPlacer;
        }

        // ── 4. Canvas + Enhanced UI ────────────────────────────────
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.sortingOrder = -5;
        canvas.planeDistance = 1f;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Background
        GameObject bgPanel = new GameObject("BackgroundPanel");
        bgPanel.transform.SetParent(canvasGO.transform, false);
        Image bgImage = bgPanel.AddComponent<Image>();
        bgImage.color = new Color(0.08f, 0.08f, 0.12f, 1f);
        RectTransform bgRect = bgPanel.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Title
        CreateText(canvasGO.transform, "TitleText", "🎨 COLOR BOX CREATOR 🎨",
            new Vector2(0.5f, 0.95f), new Vector2(0.5f, 0.95f),
            new Vector2(0f, -20f), new Vector2(0f, -20f),
            60, TextAnchor.MiddleCenter, new Color(1f, 0.9f, 0.3f));

        // Score Display (Top Left)
        scoreText = CreateText(canvasGO.transform, "ScoreText", "SCORE: 0",
            new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(20f, -80f), new Vector2(300f, -80f),
            40, TextAnchor.MiddleLeft, Color.white);

        // Combo Display (Top Left, below score)
        comboText = CreateText(canvasGO.transform, "ComboText", "COMBO: 0x",
            new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(20f, -130f), new Vector2(300f, -130f),
            32, TextAnchor.MiddleLeft, new Color(1f, 0.5f, 0.2f));

        // Current Color Display (Top Right)
        colorText = CreateText(canvasGO.transform, "ColorText", "COLOR: 1 (Red)",
            new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(-300f, -80f), new Vector2(-20f, -80f),
            36, TextAnchor.MiddleRight, new Color(1f, 0.3f, 0.3f));

        // Instructions Panel (Bottom)
        GameObject instructionsPanel = new GameObject("InstructionsPanel");
        instructionsPanel.transform.SetParent(canvasGO.transform, false);
        Image instrBg = instructionsPanel.AddComponent<Image>();
        instrBg.color = new Color(0.1f, 0.1f, 0.15f, 0.9f);
        RectTransform instrRect = instructionsPanel.GetComponent<RectTransform>();
        instrRect.anchorMin = new Vector2(0.05f, 0.02f);
        instrRect.anchorMax = new Vector2(0.95f, 0.15f);
        instrRect.offsetMin = Vector2.zero;
        instrRect.offsetMax = Vector2.zero;

        // Instructions Text
        string instructions = "🖱️ CLICK to place/remove boxes  |  " +
                            "1-8 to change colors  |  " +
                            "R for Rainbow Mode  |  " +
                            "C to Clear All\n" +
                            "💡 Create lines (3+) or 2x2 squares for bonus points!";
        
        CreateText(instructionsPanel.transform, "InstructionsText", instructions,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(-450f, -30f), new Vector2(450f, 30f),
            24, TextAnchor.MiddleCenter, new Color(0.7f, 0.9f, 1f));

        // Pattern Guide (Right side)
        GameObject guidePanel = new GameObject("GuidePanel");
        guidePanel.transform.SetParent(canvasGO.transform, false);
        Image guideBg = guidePanel.AddComponent<Image>();
        guideBg.color = new Color(0.1f, 0.1f, 0.15f, 0.85f);
        RectTransform guideRect = guidePanel.GetComponent<RectTransform>();
        guideRect.anchorMin = new Vector2(0.82f, 0.25f);
        guideRect.anchorMax = new Vector2(0.98f, 0.75f);
        guideRect.offsetMin = Vector2.zero;
        guideRect.offsetMax = Vector2.zero;

        string guideText = "SCORING:\n\n" +
                          "Place Box: 10\n\n" +
                          "3+ Line: 60+\n\n" +
                          "2x2 Square: 50\n\n" +
                          "Combo: x2, x3...\n\n" +
                          "(Place quickly!)";
        
        CreateText(guidePanel.transform, "GuideText", guideText,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(-80f, -150f), new Vector2(80f, 150f),
            20, TextAnchor.UpperCenter, new Color(0.8f, 0.8f, 1f));

        // ── 5. Optional FPS Counter ────────────────────────────────
        // Uncomment to show FPS during presentation
        // GameObject fpsGO = new GameObject("FPSCounter");
        // fpsGO.AddComponent<FPSCounter>();

        // Subscribe to events
        boxPlacer.OnScoreChanged += UpdateScore;
        boxPlacer.OnComboChanged += UpdateCombo;
        boxPlacer.OnColorChanged += UpdateColor;

        Debug.Log("Enhanced Scene Setup Complete! Ready to create art!");
    }

    Text CreateText(Transform parent, string name, string content,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax,
        int fontSize, TextAnchor alignment, Color color)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(parent, false);
        Text txt = textGO.AddComponent<Text>();
        txt.text = content;
        txt.fontSize = fontSize;
        txt.alignment = alignment;
        txt.color = color;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontStyle = FontStyle.Bold;

        // Add outline for better readability
        Outline outline = textGO.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.8f);
        outline.effectDistance = new Vector2(2, -2);

        RectTransform txtRect = textGO.GetComponent<RectTransform>();
        txtRect.anchorMin = anchorMin;
        txtRect.anchorMax = anchorMax;
        txtRect.offsetMin = offsetMin;
        txtRect.offsetMax = offsetMax;

        return txt;
    }

    void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score}";
        }
    }

    void UpdateCombo(int combo)
    {
        if (comboText != null)
        {
            if (combo > 0)
            {
                comboText.text = $"COMBO: {combo}x 🔥";
                comboText.color = Color.Lerp(new Color(1f, 0.5f, 0.2f), Color.red, combo / 10f);
            }
            else
            {
                comboText.text = "COMBO: 0x";
                comboText.color = new Color(1f, 0.5f, 0.2f);
            }
        }
    }

    void UpdateColor(int colorIndex)
    {
        if (colorText != null && boxPlacer != null)
        {
            string[] colorNames = { "Red", "Green", "Blue", "Yellow", "Orange", "Purple", "Cyan", "Pink" };
            string colorName = colorIndex < colorNames.Length ? colorNames[colorIndex] : "Color";
            colorText.text = $"COLOR: {colorIndex + 1} ({colorName})";
            
            if (colorIndex < boxPlacer.colorPalette.Length)
            {
                colorText.color = boxPlacer.colorPalette[colorIndex];
            }
        }
    }

    void OnDestroy()
    {
        if (boxPlacer != null)
        {
            boxPlacer.OnScoreChanged -= UpdateScore;
            boxPlacer.OnComboChanged -= UpdateCombo;
            boxPlacer.OnColorChanged -= UpdateColor;
        }
    }
}

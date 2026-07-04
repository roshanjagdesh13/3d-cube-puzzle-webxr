using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Enhanced BoxPlacer with colors, patterns, scoring, and visual effects
public class BoxPlacer : MonoBehaviour
{
    [Header("Grid Settings")]
    public float boxSize = 1.0f;
    
    [Header("Color Palette")]
    public Color[] colorPalette = new Color[]
    {
        new Color(1.0f, 0.3f, 0.3f, 0.9f),  // Red
        new Color(0.3f, 1.0f, 0.3f, 0.9f),  // Green
        new Color(0.3f, 0.5f, 1.0f, 0.9f),  // Blue
        new Color(1.0f, 0.9f, 0.2f, 0.9f),  // Yellow
        new Color(1.0f, 0.5f, 0.0f, 0.9f),  // Orange
        new Color(0.8f, 0.2f, 1.0f, 0.9f),  // Purple
        new Color(0.2f, 0.9f, 0.9f, 0.9f),  // Cyan
        new Color(1.0f, 0.4f, 0.7f, 0.9f)   // Pink
    };

    [Header("Game Settings")]
    public bool rainbowMode = false;
    public float animationSpeed = 5f;

    // Internal state
    private Dictionary<Vector2Int, BoxData> placedBoxes = new Dictionary<Vector2Int, BoxData>();
    private Camera mainCamera;
    private int currentColorIndex = 0;
    private int score = 0;
    private int combo = 0;
    private float comboTimer = 0f;
    private const float comboTimeout = 2f;

    // Events for UI updates
    public System.Action<int> OnScoreChanged;
    public System.Action<int> OnComboChanged;
    public System.Action<int> OnColorChanged;

    private class BoxData
    {
        public GameObject gameObject;
        public Color color;
        public int colorIndex;
        public float spawnTime;
    }

    void Start()
    {
        mainCamera = Camera.main;
        OnColorChanged?.Invoke(currentColorIndex);
        Debug.Log("Enhanced BoxPlacer ready! Press 1-8 to change colors, R for rainbow mode!");
    }

    void Update()
    {
        HandleInput();
        UpdateComboTimer();
        
        if (rainbowMode)
        {
            AnimateRainbowBoxes();
        }
    }

    void HandleInput()
    {
        // Mouse click to place/remove boxes
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        // Number keys to change color
        for (int i = 0; i < Mathf.Min(8, colorPalette.Length); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentColorIndex = i;
                OnColorChanged?.Invoke(currentColorIndex);
                Debug.Log($"Color changed to {i + 1}");
            }
        }

        // R key for rainbow mode
        if (Input.GetKeyDown(KeyCode.R))
        {
            rainbowMode = !rainbowMode;
            Debug.Log($"Rainbow mode: {rainbowMode}");
        }

        // C key to clear all
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAll();
        }
    }

    void HandleClick(Vector3 mouseScreenPos)
    {
        if (mainCamera == null) return;

        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;

        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(worldPos.x / boxSize),
            Mathf.RoundToInt(worldPos.y / boxSize)
        );

        if (placedBoxes.ContainsKey(gridPos))
        {
            RemoveBox(gridPos);
        }
        else
        {
            Vector3 snapPos = new Vector3(gridPos.x * boxSize, gridPos.y * boxSize, 0f);
            PlaceBox(gridPos, snapPos);
        }
    }

    void PlaceBox(Vector2Int gridPos, Vector3 position)
    {
        Color boxColor = colorPalette[currentColorIndex];
        GameObject box = CreateBox(position, boxColor);
        
        BoxData boxData = new BoxData
        {
            gameObject = box,
            color = boxColor,
            colorIndex = currentColorIndex,
            spawnTime = Time.time
        };
        
        placedBoxes[gridPos] = boxData;

        // Animate spawn
        StartCoroutine(AnimateBoxSpawn(box));

        // Check for patterns and award points
        CheckPatterns(gridPos);
        
        // Extend combo
        comboTimer = comboTimeout;
        combo++;
        OnComboChanged?.Invoke(combo);
    }

    void RemoveBox(Vector2Int gridPos)
    {
        if (placedBoxes.TryGetValue(gridPos, out BoxData boxData))
        {
            StartCoroutine(AnimateBoxRemoval(boxData.gameObject, gridPos));
            
            // Reset combo
            combo = 0;
            OnComboChanged?.Invoke(combo);
        }
    }

    GameObject CreateBox(Vector3 position, Color color)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Quad);
        box.transform.position = position;
        box.transform.localScale = new Vector3(boxSize * 0.9f, boxSize * 0.9f, 1f);
        box.name = "ColorBox";

        Destroy(box.GetComponent<MeshCollider>());

        Renderer rend = box.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = color;
            rend.material = mat;
        }

        // Add a subtle border effect
        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Quad);
        border.transform.SetParent(box.transform);
        border.transform.localPosition = Vector3.zero;
        border.transform.localScale = Vector3.one * 1.1f;
        Destroy(border.GetComponent<MeshCollider>());
        
        Renderer borderRend = border.GetComponent<Renderer>();
        if (borderRend != null)
        {
            Material borderMat = new Material(Shader.Find("Sprites/Default"));
            borderMat.color = new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, 0.8f);
            borderRend.material = borderMat;
            borderRend.sortingOrder = -1;
        }

        return box;
    }

    void CheckPatterns(Vector2Int newPos)
    {
        int points = 10; // Base points
        
        // Check for horizontal line (3+ same color)
        int horizontalCount = CountSameColorInDirection(newPos, Vector2Int.right) + 
                             CountSameColorInDirection(newPos, Vector2Int.left) + 1;
        
        // Check for vertical line (3+ same color)
        int verticalCount = CountSameColorInDirection(newPos, Vector2Int.up) + 
                           CountSameColorInDirection(newPos, Vector2Int.down) + 1;
        
        // Check for 2x2 square
        bool hasSquare = CheckForSquare(newPos);
        
        // Award points based on patterns
        if (horizontalCount >= 3)
        {
            points += horizontalCount * 20;
            Debug.Log($"Horizontal line of {horizontalCount}! +{horizontalCount * 20} points");
        }
        
        if (verticalCount >= 3)
        {
            points += verticalCount * 20;
            Debug.Log($"Vertical line of {verticalCount}! +{verticalCount * 20} points");
        }
        
        if (hasSquare)
        {
            points += 50;
            Debug.Log("2x2 Square! +50 points");
        }
        
        // Apply combo multiplier
        int finalPoints = points * Mathf.Max(1, combo / 3);
        score += finalPoints;
        OnScoreChanged?.Invoke(score);
        
        if (finalPoints > 10)
        {
            CreateScorePopup(newPos, finalPoints);
        }
    }

    int CountSameColorInDirection(Vector2Int start, Vector2Int direction)
    {
        if (!placedBoxes.ContainsKey(start)) return 0;
        
        int colorIndex = placedBoxes[start].colorIndex;
        int count = 0;
        Vector2Int current = start + direction;
        
        while (placedBoxes.ContainsKey(current) && placedBoxes[current].colorIndex == colorIndex)
        {
            count++;
            current += direction;
        }
        
        return count;
    }

    bool CheckForSquare(Vector2Int pos)
    {
        if (!placedBoxes.ContainsKey(pos)) return false;
        
        int colorIndex = placedBoxes[pos].colorIndex;
        Vector2Int[] offsets = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        };
        
        foreach (var offset in offsets)
        {
            Vector2Int checkPos = pos + offset;
            if (!placedBoxes.ContainsKey(checkPos) || placedBoxes[checkPos].colorIndex != colorIndex)
                return false;
        }
        
        return true;
    }

    void CreateScorePopup(Vector2Int gridPos, int points)
    {
        Vector3 worldPos = new Vector3(gridPos.x * boxSize, gridPos.y * boxSize, -1f);
        GameObject popup = new GameObject("ScorePopup");
        popup.transform.position = worldPos;
        
        TextMesh textMesh = popup.AddComponent<TextMesh>();
        textMesh.text = $"+{points}";
        textMesh.fontSize = 40;
        textMesh.color = Color.yellow;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        
        StartCoroutine(AnimateScorePopup(popup));
    }

    void UpdateComboTimer()
    {
        if (combo > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                combo = 0;
                OnComboChanged?.Invoke(combo);
            }
        }
    }

    void AnimateRainbowBoxes()
    {
        float hue = (Time.time * 0.1f) % 1f;
        
        foreach (var kvp in placedBoxes)
        {
            if (kvp.Value.gameObject != null)
            {
                Renderer rend = kvp.Value.gameObject.GetComponent<Renderer>();
                if (rend != null)
                {
                    float boxHue = (hue + kvp.Key.x * 0.1f + kvp.Key.y * 0.1f) % 1f;
                    rend.material.color = Color.HSVToRGB(boxHue, 0.8f, 1f);
                }
            }
        }
    }

    void ClearAll()
    {
        foreach (var kvp in placedBoxes.ToList())
        {
            if (kvp.Value.gameObject != null)
            {
                Destroy(kvp.Value.gameObject);
            }
        }
        placedBoxes.Clear();
        score = 0;
        combo = 0;
        OnScoreChanged?.Invoke(score);
        OnComboChanged?.Invoke(combo);
        Debug.Log("All boxes cleared!");
    }

    System.Collections.IEnumerator AnimateBoxSpawn(GameObject box)
    {
        Vector3 originalScale = box.transform.localScale;
        box.transform.localScale = Vector3.zero;
        
        float elapsed = 0f;
        float duration = 0.2f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease out
            box.transform.localScale = originalScale * scale;
            yield return null;
        }
        
        box.transform.localScale = originalScale;
    }

    System.Collections.IEnumerator AnimateBoxRemoval(GameObject box, Vector2Int gridPos)
    {
        Vector3 originalScale = box.transform.localScale;
        
        float elapsed = 0f;
        float duration = 0.15f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            box.transform.localScale = originalScale * (1f - t);
            box.transform.Rotate(0, 0, 720f * Time.deltaTime);
            yield return null;
        }
        
        Destroy(box);
        placedBoxes.Remove(gridPos);
    }

    System.Collections.IEnumerator AnimateScorePopup(GameObject popup)
    {
        Vector3 startPos = popup.transform.position;
        float elapsed = 0f;
        float duration = 1f;
        
        TextMesh textMesh = popup.GetComponent<TextMesh>();
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            popup.transform.position = startPos + Vector3.up * t * 2f;
            
            Color color = textMesh.color;
            color.a = 1f - t;
            textMesh.color = color;
            
            yield return null;
        }
        
        Destroy(popup);
    }

    public int GetScore() => score;
    public int GetCombo() => combo;
    public int GetBoxCount() => placedBoxes.Count;
}

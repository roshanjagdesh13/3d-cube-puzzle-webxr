using UnityEngine;

// Creates a subtle grid background for better visual reference
[DefaultExecutionOrder(-50)]
public class GridBackground : MonoBehaviour
{
    [Header("Grid Settings")]
    public float gridSize = 1.0f;
    public int gridWidth = 20;
    public int gridHeight = 20;
    public Color gridColor = new Color(0.15f, 0.15f, 0.2f, 0.5f);
    public float lineWidth = 0.02f;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        GameObject gridParent = new GameObject("GridLines");
        gridParent.transform.SetParent(transform);

        // Create vertical lines
        for (int x = -gridWidth / 2; x <= gridWidth / 2; x++)
        {
            CreateLine(
                new Vector3(x * gridSize, -gridHeight / 2 * gridSize, 0.5f),
                new Vector3(x * gridSize, gridHeight / 2 * gridSize, 0.5f),
                gridParent.transform
            );
        }

        // Create horizontal lines
        for (int y = -gridHeight / 2; y <= gridHeight / 2; y++)
        {
            CreateLine(
                new Vector3(-gridWidth / 2 * gridSize, y * gridSize, 0.5f),
                new Vector3(gridWidth / 2 * gridSize, y * gridSize, 0.5f),
                gridParent.transform
            );
        }

        Debug.Log("Grid background created!");
    }

    void CreateLine(Vector3 start, Vector3 end, Transform parent)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(parent);

        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = gridColor;
        lr.endColor = gridColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.sortingOrder = -10; // Behind everything
        lr.useWorldSpace = true;

        // Disable shadows
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
    }
}

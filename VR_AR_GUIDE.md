# 🥽 VR/AR Implementation Guide

Complete guide for adapting the 3D Color Cube Puzzle to Virtual Reality and Augmented Reality platforms.

---

## Table of Contents

1. [Overview](#overview)
2. [Unity VR Implementation](#unity-vr-implementation)
3. [Unity AR Implementation](#unity-ar-implementation)
4. [WebXR Implementation](#webxr-implementation)
5. [Platform Comparison](#platform-comparison)
6. [Best Practices](#best-practices)

---

## Overview

### Why This Project is VR/AR Ready

✅ **3D-Native Architecture**
- Already uses 3D coordinates (x, y, z)
- Rotation matrices in place
- Depth sorting implemented
- Ray-casting for interaction

✅ **Modular Design**
- Input layer separated from logic
- Easy to swap mouse → controller
- Platform-agnostic game rules

✅ **Proven Mechanics**
- Working prototype validates concept
- User interaction tested
- Visual feedback polished

---

## Unity VR Implementation

### Step 1: Setup Unity Project

#### Required Unity Version
- Unity 2021.3 LTS or newer
- Universal Render Pipeline (URP) recommended

#### Required Packages
```
Window → Package Manager:
- XR Interaction Toolkit
- XR Plugin Management
- Oculus XR Plugin (for Quest)
  OR
- OpenXR Plugin (for cross-platform)
```

### Step 2: Create Cube System

#### CubeCell.cs
```csharp
using UnityEngine;

public class CubeCell : MonoBehaviour
{
    public Vector3Int gridPosition;
    public int colorIndex = -1;
    public Material material;
    
    private static readonly Color[] colorPalette = new Color[]
    {
        new Color(1.0f, 0.3f, 0.3f), // Red
        new Color(0.3f, 1.0f, 0.3f), // Green
        new Color(0.3f, 0.5f, 1.0f), // Blue
        new Color(1.0f, 0.9f, 0.2f), // Yellow
        new Color(1.0f, 0.5f, 0.0f), // Orange
        new Color(0.8f, 0.2f, 1.0f), // Purple
        new Color(0.2f, 0.9f, 0.9f), // Cyan
        new Color(1.0f, 0.4f, 0.7f)  // Pink
    };
    
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    
    public void SetColor(int index)
    {
        if (index >= 0 && index < colorPalette.Length)
        {
            colorIndex = index;
            material.color = colorPalette[index];
        }
    }
    
    public void ClearColor()
    {
        colorIndex = -1;
        material.color = new Color(0.2f, 0.2f, 0.3f, 0.3f);
    }
}
```

#### CubeManager.cs
```csharp
using UnityEngine;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour
{
    [Header("Cube Settings")]
    public int cubeSize = 5;
    public float cellSize = 0.1f;
    public GameObject cellPrefab;
    
    [Header("Game State")]
    public int currentColorIndex = 0;
    public int score = 0;
    public int moves = 0;
    
    private Dictionary<Vector3Int, CubeCell> cells = new Dictionary<Vector3Int, CubeCell>();
    
    void Start()
    {
        GenerateCube();
    }
    
    void GenerateCube()
    {
        for (int x = 0; x < cubeSize; x++)
        {
            for (int y = 0; y < cubeSize; y++)
            {
                for (int z = 0; z < cubeSize; z++)
                {
                    // Only create surface cells
                    if (x == 0 || x == cubeSize - 1 ||
                        y == 0 || y == cubeSize - 1 ||
                        z == 0 || z == cubeSize - 1)
                    {
                        Vector3Int gridPos = new Vector3Int(x, y, z);
                        Vector3 worldPos = GridToWorld(gridPos);
                        
                        GameObject cellObj = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                        CubeCell cell = cellObj.AddComponent<CubeCell>();
                        cell.gridPosition = gridPos;
                        cell.ClearColor();
                        
                        cells[gridPos] = cell;
                    }
                }
            }
        }
        
        // Center the cube
        transform.position = -new Vector3(cubeSize * cellSize / 2, cubeSize * cellSize / 2, cubeSize * cellSize / 2);
    }
    
    Vector3 GridToWorld(Vector3Int gridPos)
    {
        return new Vector3(
            gridPos.x * cellSize,
            gridPos.y * cellSize,
            gridPos.z * cellSize
        );
    }
    
    public void PlaceColor(CubeCell cell)
    {
        if (cell.colorIndex == -1)
        {
            cell.SetColor(currentColorIndex);
            score += 10;
            moves++;
            UpdateUI();
        }
    }
    
    public void RemoveColor(CubeCell cell)
    {
        if (cell.colorIndex != -1)
        {
            cell.ClearColor();
            moves++;
            UpdateUI();
        }
    }
    
    void UpdateUI()
    {
        // Update VR UI panels
        // (Implementation depends on UI system)
    }
}
```

### Step 3: VR Interaction

#### VRCubeInteraction.cs
```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRCubeInteraction : MonoBehaviour
{
    [Header("VR References")]
    public XRRayInteractor rightHandRay;
    public XRRayInteractor leftHandRay;
    
    [Header("Game References")]
    public CubeManager cubeManager;
    
    [Header("Input Actions")]
    public XRController rightController;
    public XRController leftController;
    
    void Update()
    {
        HandleRightHand();
        HandleLeftHand();
    }
    
    void HandleRightHand()
    {
        if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            CubeCell cell = hit.collider.GetComponent<CubeCell>();
            
            if (cell != null)
            {
                // Trigger pressed = place color
                if (rightController.selectInteractionState.activatedThisFrame)
                {
                    cubeManager.PlaceColor(cell);
                }
                
                // Grip pressed = remove color
                if (rightController.activateInteractionState.activatedThisFrame)
                {
                    cubeManager.RemoveColor(cell);
                }
            }
        }
    }
    
    void HandleLeftHand()
    {
        // Color selection with left hand buttons
        if (leftController.selectInteractionState.activatedThisFrame)
        {
            cubeManager.currentColorIndex = (cubeManager.currentColorIndex + 1) % 8;
        }
    }
}
```

### Step 4: XR Rig Setup

1. **Create XR Rig**
   ```
   Hierarchy:
   └── XR Origin
       ├── Camera Offset
       │   ├── Main Camera
       │   ├── LeftHand Controller
       │   │   └── Ray Interactor
       │   └── RightHand Controller
       │       └── Ray Interactor
       └── Locomotion System
   ```

2. **Configure Controllers**
   - Add `XR Ray Interactor` component
   - Add `Line Renderer` for visual ray
   - Set interaction layer mask

3. **Add Cube**
   - Place `CubeManager` in scene
   - Position at comfortable VR distance (1-2 meters)
   - Add `VRCubeInteraction` component

### Step 5: Build for Quest

```
File → Build Settings:
- Platform: Android
- Texture Compression: ASTC
- Minimum API Level: Android 10.0

XR Plugin Management:
- ✓ Oculus

Player Settings:
- Color Space: Linear
- Graphics API: OpenGLES3
- Multithreaded Rendering: ✓
```

---

## Unity AR Implementation

### Step 1: Setup AR Foundation

#### Required Packages
```
- AR Foundation
- ARCore XR Plugin (Android)
- ARKit XR Plugin (iOS)
```

### Step 2: AR Cube Placement

#### ARCubePlacer.cs
```csharp
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARCubePlacer : MonoBehaviour
{
    [Header("AR References")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    
    [Header("Prefabs")]
    public GameObject cubePrefab;
    
    private GameObject spawnedCube;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    void Update()
    {
        if (Input.touchCount == 0) return;
        
        Touch touch = Input.GetTouch(0);
        
        // Place cube on tap
        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                
                if (spawnedCube == null)
                {
                    // First placement
                    spawnedCube = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    // Move existing cube
                    spawnedCube.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}
```

#### ARCubeInteraction.cs
```csharp
using UnityEngine;

public class ARCubeInteraction : MonoBehaviour
{
    public CubeManager cubeManager;
    public Camera arCamera;
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    CubeCell cell = hit.collider.GetComponent<CubeCell>();
                    
                    if (cell != null)
                    {
                        cubeManager.PlaceColor(cell);
                    }
                }
            }
        }
    }
}
```

### Step 3: AR Scene Setup

1. **Create AR Session**
   ```
   Hierarchy:
   ├── AR Session
   ├── AR Session Origin
   │   └── AR Camera
   ├── AR Plane Manager
   └── AR Raycast Manager
   ```

2. **Configure Plane Detection**
   ```csharp
   AR Plane Manager:
   - Detection Mode: Horizontal
   - Plane Prefab: (visual indicator)
   ```

3. **Add UI Overlay**
   - Canvas with Camera overlay
   - Instructions text
   - Color picker buttons
   - Score display

### Step 4: Build for Mobile

**Android (ARCore):**
```
Build Settings:
- Platform: Android
- Minimum API: Android 7.0

Player Settings:
- Package Name: com.yourcompany.cubepuzzle
- ARCore: Required

XR Plugin Management:
- ✓ ARCore
```

**iOS (ARKit):**
```
Build Settings:
- Platform: iOS
- Minimum iOS: 11.0

Player Settings:
- Bundle Identifier: com.yourcompany.cubepuzzle
- Camera Usage Description: "AR cube placement"

XR Plugin Management:
- ✓ ARKit
```

---

## WebXR Implementation

### Step 1: WebXR Setup

```html
<!DOCTYPE html>
<html>
<head>
    <script src="https://aframe.io/releases/1.4.0/aframe.min.js"></script>
</head>
<body>
    <a-scene>
        <!-- VR environment -->
        <a-sky color="#1a1a2e"></a-sky>
        
        <!-- Cube -->
        <a-entity id="cube" position="0 1.5 -2">
            <!-- Generate cube cells here -->
        </a-entity>
        
        <!-- VR controllers -->
        <a-entity id="leftHand" 
                  hand-controls="hand: left"
                  laser-controls></a-entity>
        <a-entity id="rightHand" 
                  hand-controls="hand: right"
                  laser-controls></a-entity>
        
        <!-- Camera rig -->
        <a-entity id="rig" position="0 0 0">
            <a-camera></a-camera>
        </a-entity>
    </a-scene>
    
    <script src="cube-webxr.js"></script>
</body>
</html>
```

### Step 2: WebXR Interaction

```javascript
// cube-webxr.js
AFRAME.registerComponent('cube-interaction', {
    init: function() {
        this.el.addEventListener('click', (e) => {
            // Place color on cube face
            placeColor(e.detail.intersection.point);
        });
    }
});

function placeColor(point) {
    // Convert world point to grid position
    const gridPos = worldToGrid(point);
    
    // Create colored cell
    const cell = document.createElement('a-box');
    cell.setAttribute('position', gridPos);
    cell.setAttribute('color', currentColor);
    cell.setAttribute('scale', '0.09 0.09 0.09');
    
    document.querySelector('#cube').appendChild(cell);
}
```

---

## Platform Comparison

| Feature | Unity VR | Unity AR | WebXR |
|---------|----------|----------|-------|
| **Development Time** | 3-5 days | 2-4 days | 4-6 days |
| **Hardware Cost** | $300-500 | $0 (phone) | $0 (browser) |
| **Platform Support** | Quest, PSVR, PC VR | iOS, Android | Any VR headset |
| **Performance** | Excellent | Good | Good |
| **Distribution** | App stores | App stores | Web (instant) |
| **Ease of Use** | Medium | Easy | Medium |
| **Testing** | Need headset | Phone only | Browser only |

---

## Best Practices

### VR Comfort

✅ **DO:**
- Keep cube at 1-2 meter distance
- Allow smooth rotation (no snap)
- Provide teleportation for movement
- Add comfort vignette option
- Target 90fps minimum

❌ **DON'T:**
- Force rapid movement
- Use artificial rotation
- Place objects too close
- Ignore comfort settings

### AR Usability

✅ **DO:**
- Show clear placement indicator
- Allow repositioning of cube
- Provide good lighting detection
- Add shadows for depth perception
- Keep UI minimal and clear

❌ **DON'T:**
- Force specific surface type
- Occlude camera view
- Use complex gestures
- Require perfect tracking

### Input Design

**VR Controller Mapping:**
```
Right Hand:
- Trigger: Place color
- Grip: Remove color
- Thumbstick: Rotate cube
- A/X Button: Change color

Left Hand:
- Trigger: Select color (UI)
- Grip: Menu
- Thumbstick: Scale cube
- B/Y Button: Reset
```

**AR Touch Mapping:**
```
Single Tap: Place color
Long Press: Remove color
Two-finger Pinch: Scale cube
Two-finger Rotate: Rotate cube
Swipe: Change color
```

---

## Estimated Timelines

### Unity VR (Meta Quest)
```
Day 1: Setup Unity + XR Toolkit
Day 2: Implement cube generation
Day 3: Add VR interaction
Day 4: UI and polish
Day 5: Testing and optimization
```

### Unity AR (iOS/Android)
```
Day 1: Setup AR Foundation
Day 2: Plane detection + placement
Day 3: Touch interaction
Day 4: UI and feedback
```

### WebXR (Browser VR)
```
Day 1-2: A-Frame setup
Day 3-4: Cube system
Day 5: Controller interaction
Day 6: Testing and polish
```

---

## Conclusion

The 3D Color Cube Puzzle's architecture makes VR/AR adaptation straightforward:

1. **3D coordinates** → Already implemented
2. **Rotation system** → Direct mapping
3. **Ray-casting** → Used for both desktop and XR
4. **Modular code** → Easy input layer swap

Choose platform based on:
- **VR**: Most immersive, requires hardware
- **AR**: Mobile-friendly, accessible
- **WebXR**: Instant access, cross-platform

---

**Ready to start?** Pick a platform and follow the guide above!

**Questions?** Open an issue on GitHub.

---

**Last Updated:** 2026-07-04  
**Version:** 1.0.0  
**Author:** Roshan

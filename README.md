# 🎲 3D Color Cube Puzzle

**An interactive 3D puzzle game with VR/AR-ready architecture**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Web%20%7C%20Unity%20%7C%20VR%20%7C%20AR-blue)](https://github.com)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Live Demo](#live-demo)
- [Installation](#installation)
- [How to Play](#how-to-play)
- [Technical Architecture](#technical-architecture)
- [VR/AR Implementation](#vrar-implementation)
- [Project Structure](#project-structure)
- [Development](#development)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

---

## 🎯 Overview

**3D Color Cube Puzzle** is an innovative interactive puzzle game that combines engaging gameplay with cutting-edge 3D visualization. Built with a modular architecture designed for seamless adaptation to VR and AR platforms.

### Key Highlights

- ✅ **Full 3D interaction** with intuitive mouse controls
- ✅ **5×5×5 cube grid** with 150 interactive cells
- ✅ **8-color palette** with visual selection interface
- ✅ **Real-time 3D rendering** using pure JavaScript
- ✅ **Professional UI/UX** design
- ✅ **VR/AR-ready architecture** for future expansion
- ✅ **Zero dependencies** - runs in any modern browser

---

## ✨ Features

### Core Gameplay

- 🎨 **Minimalist UI Design**
  - Clean interface optimized for VR/AR viewing
  - No distracting text overlays
  - Focus on the cube interaction
  - Essential controls only

- 🎨 **Color Placement System**
  - Click on any cube face to place colors
  - Shift+Click to remove colors
  - Real-time visual feedback
  - 8-color palette with emoji indicators

- 🔄 **Dynamic 3D Rotation**
  - Drag to rotate the cube 360°
  - Smooth interpolation for natural movement
  - Auto-rotation mode available
  - Optimized for VR/AR adaptation

- 🔍 **Zoom Controls**
  - Mouse wheel zoom (0.5x - 3x)
  - Maintains cube center focus
  - Fluid zoom transitions

- 📊 **Streamlined Progress Tracking**
  - Compact score and progress display
  - Move counter
  - Completion detection
  - Non-intrusive HUD

### Advanced Features

- **Depth Sorting Algorithm**
  - Proper face rendering order
  - Accurate 3D occlusion
  - Optimized for performance

- **Ray-Casting Interaction**
  - Precise face detection
  - Polygon intersection testing
  - Z-depth prioritization

- **Color Palette System**
  - 8 carefully chosen colors
  - Visual selection indicators
  - Emoji-based identification
  - Keyboard shortcuts (1-8)

- **Action System**
  - Reset view to default position
  - Toggle auto-rotation
  - Random fill for testing
  - Clear all functionality

---

## 🌐 Live Demo

### Web Version

Open `index.html` in any modern web browser:

```bash
# Chrome (recommended)
start chrome index.html

# Firefox
start firefox index.html

# Edge
start msedge index.html
```

**System Requirements:**
- Modern web browser (Chrome 90+, Firefox 88+, Edge 90+)
- JavaScript enabled
- Minimum 1280x720 screen resolution
- Mouse or trackpad for interaction

---

## 🚀 Installation

### Quick Start

1. **Clone or Download the Repository**
   ```bash
   git clone https://github.com/yourusername/3d-cube-puzzle.git
   cd 3d-cube-puzzle
   ```

2. **Open the Demo**
   - Simply open `index.html` in your browser
   - No build process required
   - No dependencies to install

### For Development

1. **Set up a local server** (optional, for advanced features)
   ```bash
   # Python
   python -m http.server 8000
   
   # Node.js
   npx http-server
   ```

2. **Open in browser**
   ```
   http://localhost:8000
   ```

---

## 🎮 How to Play

### Basic Controls

| Action | Control |
|--------|---------|
| **Rotate Cube** | Click and drag with mouse |
| **Zoom In/Out** | Scroll wheel up/down |
| **Place Color** | Click on cube face |
| **Remove Color** | Shift + Click on face |
| **Select Color** | Click palette or press 1-8 |
| **Toggle Auto-Rotate** | Press Space or click button |

### Gameplay Instructions

1. **Select a Color**
   - Click on any color in the right palette
   - Or press number keys 1-8
   - Selected color will be highlighted

2. **Place Colors on Cube**
   - Rotate the cube to see different faces
   - Click on any visible face to place your color
   - Each placement adds 10 points and 1 move

3. **Remove Colors**
   - Hold Shift key
   - Click on a colored face to remove it

4. **Complete the Puzzle**
   - Fill all 150 cells to complete the cube
   - Track your progress in the top-left display
   - Try to complete with minimum moves!

### Pro Tips

- 💡 Use auto-rotation to see all angles
- 💡 Plan your color placement strategy
- 💡 Use keyboard shortcuts for faster color switching
- 💡 Zoom in for precise placement on smaller faces
- 💡 Reset view if you lose orientation

---

## 🏗️ Technical Architecture

### Core Technologies

- **HTML5 Canvas** - Hardware-accelerated 2D rendering
- **Vanilla JavaScript** - No framework dependencies
- **CSS3** - Modern UI styling with gradients and shadows
- **Mathematical Transforms** - 3D rotation matrices and projection

### 3D Rendering Pipeline

```
User Input → 3D World Coordinates → Rotation Matrices → 
Projection → 2D Screen Coordinates → Canvas Rendering
```

#### Key Algorithms

1. **3D Transformation**
   ```javascript
   // Rotation around Y-axis
   rotateY(point, angle) {
     x' = x * cos(θ) + z * sin(θ)
     z' = -x * sin(θ) + z * cos(θ)
   }
   
   // Rotation around X-axis
   rotateX(point, angle) {
     y' = y * cos(θ) - z * sin(θ)
     z' = y * sin(θ) + z * cos(θ)
   }
   ```

2. **Perspective Projection**
   ```javascript
   project(point) {
     scale = focalLength / (focalLength + z)
     screenX = x * scale + centerX
     screenY = y * scale + centerY
   }
   ```

3. **Depth Sorting**
   ```javascript
   // Painter's algorithm
   faces.sort((a, b) => a.avgZ - b.avgZ)
   // Render from back to front
   ```

4. **Ray-Polygon Intersection**
   ```javascript
   // Point-in-polygon test
   pointInPolygon(point, polygon) {
     // Ray casting algorithm
     // Counts intersections with edges
   }
   ```

### Data Structures

```javascript
// Cube state storage
cube = {
  "x,y,z": {
    colorIndex: 0-7,
    color: { r, g, b }
  }
}

// Color palette
colorPalette = [
  { r, g, b, name, emoji },
  ...
]

// Camera state
{
  rotationX: float,
  rotationY: float,
  zoom: float
}
```

### Performance Optimizations

- ✅ **Efficient rendering** - Only visible faces are drawn
- ✅ **Smart depth sorting** - Minimal computational overhead
- ✅ **Event throttling** - Smooth 60fps animation
- ✅ **Memory efficient** - Sparse 3D grid storage
- ✅ **Canvas optimization** - Double buffering, clipping

---

## 🥽 VR/AR Implementation

### Architecture Design

This project is built with VR/AR adaptation in mind. The modular architecture allows seamless integration with:

- **Unity XR Toolkit**
- **WebXR API**
- **AR Foundation** (Unity)
- **A-Frame** (Web VR)

### VR Implementation Path

#### 1. Unity VR Version

```csharp
// Core logic remains identical
// Replace mouse input with VR controller

void HandleVRInput() {
    // Ray from VR controller
    Ray ray = new Ray(
        controller.position, 
        controller.forward
    );
    
    if (Physics.Raycast(ray, out hit)) {
        if (controller.triggerPressed) {
            PlaceColor(hit.point);
        }
    }
}
```

**Required Unity Packages:**
- XR Interaction Toolkit
- XR Plugin Management
- Oculus XR Plugin (for Quest)

**Estimated Implementation Time:** 3-5 days

#### 2. AR Version (Mobile)

```csharp
// AR plane detection
void HandleARTouch() {
    if (Input.touchCount > 0) {
        Touch touch = Input.GetTouch(0);
        
        if (arRaycastManager.Raycast(
            touch.position, 
            hits, 
            TrackableType.PlaneWithinPolygon
        )) {
            // Place cube in AR space
            PlaceCubeInAR(hits[0].pose);
        }
    }
}
```

**Required Unity Packages:**
- AR Foundation
- ARCore XR Plugin (Android)
- ARKit XR Plugin (iOS)

**Estimated Implementation Time:** 2-4 days

#### 3. WebXR Version

```javascript
// WebXR controller input
navigator.xr.requestSession('immersive-vr').then(session => {
    // Same cube logic
    // Replace canvas with WebGL context
    // Replace mouse with XR controller
});
```

**Estimated Implementation Time:** 4-6 days

### Interaction Mapping

| Desktop | VR | AR |
|---------|----|----|
| Mouse drag | Head tracking | Gyroscope |
| Mouse click | Trigger press | Screen tap |
| Scroll zoom | Grip buttons | Pinch gesture |
| Keyboard | Controller buttons | UI buttons |

### Why This Architecture Works

1. **Separation of Concerns**
   - Game logic ≠ Input handling
   - Rendering ≠ Game state
   - Easy to swap input methods

2. **3D-First Design**
   - Already using 3D coordinates
   - Rotation matrices in place
   - Ray-casting ready

3. **Modular Structure**
   - Functions are independent
   - Clear interfaces
   - Easy to port

---

## 📁 Project Structure

```
3D-Cube-Puzzle/
├── index.html              # Main game file (all-in-one)
├── README.md               # This file
├── LICENSE                 # MIT License
├── .gitignore             # Git ignore rules
├── docs/                   # Documentation (optional)
│   ├── ARCHITECTURE.md    # Technical deep dive
│   ├── VR_GUIDE.md       # VR implementation guide
│   └── API_REFERENCE.md   # Code API documentation
├── screenshots/            # Project screenshots
│   ├── gameplay.png
│   ├── ui.png
│   └── demo.gif
└── unity-version/          # Future Unity implementation
    └── (Unity project files)
```

### File Description

- **index.html** - Complete standalone game (HTML + CSS + JavaScript)
- **README.md** - Comprehensive project documentation
- **LICENSE** - MIT License for open-source use
- **.gitignore** - Excludes Unity Library, Temp, etc.

---

## 💻 Development

### Code Structure

The `index.html` file contains three main sections:

1. **HTML Structure** (`<body>`)
   - Canvas element
   - UI overlays
   - HUD displays

2. **CSS Styling** (`<style>`)
   - Layout and positioning
   - Visual effects
   - Responsive design
   - Animations

3. **JavaScript Logic** (`<script>`)
   - Game state management
   - 3D math functions
   - Rendering engine
   - Input handling
   - UI updates

### Key Functions

```javascript
// 3D Transformations
rotateX(point, angle)      // Rotate around X-axis
rotateY(point, angle)      // Rotate around Y-axis
project(point)             // 3D to 2D projection
transform3D(x, y, z)       // Complete transformation pipeline

// Rendering
drawCube()                 // Render entire cube
drawCubeFace(x, y, z, face, color) // Draw single face
getFaceCorners(x, y, z, face)      // Get face vertices

// Interaction
handleCanvasClick(event)   // Process mouse clicks
pointInPolygon(point, poly) // Hit detection

// Game Logic
selectColor(index)         // Change active color
updateUI()                 // Refresh displays
clearCube()               // Reset game
```

### Customization Options

#### Change Cube Size

```javascript
const cubeSize = 5; // Default: 5x5x5
// Change to 3 for 3x3x3 cube
// Change to 7 for 7x7x7 cube
```

#### Modify Cell Size

```javascript
const cellSize = 40; // Default: 40px per cell
// Increase for larger cubes
// Decrease for smaller display
```

#### Add New Colors

```javascript
colorPalette.push({
    r: 255, g: 255, b: 255,
    name: 'White',
    emoji: '⚪'
});
```

#### Adjust Camera

```javascript
let rotationX = -0.5;  // Initial X rotation
let rotationY = 0.7;   // Initial Y rotation
let zoom = 1.2;        // Initial zoom level
```

---

## 🗺️ Roadmap

### Version 1.1 (Current)
- ✅ Core 3D cube rendering
- ✅ Interactive color placement
- ✅ Full rotation and zoom
- ✅ Score and progress tracking
- ✅ Professional UI design

### Version 1.2 (Planned)
- ⬜ Pattern detection system
- ⬜ Achievements and badges
- ⬜ Undo/Redo functionality
- ⬜ Save/Load game state
- ⬜ Color themes

### Version 2.0 (Future)
- ⬜ Unity WebGL build
- ⬜ Multiplayer mode
- ⬜ Challenge levels
- ⬜ Time trials
- ⬜ Leaderboards

### Version 3.0 (VR/AR)
- ⬜ Unity VR implementation
- ⬜ Quest 2/3 support
- ⬜ Mobile AR version
- ⬜ Hand tracking
- ⬜ Voice commands

---

## 🤝 Contributing

Contributions are welcome! This is an educational project designed to demonstrate 3D rendering and VR/AR readiness.

### How to Contribute

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Commit your changes**
   ```bash
   git commit -m "Add amazing feature"
   ```
4. **Push to branch**
   ```bash
   git push origin feature/amazing-feature
   ```
5. **Open a Pull Request**

### Contribution Ideas

- 🎨 New color palettes
- 🎮 Additional game modes
- 🐛 Bug fixes and optimizations
- 📚 Documentation improvements
- 🌍 Internationalization (i18n)
- ♿ Accessibility features

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2026 Roshan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

## 👨‍💻 Author

**Roshan**

- 📧 Email: your.email@example.com
- 🐙 GitHub: [@yourusername](https://github.com/yourusername)
- 💼 LinkedIn: [Your Name](https://linkedin.com/in/yourprofile)
- 🌐 Portfolio: [yourwebsite.com](https://yourwebsite.com)

---

## 🙏 Acknowledgments

- Inspired by classic 3D puzzle games
- Built for educational purposes
- Designed with VR/AR future in mind
- Special thanks to the open-source community

---

## 📞 Support

For questions, issues, or feedback:

- 🐛 [Open an issue](https://github.com/yourusername/3d-cube-puzzle/issues)
- 💬 [Start a discussion](https://github.com/yourusername/3d-cube-puzzle/discussions)
- 📧 Email: your.email@example.com

---

## 🌟 Show Your Support

If you found this project helpful or interesting:

- ⭐ Star this repository
- 🍴 Fork it for your own experiments
- 📢 Share it with others
- 🐛 Report bugs or suggest features

---

<div align="center">

**Made with ❤️ by Roshan**

[⬆ Back to Top](#-3d-color-cube-puzzle)

</div>

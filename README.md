# 🎮 Bounce Ball 2D

<div align="center">

![Bounce Ball 2D Banner](https://img.shields.io/badge/Unity-2D%20Game-black?style=for-the-badge&logo=unity&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20PC-orange?style=for-the-badge)
![Language](https://img.shields.io/badge/Language-C%23-blue?style=for-the-badge&logo=csharp)
![License](https://img.shields.io/badge/License-Review%20Required-yellow?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge)

<br/>

> **A fast-paced 2D arcade ball-bouncing game built with Unity.**  
> Control the paddle, keep the ball alive, and chase the high score!

<br/>

![image alt](https://github.com/Chandan-Baskey/BounceBall-2dGame/blob/5ebc1c05752c23425790d06c28cca0b9607e0c9f/GameView1.png)

</div>

---

## 📖 Table of Contents

- [About the Game](#-about-the-game)
- [Gameplay](#-gameplay)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Scripts Overview](#-scripts-overview)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Game](#running-the-game)
- [Controls](#-controls)
- [Game Flow](#-game-flow)
- [How It Works](#-how-it-works)
- [Customization](#-customization)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🕹️ About the Game Details 

**Bounce Ball 2D** is a classic arcade-style game where your reflexes are everything. A ball bounces around the screen, and your only job is to keep it from falling — using a paddle you control with a tap or mouse click.

Simple to pick up. Impossible to master. Every bounce counts.

---

## 🎮 Gameplay

- The ball drops and bounces off the paddle and walls
- Tap left or right side of the screen to move the paddle in that direction
- Each successful bounce scores a point
- Miss the ball — game over
- Beat your high score every run

---

## ✨ Features

| Feature | Description |
|---|---|
| 🎮 Unified Input | Supports multitouch, mouse, keyboard, D-pad, and gamepad stick controls |
| 🏓 Dynamic Paddle | Smooth, physics-driven paddle movement |
| 📊 Live Score Tracking | Score updates in real-time on every bounce |
| 🏆 Persistent Best Score | Best score is saved between sessions |
| ⏸️ Pause & Resume | Supports keyboard, controller, and app focus changes |
| 🔄 Game Over & Restart | Shows final score and supports tap/key restart |
| 🚀 Game Start Screen | Clean "Tap to Start" panel before the game begins |
| 📱 Cross-Platform | Runs on Android, iOS, and PC |

---

## 🛠️ Tech Stack

- **Engine:** [Unity](https://unity.com/) (2D)
- **Language:** C# (MonoBehaviour scripts)
- **Physics:** Unity Rigidbody2D
- **UI:** Unity UI (Canvas / Text / Panel)
- **Scene Management:** UnityEngine.SceneManagement

---

## 📁 Project Structure

```
BounceBall2D/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Platform.cs          # Paddle movement logic
│   │   ├── Ball.cs              # Launch, collision, and bounce logic
│   │   ├── GameManager.cs       # State, score, pause, and persistence
│   │   └── UIManager.cs         # Main-menu navigation
│   │
│   ├── Scenes/
│   │   ├── MainScene.unity      # Main menu
│   │   └── LvL.unity            # Gameplay
│   └── Sprites/                 # Ball, paddle, and background art
│
├── ProjectSettings/
└── README.md
```

---

## 📜 Scripts Overview

### `Platform.cs` — Paddle Controller

Handles all paddle input and movement using Unity's **Rigidbody2D** physics system.

**Key Behaviour:**
- Uses Unity's Input System for mouse, keyboard, controller, and touch
- Splits touch and mouse input using the actual screen midpoint
- Tracks each active touch by touch ID and ignores touches over UI
- Cancels movement when left and right are pressed simultaneously
- Applies Rigidbody2D velocity during `FixedUpdate`
- Stops instantly when input is released

---

### `GameManager.cs` — Game State & Score

Manages the `Waiting`, `Playing`, `Paused`, and `GameOver` states, live score, persistent best score, restart flow, and application focus handling.

**Key Behaviour:**
- Uses a **Singleton** (`GameManager.instance`) for global access
- Rejects score changes outside active gameplay
- Saves a new best score with `PlayerPrefs`
- Pauses automatically when the app loses focus
- Shows final and best scores before restarting

---

## 🚀 Getting Started

### Prerequisites

- [Unity Hub](https://unity.com/download) installed
- Unity Editor **2020.3 LTS or newer** (2D module required)
- Git (optional, for cloning)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/bounce-ball-2d.git
   cd bounce-ball-2d
   ```

2. **Open in Unity Hub:**
   - Open Unity Hub
   - Click **"Open"** → select the cloned project folder
   - Unity will import all assets automatically

3. **Open the Game Scene:**
   - In the **Project** panel, navigate to `Assets/Scenes/`
   - Double-click `GameScene.unity`

### Running the Game

**In Unity Editor:**
- Press the ▶️ **Play** button at the top of the editor

**Build for Android:**
```
File → Build Settings → Android → Switch Platform → Build & Run
```

**Build for PC:**
```
File → Build Settings → PC, Mac & Linux Standalone → Build & Run
```

---

## 🎯 Controls

| Platform | Action | Control |
|---|---|---|
| 📱 Mobile | Move paddle left | Tap left half of screen |
| 📱 Mobile | Move paddle right | Tap right half of screen |
| 🖥️ Desktop | Move paddle left | Hold left mouse button (left of center) |
| 🖥️ Desktop | Move paddle right | Hold left mouse button (right of center) |
| ⌨️ Keyboard | Move paddle | `A` / `D` or Left / Right arrows |
| 🎮 Controller | Move paddle | Left stick or D-pad |
| All | Start round | Tap/click, Space, Enter, gamepad South button, or Start |
| Keyboard/controller | Pause/resume | `Esc`, `P`, or gamepad Start |
| Mobile | Resume after focus pause | Tap the pause overlay |
| All | Restart after game over | Tap/click, `R`, Space, Enter, or gamepad South button |

If left and right are requested at the same time—including two touches on opposite halves—the inputs cancel and the paddle stops. UI touches are never forwarded to paddle movement. The pointer used to start a round must be released before pointer movement becomes active.

---

## 🔄 Game Flow

```
┌─────────────────────┐
│    App Launches      │
│  "TAP TO START"      │
│  Panel is shown      │
└────────┬────────────┘
         │  Player taps
         ▼
┌─────────────────────┐
│   GameStart()        │
│ - Hide start panel   │
│ - Show score UI      │
└────────┬────────────┘
         │  Ball starts moving
         ▼
┌─────────────────────┐
│    Active Gameplay   │
│ - Ball bounces       │
│ - Player moves pad   │◄──── AddScore() on each bounce
│ - Score increases    │
└────────┬────────────┘
         │  Ball missed
         ▼
┌─────────────────────┐
│     Game Over        │
│  Show Restart UI     │
└────────┬────────────┘
         │  Player taps Restart
         ▼
┌─────────────────────┐
│   Restart()          │
│ Scene reloads fresh  │
└─────────────────────┘
```

---

## ⚙️ How It Works

### Physics Setup

The ball uses Unity's **Rigidbody2D** with a **PhysicsMaterial2D** set to high bounciness (`bounciness = 1`, `friction = 0`) so it never loses energy.

The paddle also uses **Rigidbody2D** but is moved via `velocity` rather than `transform` — this ensures proper physics collision response instead of the ball clipping through.

### Input Detection

The game uses Unity's **Input System**. Keyboard and controller bindings are represented by input actions. Mouse and touch positions remain in screen space and are compared with `Screen.width * 0.5f`, so the control split does not depend on the camera's world position.

Every active touch is read separately with its touch ID. Before a pointer contributes movement, the EventSystem raycasts the pointer position against UI; pointers over buttons or other raycastable UI are rejected.

### Singleton Pattern

`GameManager` uses a static `instance` variable so any script in the scene can call:
```csharp
GameManager.instance.AddScore();
GameManager.instance.Restart();
```
No `FindObjectOfType` needed — clean and performant.

---

## 🎨 Customization

You can tweak the following directly in the **Unity Inspector** without changing code:

| Parameter | Script | Description |
|---|---|---|
| `speed` | `Platform.cs` | How fast the paddle moves |
| `textScore` | `GameManager.cs` | UI Text element for score display |
| `gameStartPanel` | `GameManager.cs` | The "Tap to Start" panel object |

**Want to add difficulty scaling?**  
Increase ball speed over time by accessing the ball's `Rigidbody2D` and adding to its velocity magnitude after each `AddScore()` call.

The best score is already persisted with `PlayerPrefs` under the `BestScore` key.

---

## 🤝 Contributing

Contributions, bug reports, and feature requests are welcome!

1. Fork the repository
2. Create your feature branch:
   ```bash
   git checkout -b feature/awesome-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add awesome feature"
   ```
4. Push to the branch:
   ```bash
   git push origin feature/awesome-feature
   ```
5. Open a Pull Request

---

## 📄 License

No repository license file is currently included. Review the project code license and every third-party font/image license before redistribution or commercial release.

---

<div align="center">

Made with ❤️ and Unity

⭐ **Star this repo if you found it useful!** ⭐

</div>

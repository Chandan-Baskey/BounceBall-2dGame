# 🎮 Bounce Ball 2D

<div align="center">

![Bounce Ball 2D Banner](https://img.shields.io/badge/Unity-2D%20Game-black?style=for-the-badge&logo=unity&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20PC-orange?style=for-the-badge)
![Language](https://img.shields.io/badge/Language-C%23-blue?style=for-the-badge&logo=csharp)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge)

<br/>

> **A fast-paced 2D arcade ball-bouncing game built with Unity.**  
> Control the paddle, keep the ball alive, and chase the high score!

<br/>

[image alt](https://github.com/Chandan-Baskey/Maze-Ball/blob/d78fd3be2c4b42164ea8757eb502daee167f8de0/GameView.png)

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

## 🕹️ About the Game

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
| 🖱️ Touch & Mouse Input | Supports both touch (mobile) and mouse (desktop) controls |
| 🏓 Dynamic Paddle | Smooth, physics-driven paddle movement |
| 📊 Live Score Tracking | Score updates in real-time on every bounce |
| 🔄 Instant Restart | One-tap restart to get back into the action |
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
│   │   └── GameManager.cs       # Score, game state, restart logic
│   │
│   ├── Scenes/
│   │   └── GameScene.unity      # Main game scene
│   │
│   ├── Sprites/
│   │   ├── Ball.png             # Ball sprite
│   │   └── Platform.png         # Paddle sprite
│   │
│   └── Prefabs/
│       └── Ball.prefab          # Ball prefab (with Rigidbody2D + Collider)
│
├── ProjectSettings/
└── README.md
```

---

## 📜 Scripts Overview

### `Platform.cs` — Paddle Controller

Handles all paddle input and movement using Unity's **Rigidbody2D** physics system.

```csharp
public class Platform : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;        // Configurable speed in Inspector

    void TouchMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move LEFT if tap is on left side of screen, RIGHT otherwise
            rb.velocity = (mousePos.x < 0) ? Vector2.left * speed : Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;    // Stop when not pressing
        }
    }
}
```

**Key Behaviour:**
- Converts screen tap position to world coordinates
- Moves paddle left if tap is on the left half of screen
- Moves paddle right if tap is on the right half
- Stops instantly when input is released

---

### `GameManager.cs` — Game State & Score

Manages the overall game loop: starting the game, tracking the score, and restarting the scene.

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager instance;   // Singleton pattern
    int score;

    public void AddScore()
    {
        score++;
        textScore.text = score.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameStart()
    {
        gameStartPanel.SetActive(false);    // Hide start panel
        textScore.gameObject.SetActive(true); // Show score UI
    }
}
```

**Key Behaviour:**
- Uses a **Singleton** (`GameManager.instance`) for global access
- `AddScore()` — call this from the Ball script whenever a bounce occurs
- `Restart()` — reloads the active scene instantly
- `GameStart()` — hides the intro panel and activates score display

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

Unity's `Input.GetMouseButton(0)` works for both:
- **Mouse** clicks on desktop
- **Touch** taps on mobile (Unity maps single-touch to mouse input automatically)

The tap position is converted from **Screen Space** → **World Space** using:
```csharp
Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
```

This allows directional decision-making regardless of screen resolution or orientation.

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

**Want a high score system?**  
Use `PlayerPrefs.SetInt("HighScore", score)` inside `GameManager` to persist scores between sessions.

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

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">

Made with ❤️ and Unity

⭐ **Star this repo if you found it useful!** ⭐

</div>

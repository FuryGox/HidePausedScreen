# Hide Paused Screen

A quality-of-life mod for **[Stacklands](https://store.steampowered.com/app/1298810/Stacklands/)** that gives you full control over the pause screen visuals. Remove the blinking **PAUSED** text and dark visual effects (vignette, color grading, and focus blur) for clean gameplay and screenshots, or customize the pause text color with an interactive color picker.

---

## ✨ Features

- 🚫 **Hide 'PAUSED' Text**: Eliminates the blinking "PAUSED" text banner across the screen when paused (Spacebar / Speed 0).
- 🖼️ **Disable Pause Visual Effects**:
  - Turns off the dark vignette and color grading (`PauseVolume`).
  - Clears focus blur / depth of field (`FocusVolume`) for crisp, unobstructed visibility while paused.
- 🎨 **Custom Text Color**:
  - Keep the pause text visible while matching your preferred aesthetic.
  - Built-in interactive color palette popup for quick color selection.
  - Supports custom Hex color codes (`#RRGGBB`).
- 🖱️ **Context Menu Shortcut**:
  - Quickly access the color picker directly from the in-game context menu (`Set Paused Text Color`).
- ⚙️ **In-Game Mod Settings**:
  - All features can be configured and toggled on the fly via the Stacklands Mod Settings menu.

---

## ⚙️ Configuration

You can customize the mod's behavior directly in Stacklands through the **Mods** settings menu:

| Setting | In-Game Name | Default | Description |
| :--- | :--- | :---: | :--- |
| `Hide Paused Text` | **Hide 'Paused' Text** | `true` | Hides the blinking "PAUSED" text when game speed is set to 0. |
| `Disable Visual Effects` | **Disable Pause Effects** | `true` | Disables pause vignette, color grading, and camera focus blur. |
| `Custom Text Color` | **Use Custom Text Color** | `false` | Enables your custom color for the "PAUSED" text (when not hidden). |
| `Paused Text Color` | **Paused Text Color** | `#FFFFFF` | Hex color code applied to the "PAUSED" text when custom color is enabled. |

---

## 🕹️ How to Use

1. **Default Experience**:
   - Once installed, the mod will automatically hide the blinking "PAUSED" text and remove all pause post-processing effects whenever you pause the game with <kbd>Space</kbd>.
2. **Custom Text Styling**:
   - If you prefer to keep the "PAUSED" text visible but want a different look, go to **Mod Settings**, uncheck **Hide 'Paused' Text**, and check **Use Custom Text Color**.
   - Open the **Set Paused Text Color** menu item to pick from the color palette or enter your own Hex color code.

---

## 📥 Installation

### Using Steam Workshop (Recommended)
- Subscribe to the mod on the Stacklands Steam Workshop.

### Manual Installation
1. Download the latest release from the Releases page.
2. Extract the `hidepausedscreen` folder into your Stacklands mods directory:
   - **Windows**: `%userprofile%/AppData/LocalLow/sokpop/Stacklands/Mods/`
3. Launch the game and enable the mod in the **Mods** menu.

---

## 🛠️ Building from Source

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (targeting `.NET Standard 2.1`)
- Python 3.x (optional, for running `build.py`)
- Stacklands game files

### Build Steps
1. Clone this repository:
   ```bash
   git clone https://github.com/FuryGox/HidePausedScreen.git
   cd HidePausedScreen
   ```
2. Build with .NET CLI:
   ```bash
   dotnet build
   ```
3. Or build and sync directly to your local Stacklands Mods folder using Python:
   ```bash
   python build.py
   ```

---

## 🔧 Technical Details

This mod uses **[Harmony](https://github.com/pardeike/Harmony)** to patch the following game methods:
- **`GameScreen.Update`**: Postfix patch to toggle the active state and color of `__instance.PausedText`.
- **`GameCamera.Update`**: Postfix patch to disable `__instance.PauseVolume` and zero out `__instance.FocusVolume.weight`.

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details (or standard open-source terms).
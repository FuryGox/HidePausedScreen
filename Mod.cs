using HarmonyLib;
using System;
using UnityEngine;

namespace HidePausedScreenNS
{
    public class HidePausedScreen : Mod
    {
        public static HidePausedScreen? Instance { get; private set; }

        public ConfigEntry<bool>? HidePausedTextConfig;
        public ConfigEntry<bool>? DisableEffectsConfig;
        public ConfigEntry<bool>? CustomTextColorConfig;
        public ConfigEntry<string>? TextColorHexConfig;

        public Color CurrentTextColor { get; private set; } = Color.white;

        public override void Ready()
        {
            Instance = this;

            // 1. Configuration: Toggle hiding the "PAUSED" text
            HidePausedTextConfig = Config.GetEntry<bool>("Hide Paused Text", true);
            HidePausedTextConfig.UI.Name = "Hide 'Paused' Text";
            HidePausedTextConfig.UI.Tooltip = "Hide the blinking PAUSED text when the game speed is set to 0 (Spacebar).";

            // 2. Configuration: Toggle pause visual effects (PauseVolume & FocusVolume)
            DisableEffectsConfig = Config.GetEntry<bool>("Disable Visual Effects", true);
            DisableEffectsConfig.UI.Name = "Disable Pause Effects";
            DisableEffectsConfig.UI.Tooltip = "Disable vignette, color grading, and focus blur when paused.";

            // 3. Configuration: Toggle custom text color for PAUSED text (when not hidden)
            CustomTextColorConfig = Config.GetEntry<bool>("Custom Text Color", false);
            CustomTextColorConfig.UI.Name = "Use Custom Text Color";
            CustomTextColorConfig.UI.Tooltip = "Apply custom color to PAUSED text instead of default white when not hidden.";

            // 4. Color picker config using CustomMenuAPI.SetupColorConfig & ColorSettingUI
            TextColorHexConfig = CustomMenuAPI.SetupColorConfig(
                this,
                "Paused Text Color",
                "Paused Text Color",
                "Color for the PAUSED text when custom text color is enabled.",
                "#FFFFFF",
                (color) =>
                {
                    CurrentTextColor = color;
                }
            );

            CurrentTextColor = ColorSettingUI.ParseColor(TextColorHexConfig, Color.white);

            // 5. Register action in CustomMenuAPI card menu to allow opening the color picker directly from the context menu
            CustomMenuAPI.RegisterAction(
                "Set Paused Text Color",
                (card) =>
                {
                    ColorPalettePopup.Open("Paused Text Color", CurrentTextColor, ColorSettingUI.PaletteColors, (newColor) =>
                    {
                        CurrentTextColor = newColor;
                        if (TextColorHexConfig != null)
                        {
                            TextColorHexConfig.Value = ColorSettingUI.ColorToHex(newColor);
                        }
                    });
                },
                condition: (card) => true,
                priority: -100
            );

            Harmony.PatchAll();
            Logger.Log("HidePausedScreen initialized with customizable options & color picker.");
        }
    }

    /// <summary>
    /// Patch GameScreen.Update: Controls visibility and text color of PausedText
    /// </summary>
    [HarmonyPatch(typeof(GameScreen), "Update")]
    public static class GameScreen_Update_Patch
    {
        public static void Postfix(GameScreen __instance)
        {
            if (__instance == null || __instance.PausedText == null) return;

            var mod = HidePausedScreen.Instance;
            if (mod == null) return;

            bool shouldHideText = mod.HidePausedTextConfig != null && mod.HidePausedTextConfig.Value;

            if (shouldHideText)
            {
                // Hide the "PAUSED" text
                __instance.PausedText.gameObject.SetActive(false);
            }
            else
            {
                // If not hidden, check if custom color should be applied
                if (mod.CustomTextColorConfig != null && mod.CustomTextColorConfig.Value)
                {
                    __instance.PausedText.color = mod.CurrentTextColor;
                }
            }
        }
    }

    /// <summary>
    /// Patch GameCamera.Update: Controls disabling visual effects (PauseVolume & FocusVolume)
    /// </summary>
    [HarmonyPatch(typeof(GameCamera), "Update")]
    public static class GameCamera_Update_Patch
    {
        public static void Postfix(GameCamera __instance)
        {
            if (__instance == null) return;

            var mod = HidePausedScreen.Instance;
            if (mod == null) return;

            bool shouldDisableEffects = mod.DisableEffectsConfig != null && mod.DisableEffectsConfig.Value;
            if (!shouldDisableEffects) return;

            // Disable PauseVolume (vignette / color grading when SpeedUp == 0f)
            if (__instance.PauseVolume != null)
            {
                __instance.PauseVolume.enabled = false;
                if (__instance.PauseVolume.gameObject != null && __instance.PauseVolume.gameObject.activeSelf)
                {
                    __instance.PauseVolume.gameObject.SetActive(false);
                }
            }

            // Disable FocusVolume (blur / focus when CurrentGameState == Paused)
            if (WorldManager.instance != null && WorldManager.instance.CurrentGameState == WorldManager.GameState.Paused)
            {
                if (__instance.FocusVolume != null)
                {
                    __instance.FocusVolume.weight = 0f;
                }
            }
        }
    }
}
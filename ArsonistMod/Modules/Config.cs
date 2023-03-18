using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System;
using UnityEngine;

namespace ArsonistMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<float> masochismHealthMultiplierOnPowered;
        public static ConfigEntry<float> timeBeforeHeatGaugeDecays;
        public static ConfigEntry<bool> shouldHaveVoice;
        public static ConfigEntry<bool> shouldEnableDieKey;

        public static ConfigEntry<KeyboardShortcut> emoteSitKey;
        public static ConfigEntry<KeyboardShortcut> emoteStrutKey;
        public static ConfigEntry<KeyboardShortcut> dieKey;

        public static void ReadConfig()
        {

            timeBeforeHeatGaugeDecays = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("00 - General", "Wait Timer on Heat Gauge"),
                5f,
                new ConfigDescription("Determines how long the player must wait before the heat gauge starts decaying after overheating.",
                    null,
                    Array.Empty<object>()
                )
            );
            masochismHealthMultiplierOnPowered = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("01 - Masochism", "Masochism Health Multiplier when activated under overheat."),
                1f,
                new ConfigDescription("Determines how much the damage should be multiplied by before converting to health for Fire damage received.",
                    null,
                    Array.Empty<object>()
                )
            );

            emoteSitKey = ArsonistPlugin.instance.Config.Bind<KeyboardShortcut>
            (
                new ConfigDefinition("02 - Emotes", "Emote Sit"),
                new KeyboardShortcut(UnityEngine.KeyCode.Alpha1),
                new ConfigDescription("Determines the key to trigger Arsonist to Emote Sit.", null, Array.Empty<object>())
            );

            emoteStrutKey = ArsonistPlugin.instance.Config.Bind<KeyboardShortcut>
            (
                new ConfigDefinition("02 - Emotes", "Emote Strut"),
                new KeyboardShortcut(UnityEngine.KeyCode.Alpha2),
                new ConfigDescription("Determines the key to trigger Arsonist to Emote Strut.", null, Array.Empty<object>())
            );

            shouldEnableDieKey = ArsonistPlugin.instance.Config.Bind<bool>
            (
                new ConfigDefinition("02 - Emotes", "Enable Die key."),
                false,
                new ConfigDescription("Die key is disabled by default. When enabled, it allows you to press the die key to instantly end the run.", null, Array.Empty<object>())
            );

            dieKey = ArsonistPlugin.instance.Config.Bind<KeyboardShortcut>
            (
                new ConfigDefinition("02 - Emotes", "Die"),
                new KeyboardShortcut(UnityEngine.KeyCode.Alpha9),
                new ConfigDescription("Determines the key to die.", null, Array.Empty<object>())
            );

            shouldHaveVoice = ArsonistPlugin.instance.Config.Bind<bool>
            (
                new ConfigDefinition("03 - Voice", "Arsonist can laugh/grunt/snicker."),
                true,
                new ConfigDescription("By default, Arsonist can laugh/grunt when Masochism or ZPB is used. When off, no voice will be played.", null, Array.Empty<object>())
            );
        }

        public static void SetupRiskOfOptions() 
        {
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    masochismHealthMultiplierOnPowered,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 15f,
                        increment = 0.25f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    timeBeforeHeatGaugeDecays,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 50f,
                        increment = 1f
                    }
                ));

            ModSettingsManager.AddOption(new KeyBindOption(
                emoteSitKey));
            ModSettingsManager.AddOption(new KeyBindOption(
                emoteStrutKey));
            ModSettingsManager.AddOption(new KeyBindOption(
                dieKey));

            ModSettingsManager.AddOption(new CheckBoxOption(shouldHaveVoice));
            ModSettingsManager.AddOption(new CheckBoxOption(shouldEnableDieKey));
        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, string description = "Set to false to disable this character", bool enabledDefault = true) {

            return ArsonistPlugin.instance.Config.Bind<bool>("General",
                                                          "Enable " + characterName,
                                                          enabledDefault,
                                                          description);
        }
    }
}
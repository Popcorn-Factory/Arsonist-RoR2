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
                1.5f,
                new ConfigDescription("Determines how much the damage should be multiplied by before converting to health for Fire damage received.",
                    null,
                    Array.Empty<object>()
                )
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
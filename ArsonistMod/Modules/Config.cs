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
        public static ConfigEntry<bool> overheatTextShouldVibrate;
        public static ConfigEntry<float> baseGaugeLowerBoundRecharge;
        public static ConfigEntry<float> baseGaugeUpperBoundRecharge;

        public static ConfigEntry<KeyboardShortcut> emoteSitKey;
        public static ConfigEntry<KeyboardShortcut> emoteStrutKey;
        public static ConfigEntry<KeyboardShortcut> dieKey;

        /*
         
        //passive onfire buff
        internal static float igniteAttackSpeedMultiplier = 1.25f;
        internal static float igniteDamageReduction = 0.5f;
        internal static float igniteDamageMultiplier = 1.5f;
        internal static float igniteMovespeedMultiplier = 1.5f;
        internal static float overheatRegenMultiplier = 2f;
        internal static float secondaryCooldownMultiplier = 0.6f;
        internal static float utilityCooldownMultiplier = 0.33f;
        internal static float specialCooldownMultiplier = 0.6f;

        internal static int noOfSegmentsOnOverheatGauge = 150;
        internal static float maxBlueWhiteSegment = 0.9f;
        internal static Vector3 SegmentedValuesOnGaugeAlt = new Vector3(0.6f, 0.3f, 0.1f);
        internal static Vector3 SegmentedValuesOnGaugeMain = new Vector3(0.9f, 0f, 0.1f);


        //energy
        internal static float baseEnergy = 100f;
        internal static float levelEnergy = 5f;
        internal static float regenOverheatFraction = 0.05f;
        internal static float backupEnergyGain = 5f;
        internal static float hardlightEnergyGain = 15f;
        internal static float lysateEnergyGain = 10f;
        internal static float backupBlueGain = 0.1f;
        internal static float hardlightBlueGain = 0.3f;
        internal static float lysateBlueGain = 0.2f;
        internal static float blueDamageMultiplier = 2f;

        //firespray
        internal static float firesprayWeakDamageCoefficient = 1.5f;
        internal static float firesprayStrongDamageCoefficient = 3f;
        internal static float firesprayEnergyCost = 8f;
        internal static float firesprayBlastRadius = 5f;
        internal static float firesprayweakBlastRadius = 2.5f;
        //flaregun
        internal static float flareWeakDamageCoefficient = 2f;
        internal static float flareStrongDamageCoefficient = 4f;
        internal static float flareBlastRadius = 5f;
        internal static float flareEnergyCost = 20f;
        internal static float flareSpeedCoefficient = 200f;
        internal static int flareTickNum = 5;
        internal static float flareInterval = 0.5f;

        //zeropointpunch
        internal static float zeropointpounchDamageCoefficient = 4f;
        internal static float zeropointBlastRadius = 5f;
        internal static float zeropointHeatReductionMultiplier = 0.5f;
        //cleanse
        internal static float cleanseDuration = 4f;
        internal static float cleanseHeatReductionMultiplier = 0.5f;
        internal static float cleanseDamageCoefficient = 0.5f;
        internal static float cleanseBlastRadius = 10f;

        //Masochism
        internal static float masochismBuffDuration = 6f;
        internal static float masochismHealCoefficient = 0.03f;
        internal static float masochismEnergyCost = 50f;

         */

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
            overheatTextShouldVibrate = ArsonistPlugin.instance.Config.Bind<bool>
            (
                new ConfigDefinition("00 - General", "Overheat Gauge text can vibrate"),
                true,
                new ConfigDescription("Controls whether the Overheat Gauge Text will vibrate slightly when overheat state is reached.", null, Array.Empty<object>())
            );
            masochismHealthMultiplierOnPowered = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("01 - Masochism", "Masochism Health Multiplier"),
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
                new ConfigDefinition("02 - Emotes", "Enable Die key"),
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

            baseGaugeLowerBoundRecharge = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("04 - Gauge", "Base Gauge Lower Bound Cooling"),
                0.8f,
                new ConfigDescription("Determines how fast the cooling occurs at 0%. Scales upwards in a parabola.")
            );

            baseGaugeUpperBoundRecharge = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("04 - Gauge", "Base Gauge Upper Bound Cooling"),
                1.4f,
                new ConfigDescription("Determines how fast the cooling occurs at 100%. Scales upwards in a parabola.")
            );
        }

        public static void SetupRiskOfOptions() 
        {
            //Mod Icon 
            Sprite icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("rooIcon");
            ModSettingsManager.SetModIcon(icon);
            ModSettingsManager.SetModDescription("The Manic Incendiary");

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

            ModSettingsManager.AddOption(new CheckBoxOption(overheatTextShouldVibrate));

            ModSettingsManager.AddOption(new StepSliderOption(
                baseGaugeLowerBoundRecharge,
                new StepSliderConfig 
                {
                    min = 0f,
                    max = 5f,
                    increment = 0.1f
                }
            ));
            ModSettingsManager.AddOption(new StepSliderOption(
                baseGaugeUpperBoundRecharge,
                new StepSliderConfig
                {
                    min = 0f,
                    max = 5f,
                    increment = 0.1f
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
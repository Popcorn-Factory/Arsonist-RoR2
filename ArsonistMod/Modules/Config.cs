﻿using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System;
using UnityEngine;

namespace ArsonistMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<float> masochismHeatChangedThreshold;
        public static ConfigEntry<int> masochismMinimumRequiredToActivate;
        public static ConfigEntry<int> masochismMaximumStack;
        //public static ConfigEntry<float> masochismHealthMultiplierOnPowered;
        public static ConfigEntry<float> masochismActiveMultipliedActive;

        public static ConfigEntry<float> masochismSurgeHealOnHitPercentage;

        public static ConfigEntry<float> timeBeforeHeatGaugeDecays;
        public static ConfigEntry<bool> shouldHaveVoice;
        public static ConfigEntry<bool> shouldEnableDieKey;
        public static ConfigEntry<bool> overheatTextShouldVibrate;
        public static ConfigEntry<float> baseGaugeLowerBoundRecharge;
        public static ConfigEntry<float> baseGaugeUpperBoundRecharge;

        public static ConfigEntry<KeyboardShortcut> emoteSitKey;
        public static ConfigEntry<KeyboardShortcut> emoteStrutKey;
        public static ConfigEntry<KeyboardShortcut> emoteLobbyKey;
        public static ConfigEntry<KeyboardShortcut> dieKey;
        //public static ConfigEntry<bool> enableOldLoadout;

        public static ConfigEntry<float> passiveHealthPercentageTriggerIgnite;

        public static ConfigEntry<int> flareSalvoAmount;

        public static ConfigEntry<bool> cleanseRingFireEffectEnabled;

        public static ConfigEntry<bool> ToggleMasochismFOVWarp;

        public static ConfigEntry<int> flareVoicelineChance;

        public static ConfigEntry<bool> enableAggressiveHeatHaze;

        public static ConfigEntry<bool> enableNonAggressiveHeatHaze;

        public static ConfigEntry<float> arsonistSFXVolume;
        public static ConfigEntry<float> arsonistVoicelineVolume;
        public static ConfigEntry<float> arsonistVoicelineVolumeArsonist;
        public static ConfigEntry<float> arsonistVoicelineVolumeFirebug;

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
            enableAggressiveHeatHaze = ArsonistPlugin.instance.Config.Bind<bool>
                (
                    new ConfigDefinition("00 - General", "Aggressive Heat Haze Effect Enabled"),
                    true,
                    new ConfigDescription("Controls whether the heat haze effect should show in Arsonist's moves. (Moves Affected: Masochism)")
                );

            enableNonAggressiveHeatHaze = ArsonistPlugin.instance.Config.Bind<bool>
                (
                    new ConfigDefinition("00 - General", "Non-Aggressive Heat Haze Effect Enabled"),
                    true,
                    new ConfigDescription("Controls whether the heat haze effect should show in Arsonist's moves. (Moves Affected: Dragon's Fury)")
                );



            //masochismHealthMultiplierOnPowered = ArsonistPlugin.instance.Config.Bind<float>
            //(
            //    new ConfigDefinition("01 - Masochism", "Health Multiplier"),
            //    1f,
            //    new ConfigDescription("Determines how much the damage should be multiplied by before converting to health for Fire damage received.",
            //        null,
            //        Array.Empty<object>()
            //    )
            //);

            masochismHeatChangedThreshold = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("01 - Masochism", "Anticipation Heat Changed Requirement"),
                100f,
                new ConfigDescription("Determines how much heat needs to built up/reduced to acquire a stack of Anticipation.",
                    null,
                    Array.Empty<object>()
                )
            );

            masochismMinimumRequiredToActivate = ArsonistPlugin.instance.Config.Bind<int>
            (
                new ConfigDefinition("01 - Masochism", "Minimum required to activate"),
                5,
                new ConfigDescription("Determines the minimum required amount of stacks to be allowed to activate Masochism.",
                    null,
                    Array.Empty<object>()
                )
            );

            masochismMaximumStack = ArsonistPlugin.instance.Config.Bind<int>
            (
                new ConfigDefinition("01 - Masochism", "Maximum Anticipation"),
                10,
                new ConfigDescription("Determines how many stacks of Anticipation can be built up at one time.",
                    null,
                    Array.Empty<object>()
                )
            );
            
            masochismActiveMultipliedActive = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("01 - Masochism", "Heal multiplication from damage during activation."),
                0.18f,
                new ConfigDescription("Determines how much should be healed from damage dealt during Masochism Active state.",
                    null,
                    Array.Empty<object>()
                )
            );


            ToggleMasochismFOVWarp = ArsonistPlugin.instance.Config.Bind<bool>
            (
                new ConfigDefinition("01 - Masochism", "FOV can warp during Masochism"),
                true,
                new ConfigDescription("Determines if the FOV can change during the duration and startup of Masochism.",
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

            emoteLobbyKey = ArsonistPlugin.instance.Config.Bind<KeyboardShortcut>
            (
                new ConfigDefinition("02 - Emotes", "Emote Match"),
                new KeyboardShortcut(UnityEngine.KeyCode.Alpha3),
                new ConfigDescription("Determines the key to trigger Arsonist to strike a match", null, Array.Empty<object>())
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
                new ConfigDefinition("03 - Voice/Volume", "Arsonist can laugh/grunt/snicker."),
                true,
                new ConfigDescription("By default, Arsonist can laugh/grunt when Masochism or ZPB is used. When off, no voice will be played.", null, Array.Empty<object>())
            );

            arsonistVoicelineVolume = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("03 - Voice/Volume", "Master Voice Volume"),
                50f,
                new ConfigDescription("Determines the volume for All voice lines, both Arsonist and Firebug.")
            );

            arsonistVoicelineVolumeArsonist = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("03 - Voice/Volume", "Arsonist Voice Volume"),
                50f,
                new ConfigDescription("Determines the volume for Arsonist's voice lines. (Does not affect Firebug, affected by Master Voice Volume)")
            );

            arsonistVoicelineVolumeFirebug = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("03 - Voice/Volume", "Firebug Voice Volume"),
                50f,
                new ConfigDescription("Determines the volume for Firebug's voice lines. (Does not affect Arsonist, affected by Master Voice Volume)")
            );

            arsonistSFXVolume = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("03 - Voice/Volume", "SFX Volume"),
                50f,
                new ConfigDescription("Determines the volume for all of Arsonist's effects, such as explosions and such. (Affected by the ROR2 Master and SFX Volume)")
            );

            baseGaugeLowerBoundRecharge = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("04 - Gauge", "Base Gauge Lower Bound Cooling"),
                0.7f,
                new ConfigDescription("Determines how fast the cooling occurs at 0%. Scales upwards in a parabola.")
            );

            baseGaugeUpperBoundRecharge = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("04 - Gauge", "Base Gauge Upper Bound Cooling"),
                1.2f,
                new ConfigDescription("Determines how fast the cooling occurs at 100%. Scales upwards in a parabola.")
            );

            passiveHealthPercentageTriggerIgnite = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("05 - Passive", "Percentage health lost before triggering ignite"),
                0.3f,
                new ConfigDescription("Percentage of health required to be inflicted before passive is triggered. If 0, it will always trigger.")
            );

            flareSalvoAmount = ArsonistPlugin.instance.Config.Bind<int>
            (
                new ConfigDefinition("06 - Flare", "Number of Salvos on explosion."),
                5,
                new ConfigDescription("Determines how many salvos are fired from one projectile after lifetime expires.",
                    null,
                    Array.Empty<object>()
                )
            );
            flareVoicelineChance = ArsonistPlugin.instance.Config.Bind<int>
            (
                new ConfigDefinition("06 - Flare", "Shout Fireball! chance"),
                1,
                new ConfigDescription("Determines the chance Arsonist shouts Fireball!. If voice is disabled, this will not play. If set to 0, this will not play.", null, Array.Empty<object>())
            );

            cleanseRingFireEffectEnabled = ArsonistPlugin.instance.Config.Bind<bool>
            (
                new ConfigDefinition("07 - Cleanse", "Ring Fire effect enabled"),
                true,
                new ConfigDescription("Determines whether the ring of fire should play when cleanses is activated.", null, Array.Empty<object>())
            );

            masochismSurgeHealOnHitPercentage = ArsonistPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("08 - Spite", "Heal multiplication from damage during activation."),
                0.05f,
                new ConfigDescription("Determines how much should be healed from damage dealt during Masochism: Surge Active state.",
                    null,
                    Array.Empty<object>()
                )
            );


            //enableOldLoadout = ArsonistPlugin.instance.Config.Bind<bool>
            //(
            //    new ConfigDefinition("08 - Secret", "Enable Old Loadout"),
            //    false,
            //    new ConfigDescription("Enables some old features of the mod from 1.0. Requires a restart to take effect.")
            //);
        }

        public static void SetupRiskOfOptions() 
        {
            //Mod Icon 
            Sprite icon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("rooIcon");
            ModSettingsManager.SetModIcon(icon);
            ModSettingsManager.SetModDescription("The Manic Incendiary");

            //ModSettingsManager.AddOption(
            //    new StepSliderOption(
            //        masochismHealthMultiplierOnPowered,
            //        new StepSliderConfig
            //        {
            //            min = 1f,
            //            max = 15f,
            //            increment = 0.25f
            //        }
            //    ));

            ModSettingsManager.AddOption(
                new StepSliderOption(
                    masochismHeatChangedThreshold,
                    new StepSliderConfig
                    {
                        min = 1f,
                        max = 1000f,
                        increment = 1f
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

            ModSettingsManager.AddOption(
                new IntSliderOption (
                    masochismMaximumStack,
                    new IntSliderConfig {
                        min = 1,
                        max = 100
                    }
                    )
                );

            ModSettingsManager.AddOption(
                new IntSliderOption(
                    masochismMinimumRequiredToActivate,
                    new IntSliderConfig
                    {
                        min = 1,
                        max = 100
                    }
                    )
                );


            ModSettingsManager.AddOption(new KeyBindOption(
                emoteSitKey));
            ModSettingsManager.AddOption(new KeyBindOption(
                emoteStrutKey));
            ModSettingsManager.AddOption(new KeyBindOption(
                emoteLobbyKey));
            ModSettingsManager.AddOption(new KeyBindOption(
                dieKey));

            ModSettingsManager.AddOption(new CheckBoxOption(shouldHaveVoice));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    arsonistVoicelineVolume,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                    )
                );
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    arsonistVoicelineVolumeArsonist,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                    )
                );
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    arsonistVoicelineVolumeFirebug,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                    )
                );

            ModSettingsManager.AddOption(
                new StepSliderOption(
                    arsonistSFXVolume,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                    )
                );

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

            //ModSettingsManager.AddOption(new CheckBoxOption(enableOldLoadout));

            ModSettingsManager.AddOption(new StepSliderOption(
                masochismActiveMultipliedActive,
                new StepSliderConfig
                {
                    min = 0.01f,
                    max = 10f,
                    increment = 0.01f
                }
            ));

            ModSettingsManager.AddOption(new StepSliderOption(
                passiveHealthPercentageTriggerIgnite,
                new StepSliderConfig
                {
                    min = 0.00f,
                    max = 1f,
                    increment = 0.01f
                }
            ));

            ModSettingsManager.AddOption(new IntSliderOption(
                flareSalvoAmount,
                new IntSliderConfig 
                {
                    min = 0,
                    max = 100,
                }));

            ModSettingsManager.AddOption(new IntSliderOption(
                flareVoicelineChance,
                new IntSliderConfig 
                {
                    min = 0,
                    max = 100,
                }
            ));

            ModSettingsManager.AddOption(new CheckBoxOption(cleanseRingFireEffectEnabled));
            ModSettingsManager.AddOption(new CheckBoxOption(ToggleMasochismFOVWarp));

            ModSettingsManager.AddOption(new CheckBoxOption(enableAggressiveHeatHaze));
            ModSettingsManager.AddOption(new CheckBoxOption(enableNonAggressiveHeatHaze));

            ModSettingsManager.AddOption(new StepSliderOption(
                masochismSurgeHealOnHitPercentage,
                new StepSliderConfig
                {
                    min = 0.01f,
                    max = 1f,
                    increment = 0.01f
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

        public static void OnChangeHooks() 
        {
            arsonistVoicelineVolume.SettingChanged += ArsonistVoicelineVolume_Changed;
            arsonistVoicelineVolumeArsonist.SettingChanged += ArsonistVoicelineVolumeArsonist_SettingChanged;
            arsonistVoicelineVolumeFirebug.SettingChanged += ArsonistVoicelineVolumeFirebug_SettingChanged;
            arsonistSFXVolume.SettingChanged += ArsonistSFXVolume_SettingChanged;
        }

        private static void ArsonistSFXVolume_SettingChanged(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_ArsonistSFX", arsonistSFXVolume.Value);
            }
        }

        private static void ArsonistVoicelineVolumeFirebug_SettingChanged(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice_Firebug", arsonistVoicelineVolumeFirebug.Value);
            }
        }

        private static void ArsonistVoicelineVolumeArsonist_SettingChanged(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice_Arsonist", arsonistVoicelineVolumeArsonist.Value);
            }
        }

        private static void ArsonistVoicelineVolume_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice", arsonistVoicelineVolume.Value);
            }
        }
    }
}
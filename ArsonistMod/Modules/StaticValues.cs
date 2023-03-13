using System;
using UnityEngine;

namespace ArsonistMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Arsonist is heat.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
             //+ "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             //+ "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             //+ "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             //+ "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

        internal const float swordDamageCoefficient = 2.8f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 16f;

        //passive onfire buff
        internal static float igniteAttackSpeedMultiplier = 1.25f;
        internal static float igniteDamageReduction = 0.5f;
        internal static float igniteDamageMultiplier = 1.5f;
        internal static float igniteMovespeedMultiplier = 1.5f;
        internal static float overheatRegenMultiplier = 2f;
        internal static float secondaryCooldownMultiplier = 0.6f;
        internal static float utilityCooldownMultiplier = 0.33f;
        internal static float specialCooldownMultiplier = 0.6f;

        internal static int noOfSegmentsOnOverheatGauge = 200;
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
        internal static float masochismBuffDuration = 8f;
        internal static float masochismHealCoefficient = 0.05f;
        internal static float masochismEnergyCost = 50f;
    }
}
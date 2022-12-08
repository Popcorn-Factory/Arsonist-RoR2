using System;
using UnityEngine;

namespace ArsonistMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Arsonist is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

        internal const float swordDamageCoefficient = 2.8f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 16f;

        //passive onfire buff
        internal static float igniteDamageMultiplier = 1.5f;
        internal static float igniteMovespeedMultiplier = 1.5f;
        internal static float overheatRegenMultiplier = 2f;
        internal static float secondaryCooldownMultiplier = 0.6f;
        internal static float utilityCooldownMultiplier = 0.33f;
        internal static float specialCooldownMultiplier = 0.6f;

        internal static int noOfSegmentsOnOverheatGauge = 250;
        internal static Vector3 SegmentedValuesOnGaugeAlt = new Vector3(0.5f, 0.4f, 0.1f);
        internal static Vector3 SegmentedValuesOnGaugeMain = new Vector3(0.9f, 0f, 0.1f);


        //energy
        internal static float baseEnergy = 100f;
        internal static float levelEnergy = 5f;
        internal static float regenOverheatFraction = 0.05f;
        internal static float backupEnergyGain = 5f;
        internal static float hardlightEnergyGain = 15f;

        //firespray
        internal static float firesprayWeakDamageCoefficient = 1f;
        internal static float firesprayStrongDamageCoefficient = 3f;
        internal static float firesprayEnergyCost = 20f;
        //zeropointpunch
        internal static float zeropointpounchDamageCoefficient = 4f;
        //cleanse
        internal static float cleanseDuration = 4f;
        internal static float cleanseDamageCoefficient = 0.5f;

        //Masochism
        internal static float masochismBuffDuration = 30f;
        internal static float masochismEnergyCost = 40f;
    }
}
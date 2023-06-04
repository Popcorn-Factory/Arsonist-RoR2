using System;
using UnityEngine;

namespace ArsonistMod.Modules
{
    internal static class StaticValues
    {
        //passive 2.0
        internal static float passiveIgniteLength = 5.0f;

        //old passive onfire buff
        internal static float igniteAttackSpeedMultiplier = 1.25f;
        internal static float igniteDamageReduction = 0.5f;
        internal static float igniteDamageMultiplier = 1.5f;
        internal static float igniteMovespeedMultiplier = 1.5f;
        internal static float overheatRegenMultiplier = 2f;
        internal static float secondaryCooldownMultiplier = 0.6f;
        internal static float utilityCooldownMultiplier = 0.33f;
        internal static float specialCooldownMultiplier = 0.6f;
        internal static float overheatAttackSpeedDebuff = 0.5f;

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
        internal static float blueDamageMultiplier = 3f;
        internal static float lowerDamageMultiplier = 0.9f;

        //firespray
        internal static float firesprayWeakDamageCoefficient = 1.5f;
        internal static float firesprayStrongDamageCoefficient = 3.5f;
        internal static float firesprayEnergyCost = 8f;
        internal static float firesprayBlastRadius = 5f;
        internal static float firesprayweakBlastRadius = 2.5f;

        //Alt-firespray
        internal static float altFiresprayStrongDamageCoefficient = 2f;
        internal static float altFiresprayWeakDamageCoefficient = 0.5f;
        internal static float altFiresprayEnergyCost = 12f;

        //Flamethrower
        internal static float flamethrowerStrongDamageCoefficient = 0.9f;
        internal static float flamethrowerWeakDamageCoefficient = 0.6f;
        internal static float flamethrowerEnergyCost = 4.5f;
        internal static float flamethrowerFireChance = 10f; // number generated higher than this number guarantees fire damage. between 1-100
        internal static float flamethrowerRange = 25f;
        internal static float flamethrowerProcCoefficient = 0.5f;
        internal static float flamethowerRadius = 1f;
        internal static int flamethrowerBaseTickRate = 2; //per half sec

        //Alt-flamethrower
        internal static float altFlamethrowerStrongDamageCoefficient = 1f;
        internal static float altFlamethrowerWeakDamageCoefficient = 0.6f;
        internal static float altFlamethrowerEnergyCost = 3f;

        //flaregun
        internal static float flareWeakDamageCoefficient = 2f;
        internal static float flareStrongDamageCoefficient = 4f;
        internal static float flareBlastRadius = 6f;
        internal static float flareHeatReductionMultiplier = 0.3f;
        internal static float flareSpeedCoefficient = 200f;
        internal static int flareTickNum = 5;
        internal static float flareInterval = 0.5f;
        internal static float flareStrongChildDamageCoefficient = 2f;
        internal static float flareBlastRadiusChild = 3f;
        internal static float flareSalvoRadius = 30f;

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
        internal static float masochismBuffDuration = 6.5f;
        internal static float masochismHealCoefficient = 0.04f;
        internal static float masochismEnergyCost = 50f;

        // neo masochism
        internal static float masochismBasePulseTimer = 0.6f;
        internal static float masochismActiveLowerBoundHeat = 0.15f;
        internal static float masochismBasePulseSelfDamageTimer = 0.6f;
        internal static float masochismSelfDamage = 0.1f;
        internal static float masochismEnergyIncreaseOverTimePercentage = 0.08f;
        internal static float masochismPulseRadius = 10f;
        internal static float masochismPulseCoefficient = 0.5f;
        internal static float masochismDamageBoost = 3f;
        internal static float masochismFinalBlastCoefficient = 2f;
        internal static float masochismFinalBlastRadius = 12f;
        internal static float masochismDamageMultiplierPerStack = 1.5f;
        internal static float masochismMaxMultipliedRange = 3f;

        internal static float masochismMoveSpeedPenalty = 0.7f;
        internal static float masochismDamagePenalty = 0.7f;
    }
}
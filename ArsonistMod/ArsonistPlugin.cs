using BepInEx;
using ArsonistMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using ArsonistMod.Modules;
using ArsonistMod.Content.Controllers;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace ArsonistMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class ArsonistPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.PopcornFactory.Arsonist";
        public const string MODNAME = "Arsonist";
        public const string MODVERSION = "0.0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "POPCORN";

        public static ArsonistPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new Arsonist().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {

            if (self.healthComponent)
            {
                orig(self);

                if (self)
                {

                    if (self.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                    {
                        EnergySystem energySystem = self.gameObject.GetComponent<EnergySystem>();

                        //passive burn movespeed and damage
                        if (self.HasBuff(RoR2Content.Buffs.AffixRed))
                        {
                            self.damage *= StaticValues.igniteDamageMultiplier;
                            self.moveSpeed *= StaticValues.igniteMovespeedMultiplier;

                            energySystem.regenOverheat *= StaticValues.overheatRegenMultiplier;

                        }
                        else if (self.HasBuff(RoR2Content.Buffs.OnFire))
                        {
                            self.damage *= StaticValues.igniteDamageMultiplier;
                            self.moveSpeed *= StaticValues.igniteMovespeedMultiplier;

                            energySystem.regenOverheat *= StaticValues.overheatRegenMultiplier;
                        }

                        //cooldowns depending on being overheated or not
                        if (self.skillLocator.secondary.cooldownRemaining > 0 && energySystem.hasOverheatedSecondary)
                        {
                            self.skillLocator.secondary.cooldownScale *= 1f;
                        }
                        else if (self.skillLocator.secondary.cooldownRemaining > 0 && !energySystem.hasOverheatedSecondary)
                        {
                            self.skillLocator.secondary.cooldownScale *= StaticValues.secondaryCooldownMultiplier;
                        }

                        if (self.skillLocator.utility.cooldownRemaining > 0 && energySystem.hasOverheatedUtility)
                        {
                            self.skillLocator.utility.cooldownScale *= 1f;
                        }
                        else if (self.skillLocator.utility.cooldownRemaining > 0 && !energySystem.hasOverheatedUtility)
                        {
                            self.skillLocator.utility.cooldownScale *= StaticValues.utilityCooldownMultiplier;
                        }

                        if (self.skillLocator.special.cooldownRemaining > 0 && energySystem.hasOverheatedSpecial)
                        {
                            self.skillLocator.special.cooldownScale *= 1f;
                        }
                        else if (self.skillLocator.special.cooldownRemaining > 0 && !energySystem.hasOverheatedSpecial)
                        {
                            self.skillLocator.special.cooldownScale *= StaticValues.specialCooldownMultiplier;
                        }
                    }
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self) 
            {
                if (self.body) 
                {
                    if (self.body.HasBuff(Modules.Buffs.masochismBuff)) 
                    {
                        //If any of these are true, heal arsonist.
                        bool dotCheck = damageInfo.dotIndex == DotController.DotIndex.Burn ||
                                        damageInfo.dotIndex == DotController.DotIndex.Helfire ||
                                        damageInfo.dotIndex == DotController.DotIndex.StrongerBurn;

                        bool damageTypeCheck = damageInfo.damageType == DamageType.IgniteOnHit;

                        if (dotCheck || damageTypeCheck) 
                        {
                            float potentialDamage = damageInfo.damage;
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                            self.Heal(potentialDamage, damageInfo.procChainMask);
                        }
                    }
                }                    
            }

            orig(self, damageInfo);
        }
    }
}
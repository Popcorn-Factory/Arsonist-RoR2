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
using BepInEx.Bootstrap;
using R2API.Networking;
using ArsonistMod.Modules.Networking;
using EmotesAPI;
using R2API;
using ArsonistMod.SkillStates.Arsonist.Secondary;
using ArsonistMod.Modules.Networking;
using System;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace ArsonistMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.prefab", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.language", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.sound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.networking", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.unlockable", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.damagetype", BepInDependency.DependencyFlags.HardDependency)]

    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

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
            if (Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                Modules.Config.SetupRiskOfOptions();
            }
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules
            Modules.Damage.SetupModdedDamage(); //add modded damage types

            // survivor initialization
            new Arsonist().Initialize();

            //networking
            NetworkingAPI.RegisterMessageType<BurnNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PlaySoundNetworkRequest>();
            NetworkingAPI.RegisterMessageType<TakeDamageNetworkRequest>();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;

            if (Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }
        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            foreach (var item in SurvivorCatalog.allSurvivorDefs)
            {
                if (item.bodyPrefab.name == "ArsonistBody")
                {
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HumanoidArsonist"));
                }
            }
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

                        //boosts when in blue zone- double damage?
                        if (self.HasBuff(Buffs.blueBuff))
                        {
                            self.damage *= StaticValues.blueDamageMultiplier;


                        }

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
                    //If any of these are true, heal arsonist.
                    bool dotCheck = damageInfo.dotIndex == DotController.DotIndex.Burn ||
                                    damageInfo.dotIndex == DotController.DotIndex.Helfire ||
                                    damageInfo.dotIndex == DotController.DotIndex.StrongerBurn;

                    bool damageTypeCheck = damageInfo.damageType == DamageType.IgniteOnHit;
                    EnergySystem energySystem = self.GetComponent<EnergySystem>();


                    if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistStickyDamageType))
                    {
                        FlareEffectControllerStrong flarecon = self.body.gameObject.AddComponent<FlareEffectControllerStrong>();
                        flarecon.arsonistBody = damageInfo.attacker.GetComponent<CharacterBody>();
                        flarecon.charbody = self.body;
                    }
                    else if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistWeakStickyDamageType))
                    {
                        FlareEffectControllerWeak flarecon = self.body.gameObject.AddComponent<FlareEffectControllerWeak>();
                        flarecon.arsonistBody = damageInfo.attacker.GetComponent<CharacterBody>();
                        flarecon.charbody = self.body;
                    }



                    if (self.body.HasBuff(Modules.Buffs.masochismBuff) && (dotCheck || damageTypeCheck))
                    {
                        if (energySystem.currentOverheat >= energySystem.maxOverheat)
                        {
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                            self.Heal(0.5f * Modules.StaticValues.masochismHealCoefficient * self.body.maxHealth * Modules.Config.masochismHealthMultiplierOnPowered.Value, new ProcChainMask(), true);
                        }
                        else
                        {
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                            self.Heal(Modules.StaticValues.masochismHealCoefficient * self.body.maxHealth * Modules.Config.masochismHealthMultiplierOnPowered.Value, new ProcChainMask(), true);
                        }
                    }
                    else if (!self.body.HasBuff(Buffs.masochismBuff))
                    {
                        if (self.body.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                        {
                            if (damageInfo.damage > 0f)
                            {
                                if(dotCheck || damageTypeCheck)
                                {
                                    damageInfo.damage *= StaticValues.igniteDamageReduction;
                                }
                            } 
                            
                        }
                    }
                }                    
            }

            orig(self, damageInfo);
        }

        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);

            if (self)
            {
                if (self.body)
                {
                    ArsonistController arsonistController = self.body.GetComponent<ArsonistController>();
                    if (arsonistController)
                    {
                        this.overlayFunction(Modules.Assets.arsonistOverheatingMaterial, (bool)arsonistController, self);
                    }
                }
            }
        }

        private void overlayFunction(Material overlayMaterial, bool condition, CharacterModel model)
        {
            if (model.activeOverlayCount >= CharacterModel.maxOverlays)
            {
                return;
            }
            if (condition)
            {
                Material[] array = model.currentOverlays;
                int num = model.activeOverlayCount;
                model.activeOverlayCount = num + 1;
                array[num] = overlayMaterial;
            }
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("ArsonistDisplay"))
            {
                ChildLocator childLocator = self.gameObject.GetComponent<ChildLocator>();
                Transform thumb = childLocator.FindChild("DisplayThumb");
            }
        }
    }
}
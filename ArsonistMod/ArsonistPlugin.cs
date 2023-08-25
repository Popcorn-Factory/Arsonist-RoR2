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
using System;
using R2API.Networking.Interfaces;

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
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("HIFU.Inferno", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    public class ArsonistPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.PopcornFactory.Arsonist";
        public const string MODNAME = "Arsonist";
        public const string MODVERSION = "2.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "POPCORN";

        public static bool starstormAvailable = false;
        public static bool infernoAvailable = false;
        public static ArsonistPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Config.ReadConfig();
            Modules.Assets.Initialize(); // load assets and read config
            if (Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                Modules.Config.SetupRiskOfOptions();
            }
            if (Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.Starstorm2")) 
            {
                starstormAvailable = true;
            }
            if (Chainloader.PluginInfos.ContainsKey("HIFU.Inferno"))
            {
                infernoAvailable = true;
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
            NetworkRequestSetup();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void NetworkRequestSetup() 
        {
            NetworkingAPI.RegisterMessageType<BurnNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PlaySoundNetworkRequest>();
            NetworkingAPI.RegisterMessageType<TakeDamageNetworkRequest>();
            NetworkingAPI.RegisterMessageType<AttachFlareNetworkRequest>();
            NetworkingAPI.RegisterMessageType<FlamethrowerDotNetworkRequest>();
            NetworkingAPI.RegisterMessageType<ToggleMasochismEffectNetworkRequest>();
            NetworkingAPI.RegisterMessageType<KillAllEffectsNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PlayCleanseBlastNetworkRequest>();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterModel.Start += CharacterModel_Start;
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            if (Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }

        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            // A lot of issues with effects and all. Let's get rid of all of the stuff when this character dies.
            if (self.baseNameToken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
            {
                //Stop all sounds, Stop all Particle effects and stop masochism sphere.
                new PlaySoundNetworkRequest(self.netId, (uint)2176930590).Send(NetworkDestination.Clients);
                new KillAllEffectsNetworkRequest(self.netId, true).Send(NetworkDestination.Clients);
                new ToggleMasochismEffectNetworkRequest(self.netId, false).Send(NetworkDestination.Clients);
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
                        if (self.HasBuff(Buffs.lowerBuff))
                        {
                            self.damage *= StaticValues.lowerDamageMultiplier;
                        }

                        if (self.HasBuff(Buffs.cleanseSpeedBoost)) 
                        {
                            self.moveSpeed += 5f;
                        }

                        #region Old Passive
                        //passive burn movespeed and damage
                        //if (self.HasBuff(RoR2Content.Buffs.AffixRed))
                        //{
                        //    self.damage *= StaticValues.igniteDamageMultiplier;
                        //    self.moveSpeed *= StaticValues.igniteMovespeedMultiplier;

                        //    energySystem.regenOverheat *= StaticValues.overheatRegenMultiplier;

                        //}
                        //else if (self.HasBuff(RoR2Content.Buffs.OnFire))
                        //{
                        //    self.damage *= StaticValues.igniteDamageMultiplier;
                        //    self.moveSpeed *= StaticValues.igniteMovespeedMultiplier;
                        //    energySystem.regenOverheat *= StaticValues.overheatRegenMultiplier;
                        //}
                        #endregion

                        if (self.HasBuff(Modules.Buffs.masochismBuff)) 
                        {
                            self.attackSpeed *= StaticValues.igniteAttackSpeedMultiplier;
                        }

                        if (self.HasBuff(Modules.Buffs.overheatDebuff)) 
                        {
                            if (!(self.HasBuff(Modules.Buffs.masochismDeactivatedDebuff) || self.HasBuff(Modules.Buffs.masochismDeactivatedNonDebuff))) 
                            {
                                self.attackSpeed *= StaticValues.overheatAttackSpeedDebuff;
                            }
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

                    //Debug.Log($"{DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistStickyDamageType)} {DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistWeakStickyDamageType)} {DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistChildExplosionDamageType)}");


                    if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistStickyDamageType))
                    {
                        new AttachFlareNetworkRequest(self.body.netId, damageInfo.attacker.GetComponent<CharacterBody>().netId, AttachFlareNetworkRequest.FlareType.STRONG).Send(NetworkDestination.Clients);
                    }
                    else if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistWeakStickyDamageType))
                    {
                        new AttachFlareNetworkRequest(self.body.netId, damageInfo.attacker.GetComponent<CharacterBody>().netId, AttachFlareNetworkRequest.FlareType.WEAK).Send(NetworkDestination.Clients);
                    }
                    else if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.arsonistChildExplosionDamageType)) 
                    {
                        new AttachFlareNetworkRequest(self.body.netId, damageInfo.attacker.GetComponent<CharacterBody>().netId, AttachFlareNetworkRequest.FlareType.CHILD_STRONG).Send(NetworkDestination.Clients);
                    }

                    DamageType tempDamageType = DamageType.FallDamage | DamageType.NonLethal;
                    DamageType frailtyDamageType = DamageType.FallDamage | DamageType.BypassOneShotProtection;
                    if (self.body.HasBuff(Modules.Buffs.fallDamageReductionBuff) && (damageInfo.damageType == tempDamageType || damageInfo.damageType == frailtyDamageType )) 
                    {
                        if (damageInfo.damage >= 20f) 
                        {
                            damageInfo.damage = 20f;
                        }
                    }

                    //if (!Modules.Config.enableOldLoadout.Value)
                    //{
                    //    //Default to using 2.0 passive.
                    //}
                    //else 
                    //{
                    //    // Use either passive depending on what's selected.

                    //}

                    #region 2.0 Passive
                    //Receive damage, check if damage is not fire.
                    if (self.body.baseNameToken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                    {
                        if (damageInfo.damage > self.fullHealth * Modules.Config.passiveHealthPercentageTriggerIgnite.Value && (damageInfo.damageType == frailtyDamageType)) 
                        {
                            if (!dotCheck)
                            {
                                //Half incoming damage
                                damageInfo.damage *= Modules.StaticValues.igniteDamageReduction;

                                //Inflict the rest of the damage as a dot.
                                InflictDotInfo info = new InflictDotInfo();
                                info.totalDamage = damageInfo.damage;
                                info.attackerObject = self.body.gameObject;
                                info.victimObject = self.body.gameObject;
                                info.duration = Modules.StaticValues.passiveIgniteLength;
                                info.dotIndex = DotController.DotIndex.Burn;

                                DotController.InflictDot(ref info);
                            }

                            if (damageInfo.damage > 0f)
                            {
                                if (dotCheck || damageTypeCheck)
                                {
                                    damageInfo.damage *= StaticValues.igniteDamageReduction;
                                }
                            }
                        }
                    }
                    #endregion


                    #region Old Passive and Masochism
                    /* Old Passive/Masochism
                    
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
                     */
                    #endregion
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

        private void CharacterModel_Start(On.RoR2.CharacterModel.orig_Start orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("ArsonistDisplay"))
            {
                ArsonistDisplayController displayHandler = self.gameObject.GetComponent<ArsonistDisplayController>();
                if (!displayHandler)
                {
                    displayHandler = self.gameObject.AddComponent<ArsonistDisplayController>();
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
    }
}
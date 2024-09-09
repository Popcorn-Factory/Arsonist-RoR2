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
using EntityStates.GravekeeperBoss;
using UnityEngine.Networking;
using static RoR2.DotController;

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
        public const string MODUID = "com.PopcornFactory.Arsonist";
        public const string MODNAME = "Arsonist";
        public const string MODVERSION = "2.2.1";

        public const string DEVELOPER_PREFIX = "POPCORN";

        public static bool starstormAvailable = false;
        public static bool infernoAvailable = false;
        public static ArsonistPlugin instance;
        public static AkGameObj akGameObject;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Config.ReadConfig();
            Modules.Config.OnChangeHooks();
            Modules.AssetsArsonist.Initialize(); // load assets and read config
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
            On.RoR2.DotController.InflictDot_refInflictDotInfo += DotController_InflictDot_refInflictDotInfo; ;
            
            //Item voice lines
            //On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            //On.RoR2.Inventory.SetEquipmentIndexForSlot += Inventory_SetEquipmentIndexForSlot;

            ////Mithrix Voice lines
            //On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += BrotherEncounter_Phase1_OnEnter;
            //On.EntityStates.Missions.BrotherEncounter.BossDeath.OnEnter += BrotherEncounter_BossDeath_OnEnter;

            ////Voidling Voice lines
            //On.EntityStates.VoidRaidCrab.DeathState.OnEnter += VoidRaidCrab_DeathState_OnEnter;
            ////On.RoR2.ScriptedCombatEncounter.BeginEncounter += ScriptedCombatEncounter_BeginEncounter;

            //Teleporter lines
            //On.RoR2.TeleporterInteraction.IdleState.OnInteractionBegin += IdleState_OnInteractionBegin;

            if (Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }

        private void DotController_InflictDot_refInflictDotInfo(On.RoR2.DotController.orig_InflictDot_refInflictDotInfo orig, ref InflictDotInfo inflictDotInfo)
        {
            if (inflictDotInfo.victimObject)
            {
                CharacterBody body = inflictDotInfo.victimObject.GetComponent<CharacterBody>();

                if (body)
                {
                    if (body.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                    {
                        //If any of these are true, heal arsonist.
                        bool dotCheck = inflictDotInfo.dotIndex == DotController.DotIndex.Burn ||
                                        inflictDotInfo.dotIndex == DotController.DotIndex.Helfire ||
                                        inflictDotInfo.dotIndex == DotController.DotIndex.StrongerBurn;

                        if (dotCheck)
                        {
                            inflictDotInfo.totalDamage *= StaticValues.igniteDamageReduction;
                        }
                    }
                }
            }

            orig(ref inflictDotInfo);
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
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, Modules.AssetsArsonist.mainAssetBundle.LoadAsset<GameObject>("HumanoidArsonist"));
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
                        if (damageInfo.damage > self.fullHealth * Modules.Config.passiveHealthPercentageTriggerIgnite.Value && (damageInfo.damageType != tempDamageType) && !self.body.HasBuff(DLC1Content.Buffs.BearVoidReady)) 
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
                        this.overlayFunction(arsonistController.overheatingMaterial, (bool)arsonistController && self.body.hasEffectiveAuthority, self);
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

        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            if (NetworkServer.active) 
            {
                if (self) 
                {
                    if (self.GetComponent<CharacterMaster>()) 
                    {
                        CharacterBody body = self.GetComponent<CharacterMaster>().GetBody();
                        if (body)
                        {
                            if (body.baseNameToken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                            {
                                string nameToken = RoR2.ItemCatalog.GetItemDef(itemIndex).nameToken;
                                switch (nameToken)
                                {
                                    case "ITEM_IGNITEONKILL_NAME":
                                        break;
                                    case "ITEM_EXPLODEONDEATH_NAME":
                                        break;
                                    case "ITEM_STRENGTHENBURN_NAME":
                                        break;
                                    case "ITEM_ICERING_NAME":
                                        break;
                                    case "ITEM_FIRERING_NAME":
                                        break;
                                    case "ITEM_EXPLODEONDEATHVOID_NAME":
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Inventory_SetEquipmentIndexForSlot(On.RoR2.Inventory.orig_SetEquipmentIndexForSlot orig, Inventory self, EquipmentIndex newEquipmentIndex, uint slot)
        {
            orig(self, newEquipmentIndex, slot);
            if (NetworkServer.active)
            {
                if (self) 
                {
                    if (self.GetComponent<CharacterMaster>()) 
                    {
                        CharacterBody body = self.gameObject.GetComponent<CharacterMaster>().GetBody();
                        if (body)
                        {
                            if (body.baseNameToken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                            {
                                string nameToken = EquipmentCatalog.GetEquipmentDef(newEquipmentIndex).nameToken;

                                if (nameToken == "EQUIPMENT_BURNNEARBY_NAME")
                                {
                                    //Do things.
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BrotherEncounter_BossDeath_OnEnter(On.EntityStates.Missions.BrotherEncounter.BossDeath.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.BossDeath self)
        {
            orig(self);
            //Get All network users
            foreach (NetworkUser user in NetworkUser.instancesList) 
            {
                if (user.master) 
                {
                    if (user.master.GetBody()) 
                    {
                        string bodytoken = user.master.GetBody().baseNameToken;
                        
                        if (bodytoken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                        {
                            //play voiceline.
                            //Chat.AddMessage("You are dead, not big souprice");
                        }
                    }
                }
            }
        }

        private void BrotherEncounter_Phase1_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase1.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase1 self)
        {
            orig(self);
            //Get All network users
            foreach (NetworkUser user in NetworkUser.instancesList)
            {
                if (user.master) 
                {
                    if (user.master.GetBody()) 
                    {
                        string bodytoken = user.master.GetBody().baseNameToken;
                        if (bodytoken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                        {
                            //play voiceline.
                            //Chat.AddMessage("You are alive, not big souprice");
                        }
                    }
                }
            }
        }


        private void VoidRaidCrab_DeathState_OnEnter(On.EntityStates.VoidRaidCrab.DeathState.orig_OnEnter orig, EntityStates.VoidRaidCrab.DeathState self)
        {
            orig(self);
            //Get All network users
            if (NetworkServer.active) 
            {
                foreach (NetworkUser user in NetworkUser.instancesList)
                {
                    if (user.master)
                    {
                        if (user.master.GetBody())
                        {
                            string bodytoken = user.master.GetBody().baseNameToken;
                            if (bodytoken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                            {
                                //play voiceline.
                                //Chat.AddMessage("You are dead, not big souprice");
                            }
                        }
                    }
                }
            }
        }

        private void ScriptedCombatEncounter_BeginEncounter(On.RoR2.ScriptedCombatEncounter.orig_BeginEncounter orig, ScriptedCombatEncounter self)
        {
            orig(self);

            //foreach (RoR2.ScriptedCombatEncounter.SpawnInfo info in self.spawns) 
            //{
            //    Chat.AddMessage(info.spawnCard.prefab.GetComponent<CharacterBody>().baseNameToken);
            //}
            //Get All network users
            if (NetworkServer.active)
            {
                foreach (NetworkUser user in NetworkUser.instancesList)
                {
                    if (user.master)
                    {
                        if (user.master.GetBody())
                        {
                            string bodytoken = user.master.GetBody().baseNameToken;
                            if (bodytoken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                            {
                                //play voiceline.
                                //Chat.AddMessage("You are alive, not big souprice");
                            }
                        }
                    }
                }
            }
        }

        private void IdleState_OnInteractionBegin(On.RoR2.TeleporterInteraction.IdleState.orig_OnInteractionBegin orig, EntityStates.BaseState self, Interactor activator)
        {
            orig(self, activator);
            //Get All network users
            if (NetworkServer.active)
            {
                foreach (NetworkUser user in NetworkUser.instancesList)
                {
                    if (user.master)
                    {
                        if (user.master.GetBody())
                        {
                            string bodytoken = user.master.GetBody().baseNameToken;
                            if (bodytoken == DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                            {
                                //play voiceline.
                                //Chat.AddMessage("You are activated, not big souprice");
                            }
                        }
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
    }
}
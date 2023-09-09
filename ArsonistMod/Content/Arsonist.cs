using BepInEx.Configuration;
using ArsonistMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using ArsonistMod.Content.Controllers;

namespace ArsonistMod.Modules.Survivors
{
    internal class Arsonist : SurvivorBase
    {
        public override string bodyName => "Arsonist";

        public const string ARSONIST_PREFIX = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ARSONIST_PREFIX;

        public static int FirebugSkinIndex = 2;
         
        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "ArsonistBody",
            bodyNameToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME",
            subtitleNameToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("ArsonistIcon"),
            bodyColor = Color.red,

            crosshair = Modules.Assets.fireballCrosshair,
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthGrowth = 40f,
            healthRegen = 1f,
            moveSpeed = 7f,
            damage = 12f,
            armor = 20f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
                new CustomRendererInfo
                {
                    childName = "ArsonistBody",
                    material = Materials.CreateHopooMaterial("matArsonist", false),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistArmor",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistBoots",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistCanister",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistChestplate",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistHead",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "ArsonistWeapon",
                    material = Materials.CreateHopooMaterial("matArsonistMetal", true),
                },
                new CustomRendererInfo
                {
                    childName = "Cylinder",
                },
                new CustomRendererInfo
                {
                    childName = "PyroRobe",
                    material = Materials.CreateHopooMaterial("matArsonistCloth", true),
                }
        };

        public override UnlockableDef characterUnlockableDef => arsonistUnlockable;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new ArsonistItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef pigSkinUnlockableDef;
        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef firebugSkinUnlockableDef;
        private static UnlockableDef flamethrowerUnlockableDef;
        private static UnlockableDef arsonistUnlockable; 

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
            bodyPrefab.AddComponent<EnergySystem>();
            bodyPrefab.AddComponent<ArsonistController>();

            //Modify the material on the fireblast
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            Transform fireBlastEffectTransform = childLocator.FindChild("FireBeam");
            Transform fireBlastFowardTransform = childLocator.FindChild("FireBeamForwardFiring");
            ParticleSystemRenderer particleSystemRenderer = fireBlastEffectTransform.GetChild(0).GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer particleSystemRenderer2 = fireBlastFowardTransform.GetChild(0).GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();

            particleSystemRenderer.material = Assets.emissionRingMatLesser;
            particleSystemRenderer2.material = Assets.emissionRingMatLesser;
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>(true);
            firebugSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.ArsonistEclipse5Achievement>(true);
            arsonistUnlockable = Modules.Unlockables.AddUnlockable<Modules.Achievements.ArsonistUnlockable>(true);
            //pigSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.ArsonistEclipse8Achievement>(true);
            flamethrowerUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.ArsonistFlamethrowerUnlockable>(true);
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            Transform hitboxTransform = childLocator.FindChild("ZeroPointHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "ZeroPoint");
        }

        public override void InitializeSkills()
        {
            ArsonistPassive passive = bodyPrefab.AddComponent<ArsonistPassive>(); 
            Modules.Skills.CreateSkillFamilies(bodyPrefab, true);
            string prefix = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_";

            //if (!Modules.Config.enableOldLoadout.Value)
            //{
            //    SkillLocator skillloc = bodyPrefab.GetComponent<SkillLocator>();
            //    skillloc.passiveSkill.enabled = true;
            //    skillloc.passiveSkill.skillNameToken = prefix + "PASSIVE_NAME";
            //    skillloc.passiveSkill.skillDescriptionToken = prefix + "PASSIVE_DESCRIPTION";
            //    skillloc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon");
            //    skillloc.passiveSkill.keywordToken = prefix + "KEYWORD_PASSIVE";
            //}

            #region Passive
            SkillLocator skillLoc = bodyPrefab.GetComponent<SkillLocator>();
            skillLoc.passiveSkill.enabled = true;
            skillLoc.passiveSkill.skillNameToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_PASSIVE_NAME";
            skillLoc.passiveSkill.skillDescriptionToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_PASSIVE_DESCRIPTION";
            skillLoc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon");

            passive.normalGaugePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PASSIVE_NORMAL_GAUGE_NAME",
                skillNameToken = prefix + "PASSIVE_NORMAL_GAUGE_NAME",
                skillDescriptionToken = prefix + "PASSIVE_NORMAL_GAUGE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistNormalGauge"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireSpray)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            passive.blueGaugePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PASSIVE_BLUE_GAUGE_NAME",
                skillNameToken = prefix + "PASSIVE_BLUE_GAUGE_NAME",
                skillDescriptionToken = prefix + "PASSIVE_BLUE_GAUGE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistBlueGauge"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireSpray)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddPassiveSkills(passive.passiveSkillSlot.skillFamily, new SkillDef[]{
                passive.normalGaugePassive,
                passive.blueGaugePassive
            });

            //if (Modules.Config.enableOldLoadout.Value) 
            //{
            //    passive.movespeedOnFirePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //    {
            //        skillName = prefix + "SECRET_PASSIVE_MOVESPEED_ON_FIRE_NAME",
            //        skillNameToken = prefix + "SECRET_PASSIVE_MOVESPEED_ON_FIRE_NAME",
            //        skillDescriptionToken = prefix + "SECRET_PASSIVE_MOVESPEED_ON_FIRE_DESCRIPTION",
            //        skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            //        activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireSpray)),
            //        activationStateMachineName = "Slide",
            //        baseMaxStock = 1,
            //        baseRechargeInterval = 0f,
            //        beginSkillCooldownOnSkillEnd = false,
            //        canceledFromSprinting = false,
            //        forceSprintDuringState = false,
            //        fullRestockOnAssign = true,
            //        interruptPriority = EntityStates.InterruptPriority.Skill,
            //        resetCooldownTimerOnUse = false,
            //        isCombatSkill = true,
            //        mustKeyPress = true,
            //        cancelSprintingOnActivation = false,
            //        rechargeStock = 1,
            //        requiredStock = 1,
            //        stockToConsume = 1
            //    });
            //    passive.halfDamagePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //    {
            //        skillName = prefix + "PASSIVE_HALF_DAMAGE_NAME",
            //        skillNameToken = prefix + "PASSIVE_HALF_DAMAGE_NAME",
            //        skillDescriptionToken = prefix + "PASSIVE_HALF_DAMAGE_DESCRIPTION",
            //        skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            //        activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireSpray)),
            //        activationStateMachineName = "Slide",
            //        baseMaxStock = 1,
            //        baseRechargeInterval = 0f,
            //        beginSkillCooldownOnSkillEnd = false,
            //        canceledFromSprinting = false,
            //        forceSprintDuringState = false,
            //        fullRestockOnAssign = true,
            //        interruptPriority = EntityStates.InterruptPriority.Skill,
            //        resetCooldownTimerOnUse = false,
            //        isCombatSkill = true,
            //        mustKeyPress = true,
            //        cancelSprintingOnActivation = false,
            //        rechargeStock = 1,
            //        requiredStock = 1,
            //        stockToConsume = 1
            //    });

            //    Modules.Skills.AddPassiveSkills(passive.secondaryPassiveSkillSlot.skillFamily, new SkillDef[] {
            //        passive.movespeedOnFirePassive,
            //        passive.halfDamagePassive
            //    });
            //}


            #endregion

            #region Primary

            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PRIMARY_FIRESPRAY_NAME",
                skillNameToken = prefix + "PRIMARY_FIRESPRAY_NAME",
                skillDescriptionToken = prefix + "PRIMARY_FIRESPRAY_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireSpray)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] {prefix + "KEYWORD_BASEGAUGE" , prefix + "KEYWORD_FIRESPRAYHEAT" }
            });


            SkillDef flamethrower = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PRIMARY_FLAMETHROWER_NAME",
                skillNameToken = prefix + "PRIMARY_FLAMETHROWER_NAME",
                skillDescriptionToken = prefix + "PRIMARY_FLAMETHROWER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistFlamethrower"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Flamethrower)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Skills.AddPrimarySkills(this.bodyPrefab, new SkillDef[]
            {
                primarySkillDef,
                flamethrower
            });

            Skills.AddUnlockablesToFamily(this.bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, new UnlockableDef[] 
            { 
                null, 
                flamethrowerUnlockableDef 
            });
            #endregion

            #region Secondary
            SkillDef flareSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SECONDARY_FLAREGUN_NAME",
                skillNameToken = prefix + "SECONDARY_FLAREGUN_NAME",
                skillDescriptionToken = prefix + "SECONDARY_FLAREGUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Flare)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 14f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] {prefix + "KEYWORD_FLAREHEAT" }
            });

            SkillDef punchSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SECONDARY_PUNCH_NAME",
                skillNameToken = prefix + "SECONDARY_PUNCH_NAME",
                skillDescriptionToken = prefix + "SECONDARY_PUNCH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistZPBIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZeroPointBlast.ZeroPointBlastStart)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 11f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" , prefix + "KEYWORD_ZEROPOINTHEAT" }
            });

            Skills.AddSecondarySkills(this.bodyPrefab, new SkillDef[]
            {
                flareSkillDef,
                punchSkillDef,
            });
            #endregion

            #region Utility
            SkillDef cleanseSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "UTILITY_CLEANSE_NAME",
                skillNameToken = prefix + "UTILITY_CLEANSE_NAME",
                skillDescriptionToken = prefix + "UTILITY_CLEANSE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Cleanse)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 18f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Vehicle,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", prefix + "KEYWORD_CLEANSEHEAT" }
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, cleanseSkillDef);
            #endregion

            #region Special
            SkillDef masochistSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SPECIAL_MASOCHIST_NAME",
                skillNameToken = prefix + "SPECIAL_MASOCHIST_NAME",
                skillDescriptionToken = prefix + "SPECIAL_MASOCHIST_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Masochism)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,    
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false, 
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", prefix + "KEYWORD_MASOCHISMHEAT" }
            });

            SkillDef neoMasochismSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SPECIAL_MASOCHISM_NAME",
                skillNameToken = prefix + "SPECIAL_MASOCHISM_NAME",
                skillDescriptionToken = prefix + "SPECIAL_MASOCHISM_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.NeoMasochism)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] {
                    prefix + "KEYWORD_MASO_ANTICIPATION", 
                    prefix + "KEYWORD_MASO_LIFESTEAL", 
                    prefix + "KEYWORD_MASO_DETONATE", 
                    prefix + "KEYWORD_OVERHEAT_MASO"}
            });

            //Modules.Skills.AddSpecialSkills(bodyPrefab, masochistSkillDef);
            Modules.Skills.AddSpecialSkills(bodyPrefab, neoMasochismSkillDef);
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            Material matArsonist = Modules.Materials.CreateHopooMaterial("matArsonist", false);
            Material matArsonistMetal = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            Material matArsonistCloth = Modules.Materials.CreateHopooMaterial("matArsonistCloth", true);
            CharacterModel.RendererInfo[] arsonistRendererInfos = SkinRendererInfos(defaultRendererinfos, new Material[] {
                matArsonist,
                matArsonistMetal,
                matArsonistMetal,
                matArsonistMetal,
                matArsonistMetal,
                matArsonistMetal,
                matArsonistMetal,
                null,
                matArsonistCloth
            });
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(ARSONIST_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                arsonistRendererInfos,
                model);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshArsonist",
                "meshArsonistArmor",
                "meshArsonistBoots",
                "meshArsonistCanister",
                "meshArsonistChestplate",
                "meshArsonistHead",
                "meshArsonistWeapon",
                "meshCylinder",
                "meshPyroRobe"
                );

            defaultSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonist", false);
            defaultSkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            defaultSkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            defaultSkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            defaultSkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            defaultSkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            defaultSkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistMetal", true);
            //defaultSkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistAlt", false);
            defaultSkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistCloth", true);

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            #region Suit Skin

            Material matSuitSkin = Modules.Materials.CreateHopooMaterial("matSuitArsonist", true);
            Material matSuitSkinMetal = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            CharacterModel.RendererInfo[] suitArsonistRendererInfos = SkinRendererInfos(defaultRendererinfos, new Material[] {
                matSuitSkin,
                matSuitSkin,
                matSuitSkinMetal,
                matSuitSkinMetal,
                matSuitSkinMetal,
                matSuitSkinMetal,
                matSuitSkinMetal,
                null,
                matSuitSkin
            });
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistMastery"),
                suitArsonistRendererInfos,
                model,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "SuitArsonist",
                "SuitArsonist", //no armour mesh replacement. use same armour mesh
                "SuitArsonistBoots",
                "SuitArsonistCanister",
                "SuitArsonistGloves",
                "SuitArsonistHead",
                "SuitArsonistWeapon",
                "meshCylinder",
                "SuitPyroRobe");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonist", true);
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonist", true);
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            masterySkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            masterySkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            masterySkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            masterySkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonistMetal", true);
            //masterySkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistAlt");
            masterySkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHopooMaterial("matSuitArsonist", true);

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            #endregion

            #region FirebugSkin

            Material matNeoArsonistMetal = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            Material matNeoArsonistCloth = Modules.Materials.CreateHopooMaterial("matNeoArsonistCloth", true);
            CharacterModel.RendererInfo[] neoArsonistRendererInfos = SkinRendererInfos(defaultRendererinfos, new Material[] {
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                matNeoArsonistMetal,
                null,
                matNeoArsonistCloth
            });
            //creating a new skindef as we did before
            SkinDef grandmasterySkin = Modules.Skins.CreateSkinDef(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_GRANDMASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistGrandmastery"),
                neoArsonistRendererInfos,
                model,
                firebugSkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            grandmasterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "NeoArsonist",
                "NeoArsonistArmor",//no gun mesh replacement. use same gun mesh
                "NeoArsonistBoots",
                "NeoArsonistCanister",
                "NeoArsonistChestplate",
                "NeoArsonistHead",
                "NeoArsonistWeapon",
                "meshCylinder",
                "NeoPyroRobe");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            grandmasterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            grandmasterySkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistMetal", true);
            //masterySkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHopooMaterial("matArsonistAlt");
            grandmasterySkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHopooMaterial("matNeoArsonistCloth", true);

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(grandmasterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {

            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            for (int i = 0; i < materials.Length; i++) 
            {
                newRendererInfos[i].defaultMaterial = materials[i];
            }

            //newRendererInfos[0].defaultMaterial = materials[0];
            //newRendererInfos[1].defaultMaterial = materials[1];
            //newRendererInfos[2].defaultMaterial = materials[2];
            //newRendererInfos[3].defaultMaterial = materials[3];
            //newRendererInfos[4].defaultMaterial = materials[4];
            //newRendererInfos[5].defaultMaterial = materials[5];
            //newRendererInfos[6].defaultMaterial = materials[6];
            //newRendererInfos[7].defaultMaterial = materials[7];
            //newRendererInfos[8].defaultMaterial = materials[8];


            return newRendererInfos;
        }
    }
}
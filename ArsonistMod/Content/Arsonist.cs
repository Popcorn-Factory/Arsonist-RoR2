using BepInEx.Configuration;
using ArsonistMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using ArsonistMod.Content.Controllers;
using R2API;
using RoR2.CharacterAI;

namespace ArsonistMod.Modules.Survivors
{
    internal class Arsonist : SurvivorBase
    {
        public override string bodyName => "Arsonist";

        public const string ARSONIST_PREFIX = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ARSONIST_PREFIX;

        public static int FirebugSkinIndex = 2;


        //SkillDefs
        public static SkillDef primarySkillDef;
        public static SkillDef flamethrowerSkillDef;
        public static SkillDef flareSkillDef;
        public static SkillDef punchSkillDef;
        public static SkillDef cleanseSkillDef;
        public static SkillDef neoMasochismSkillDef;
         
        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "ArsonistBody",
            bodyNameToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME",
            subtitleNameToken = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_SUBTITLE",

            characterPortrait = AssetsArsonist.mainAssetBundle.LoadAsset<Texture>("ArsonistIcon"),
            bodyColor = Color.red,

            crosshair = Modules.AssetsArsonist.fireballCrosshair,
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

        public override UnlockableDef characterUnlockableDef => Modules.Unlockables.characterUnlockableDef;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new ArsonistItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

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

            particleSystemRenderer.material = AssetsArsonist.emissionRingMatLesser;
            particleSystemRenderer2.material = AssetsArsonist.emissionRingMatLesser;
        }

        public override void InitializeUnlockables()
        {

        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            Transform hitboxTransform = childLocator.FindChild("ZeroPointHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "ZeroPoint");
        }

        public override void InitializeDoppelganger(string clone)
        {
            GameObject newMasterGameObject = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/" + clone + "MonsterMaster"), bodyName + "MonsterMaster", true);
            CharacterMaster master = newMasterGameObject.GetComponent<CharacterMaster>();
            master.bodyPrefab = bodyPrefab;

            //Set AI Skill Drivers

            AISkillDriver[] drivers = master.GetComponents<AISkillDriver>();
            foreach (AISkillDriver driver in drivers) 
            {
                UnityEngine.Object.DestroyImmediate(driver);
            }

            //Fire as much as possible in range.
            AISkillDriver flamethrower = master.gameObject.AddComponent<AISkillDriver>();
            flamethrower.customName = "Arsonist Primary";
            flamethrower.skillSlot = SkillSlot.Primary;
            flamethrower.requireSkillReady = true;
            flamethrower.requireEquipmentReady = false;
            flamethrower.minDistance = 15f;
            flamethrower.maxDistance = 40f;
            flamethrower.selectionRequiresAimTarget = true;
            flamethrower.selectionRequiresOnGround = false;
            flamethrower.selectionRequiresAimTarget = true;
            flamethrower.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            flamethrower.activationRequiresTargetLoS = true;
            flamethrower.activationRequiresAimTargetLoS = true;
            flamethrower.activationRequiresAimConfirmation = true;
            flamethrower.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            flamethrower.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            flamethrower.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            flamethrower.resetCurrentEnemyOnNextDriverSelection = false;

            //Always fire when at a distance away from the player.
            AISkillDriver flare = master.gameObject.AddComponent<AISkillDriver>();
            flare.customName = "Arsonist Secondary";
            flare.skillSlot = SkillSlot.Secondary;
            flare.requireSkillReady = true;
            flare.requireEquipmentReady = false;
            flare.minDistance = 30f;
            flare.maxDistance = 100f;
            flare.selectionRequiresAimTarget = true;
            flare.selectionRequiresOnGround = false;
            flare.selectionRequiresAimTarget = true;
            flare.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            flare.activationRequiresTargetLoS = true;
            flare.activationRequiresAimTargetLoS = true;
            flare.activationRequiresAimConfirmation = true;
            flare.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            flare.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            flare.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            flare.resetCurrentEnemyOnNextDriverSelection = true;
            flare.noRepeat = true;

            AISkillDriver cleanse = master.gameObject.AddComponent<AISkillDriver>();
            cleanse.customName = "Arsonist Cleanse";
            cleanse.skillSlot = SkillSlot.Utility;
            cleanse.requireSkillReady = true;
            cleanse.requireEquipmentReady = false;
            cleanse.minDistance = 0f;
            cleanse.maxDistance = 75f;
            cleanse.selectionRequiresAimTarget = true;
            cleanse.selectionRequiresOnGround = false;
            cleanse.selectionRequiresAimTarget = true;
            cleanse.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            cleanse.activationRequiresTargetLoS = true;
            cleanse.activationRequiresAimTargetLoS = true;
            cleanse.activationRequiresAimConfirmation = true;
            cleanse.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            cleanse.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            cleanse.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            cleanse.resetCurrentEnemyOnNextDriverSelection = false;
            cleanse.resetCurrentEnemyOnNextDriverSelection = true;
            cleanse.noRepeat = true;

            AISkillDriver maso = master.gameObject.AddComponent<AISkillDriver>();
            maso.customName = "Arsonist Masochism";
            maso.skillSlot = SkillSlot.Special;
            maso.requireSkillReady = true;
            maso.requireEquipmentReady = false;
            maso.minDistance = 10f;
            maso.maxDistance = 15f;
            maso.minTargetHealthFraction = 50f;
            maso.maxTargetHealthFraction = float.PositiveInfinity;
            maso.selectionRequiresAimTarget = true;
            maso.selectionRequiresOnGround = false;
            maso.selectionRequiresAimTarget = true;
            maso.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            maso.activationRequiresTargetLoS = true;
            maso.activationRequiresAimTargetLoS = true;
            maso.activationRequiresAimConfirmation = true;
            maso.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            maso.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            maso.resetCurrentEnemyOnNextDriverSelection = false;
            maso.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            maso.noRepeat = true;

            //Setup AI
            AISkillDriver flee = master.gameObject.AddComponent<AISkillDriver>();
            flee.customName = "Arsonist Flee if too close";
            flee.skillSlot = SkillSlot.None;
            flee.requireSkillReady = false;
            flee.requireEquipmentReady = false;
            flee.minDistance = 5f;
            flee.shouldSprint = true;
            flee.maxDistance = 20f;
            flee.minDistance = 0f;
            flee.selectionRequiresAimTarget = false;
            flee.selectionRequiresOnGround = false;
            flee.selectionRequiresAimTarget = false;
            flee.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            flee.moveInputScale = 1f;
            flee.activationRequiresTargetLoS = true;
            flee.activationRequiresAimTargetLoS = true;
            flee.activationRequiresAimConfirmation = true;
            flee.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            flee.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            flee.resetCurrentEnemyOnNextDriverSelection = true;

            AISkillDriver FIND = master.gameObject.AddComponent<AISkillDriver>();
            FIND.customName = "Arsonist Flee if too close";
            FIND.skillSlot = SkillSlot.None;
            FIND.requireSkillReady = false;
            FIND.requireEquipmentReady = false;
            FIND.minDistance = 0f;
            FIND.maxDistance = float.PositiveInfinity;
            FIND.shouldSprint = true;
            FIND.selectionRequiresAimTarget = false;
            FIND.selectionRequiresOnGround = false;
            FIND.selectionRequiresAimTarget = false;
            FIND.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            FIND.moveInputScale = 1f;
            FIND.activationRequiresTargetLoS = false;
            FIND.activationRequiresAimTargetLoS = false;
            FIND.activationRequiresAimConfirmation = false;
            FIND.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            FIND.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            FIND.resetCurrentEnemyOnNextDriverSelection = false;

            Modules.Content.AddMasterPrefab(newMasterGameObject);
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
            skillLoc.passiveSkill.icon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon");
            skillLoc.passiveSkill.keywordToken = prefix + "KEYWORD_PYROMANIAPASSIVE";

            passive.normalGaugePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PASSIVE_NORMAL_GAUGE_NAME",
                skillNameToken = prefix + "PASSIVE_NORMAL_GAUGE_NAME",
                skillDescriptionToken = prefix + "PASSIVE_NORMAL_GAUGE_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistNormalGauge"),
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
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "KEYWORD_BASEGAUGEPASSIVE" }
            });

            passive.blueGaugePassive = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PASSIVE_BLUE_GAUGE_NAME",
                skillNameToken = prefix + "PASSIVE_BLUE_GAUGE_NAME",
                skillDescriptionToken = prefix + "PASSIVE_BLUE_GAUGE_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistBlueGauge"),
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
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "KEYWORD_SUPERCRITICALPASSIVE" }
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

            primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PRIMARY_FIRESPRAY_NAME",
                skillNameToken = prefix + "PRIMARY_FIRESPRAY_NAME",
                skillDescriptionToken = prefix + "PRIMARY_FIRESPRAY_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
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
                keywordTokens = new string[] {prefix + "KEYWORD_BASEGAUGE" , prefix + "KEYWORD_PRIMARYHEAT" }
            });


            flamethrowerSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "PRIMARY_FLAMETHROWER_NAME",
                skillNameToken = prefix + "PRIMARY_FLAMETHROWER_NAME",
                skillDescriptionToken = prefix + "PRIMARY_FLAMETHROWER_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistFlamethrower"),
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
                keywordTokens = new string[] { prefix + "KEYWORD_PRIMARYHEAT" }
            });

            Skills.AddPrimarySkills(this.bodyPrefab, new SkillDef[]
            {
                primarySkillDef,
                flamethrowerSkillDef
            });

            Skills.AddUnlockablesToFamily(this.bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, new UnlockableDef[] 
            { 
                null, 
                Modules.Unlockables.flamethrowerUnlockableDef 
            });
            #endregion

            #region Secondary
            flareSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SECONDARY_FLAREGUN_NAME",
                skillNameToken = prefix + "SECONDARY_FLAREGUN_NAME",
                skillDescriptionToken = prefix + "SECONDARY_FLAREGUN_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
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
                keywordTokens = new string[] {prefix + "KEYWORD_FLAREHEAT", prefix + "KEYWORD_FLARENOTE" }
            });

            punchSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SECONDARY_PUNCH_NAME",
                skillNameToken = prefix + "SECONDARY_PUNCH_NAME",
                skillDescriptionToken = prefix + "SECONDARY_PUNCH_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistZPBIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZeroPointBlast.ZeroPointBlastStart)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 9f,
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
            cleanseSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "UTILITY_CLEANSE_NAME",
                skillNameToken = prefix + "UTILITY_CLEANSE_NAME",
                skillDescriptionToken = prefix + "UTILITY_CLEANSE_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
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
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
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

            neoMasochismSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SPECIAL_MASOCHISM_NAME",
                skillNameToken = prefix + "SPECIAL_MASOCHISM_NAME",
                skillDescriptionToken = prefix + "SPECIAL_MASOCHISM_DESCRIPTION",
                skillIcon = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
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
                AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
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
                AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistMastery"),
                suitArsonistRendererInfos,
                model,
                Modules.Unlockables.masteryUnlockableDef);

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
                AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistGrandmastery"),
                neoArsonistRendererInfos,
                model,
                Modules.Unlockables.eclipse5UnlockableDef);

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

            return newRendererInfos;
        }
    }
}
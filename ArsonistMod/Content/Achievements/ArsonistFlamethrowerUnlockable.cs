using ArsonistMod.Content.Controllers;
using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT",
    ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_REWARD_ID", null, null)]
    internal class ArsonistFlamethrowerUnlockable : ModdedUnlockable
    {
        public CharacterBody body;

        public override string AchievementIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string UnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_REWARD_ID";

        public override string AchievementNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_UNLOCKABLE_NAME";

        public override string UnlockableNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_UNLOCKABLE_NAME";

        public override string AchievementDescToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_DESC";

        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistFlamethrower");
        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override string PrerequisiteUnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_UNLOCKABLE_REWARD_ID";
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        public override void OnInstall() 
        {
            base.OnInstall();
            SceneExitController.onBeginExit += this.onSceneExit;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
        }

        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);
            if (!body) 
            {
                //Get player controlled Arsonist. There should only be one.
                if (self.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME" && self.hasEffectiveAuthority) 
                {
                    body = self;
                }
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            SceneExitController.onBeginExit -= this.onSceneExit;
            On.RoR2.CharacterBody.FixedUpdate -= CharacterBody_FixedUpdate;
        }

        private void onSceneExit(SceneExitController obj)
        {
            if (body) 
            {
                EnergySystem energy = body.GetComponent<EnergySystem>();
                if (this.meetsBodyRequirement && energy)
                {
                    if (!energy.hasOverheatedThisStage) 
                    {
                        base.Grant();
                    }   
                }
            }
        }
    }
}
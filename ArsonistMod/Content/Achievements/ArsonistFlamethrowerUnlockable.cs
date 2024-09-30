using ArsonistMod.Content.Controllers;
using RoR2;
using RoR2.Achievements;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(identifier, unlockableidentifier, prerequisiteAchievementIdentifier, 5)]
    internal class ArsonistFlamethrowerUnlockable : BaseAchievement
    {
        public CharacterBody body;

        public const string identifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_ACHIEVEMENT";
        public const string unlockableidentifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_FLAMETHROWERUNLOCKABLE_REWARD_ID";
        public const string prerequisiteAchievementIdentifier = ArsonistUnlockable.identifier;

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
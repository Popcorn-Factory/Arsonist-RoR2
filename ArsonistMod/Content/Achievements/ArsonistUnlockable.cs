using RoR2;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT",
    ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_REWARD_ID", null, null)]
    internal class ArsonistUnlockable : ModdedUnlockable
    {

        public CharacterBody body;
        public override string AchievementIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_ID";

        public override string UnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_REWARD_ID";

        public override string AchievementNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_UNLOCKABLE_NAME";

        public override string UnlockableNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_UNLOCKABLE_NAME";

        public override string AchievementDescToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_DESC";

        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ArsonistIconUnlock");
        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override string PrerequisiteUnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_UNLOCKABLE_REWARD_ID";
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            if (self) 
            {
                if (self.isChampion && 
                    (self.HasBuff(RoR2Content.Buffs.OnFire) || self.HasBuff(RoR2.DLC1Content.Buffs.StrongerBurn))
                    )
                {
                    //Has died, and has the OnFire debuff? sounds good to me. Grant it!
                    base.Grant();
                }
            }
            orig(self);
        }

        public override void OnGranted()
        {
            base.OnGranted();
            if (body) 
            {
                Util.PlaySound("Arsonist_Masochism_Laugh", body.gameObject);
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            On.RoR2.CharacterBody.Start += CharacterBody_Start;
        }

        private void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig(self);
            if (self) 
            {
                if (self.isPlayerControlled) 
                {
                    if (!body) 
                    {
                        body = self;
                    }  
                }
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterBody.OnDeathStart -= CharacterBody_OnDeathStart;
            On.RoR2.CharacterBody.Start -= CharacterBody_Start;
        }
    }
}
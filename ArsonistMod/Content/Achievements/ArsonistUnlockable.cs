using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(identifier, unlockableidentifier, null, 10)]
    internal class ArsonistUnlockable : BaseAchievement
    {

        public CharacterBody body;
        public const string identifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT";
        public const string unlockableidentifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT_ID";


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
using ArsonistMod.Modules.Achievements;
using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Modules
{
    internal class Unlockables
    {
        public static UnlockableDef characterUnlockableDef;
        public static UnlockableDef flamethrowerUnlockableDef;
        public static UnlockableDef masteryUnlockableDef;
        public static UnlockableDef eclipse5UnlockableDef;
        public static UnlockableDef eclipse8UnlockableDef;

        public static void Initialize() 
        {
            characterUnlockableDef = Modules.ContentPacks.CreateAndAddUnlockbleDef(
                ArsonistUnlockable.unlockableidentifier,
                Modules.Helpers.GetAchievementNameToken(ArsonistUnlockable.identifier),
                Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("ArsonistIconUnlock")
            );

            flamethrowerUnlockableDef = Modules.ContentPacks.CreateAndAddUnlockbleDef(
                ArsonistFlamethrowerUnlockable.unlockableidentifier,
                Modules.Helpers.GetAchievementNameToken(ArsonistFlamethrowerUnlockable.identifier),
                Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistFlamethrower")
            );

            masteryUnlockableDef = Modules.ContentPacks.CreateAndAddUnlockbleDef(
                MasteryAchievement.unlockableidentifier,
                Modules.Helpers.GetAchievementNameToken(MasteryAchievement.identifier),
                Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistMastery")
            );

            eclipse5UnlockableDef = Modules.ContentPacks.CreateAndAddUnlockbleDef(
                ArsonistEclipse5Achievement.unlockableidentifier,
                Modules.Helpers.GetAchievementNameToken(ArsonistEclipse5Achievement.identifier),
                Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistGrandmastery")
            );

            //eclipse8UnlockableDef = Modules.ContentPacks.CreateAndAddUnlockbleDef(
            //    ArsonistEclipse8Achievement.identifier,
            //    Modules.Helpers.GetAchievementNameToken(ArsonistEclipse8Achievement.identifier),
            //    Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistInferno")
            //);
        }
    }
}

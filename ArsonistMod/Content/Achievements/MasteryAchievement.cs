using RoR2;
using System;
using System.Data.Common;
using UnityEngine;
using static ArsonistMod.Modules.BaseMasteryUnlockable;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(identifier, unlockableidentifier, prerequisiteAchievementIdentifier, 10)]
    internal class MasteryAchievement: BaseMasteryUnlockable
    {
        public const string identifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT";
        public const string unlockableidentifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string prerequisiteAchievementIdentifier = ArsonistUnlockable.identifier;

        public override string RequiredCharacterBody => Modules.Survivors.Arsonist.instance.fullBodyName;

        public override float RequiredDifficultyCoefficient => 3;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

    }
}
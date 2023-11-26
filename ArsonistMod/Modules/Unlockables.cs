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
        private static readonly HashSet<string> usedRewardIds = new HashSet<string>();
        internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();
        private static readonly List<(AchievementDef achDef, UnlockableDef unlockableDef, String unlockableName)> moddedUnlocks = new List<(AchievementDef achDef, UnlockableDef unlockableDef, string unlockableName)>();

        public static bool ableToAdd { get; private set; } = false;

        internal static UnlockableDef CreateNewUnlockable(UnlockableInfo unlockableInfo)
        {
            UnlockableDef newUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();

            newUnlockableDef.nameToken = unlockableInfo.Name;
            newUnlockableDef.cachedName = unlockableInfo.Name;
            newUnlockableDef.getHowToUnlockString = unlockableInfo.HowToUnlockString;
            newUnlockableDef.getUnlockedString = unlockableInfo.UnlockedString;
            newUnlockableDef.sortScore = unlockableInfo.SortScore;
            newUnlockableDef.achievementIcon = unlockableInfo.sprite;

            return newUnlockableDef;
        }

        public static UnlockableDef AddUnlockable<TUnlockable>(bool serverTracked) where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
        {
            TUnlockable instance = new TUnlockable();

            string unlockableIdentifier = instance.UnlockableIdentifier;

            if (!usedRewardIds.Add(unlockableIdentifier)) throw new InvalidOperationException($"The unlockable identifier '{unlockableIdentifier}' is already used by another mod or the base game.");

            UnlockableDef unlockableDef = CreateNewUnlockable(new UnlockableInfo
            {
                Name = instance.UnlockableIdentifier,
                HowToUnlockString = instance.GetHowToUnlock,
                UnlockedString = instance.GetUnlocked,
                SortScore = 200,
                sprite = instance.Sprite
            });

            unlockableDefs.Add(unlockableDef);

            return unlockableDef;
        }
        internal struct UnlockableInfo
        {
            internal string Name;
            internal Func<string> HowToUnlockString;
            internal Func<string> UnlockedString;
            internal int SortScore;
            internal Sprite sprite;
        }

        internal interface IModdedUnlockableDataProvider
        {
            string AchievementIdentifier { get; }
            string UnlockableIdentifier { get; }
            string AchievementNameToken { get; }
            string PrerequisiteUnlockableIdentifier { get; }
            string UnlockableNameToken { get; }
            string AchievementDescToken { get; }
            Sprite Sprite { get; }
            Func<string> GetHowToUnlock { get; }
            Func<string> GetUnlocked { get; }
        }

        public abstract class ModdedUnlockable : BaseAchievement, IModdedUnlockableDataProvider
        {

            #region Implementation
            public void Revoke()
            {
                if (base.userProfile.HasAchievement(this.AchievementIdentifier))
                {
                    base.userProfile.RevokeAchievement(this.AchievementIdentifier);
                }

                base.userProfile.RevokeUnlockable(UnlockableCatalog.GetUnlockableDef(this.UnlockableIdentifier));
            }
            #endregion

            #region Contract
            public abstract string AchievementIdentifier { get; }
            public abstract string UnlockableIdentifier { get; }
            public abstract string AchievementNameToken { get; }
            public abstract string PrerequisiteUnlockableIdentifier { get; }
            public abstract string UnlockableNameToken { get; }
            public abstract string AchievementDescToken { get; }
            public abstract Sprite Sprite { get; }
            public abstract Func<string> GetHowToUnlock { get; }
            public abstract Func<string> GetUnlocked { get; }
            #endregion

            #region Virtuals
            public override void OnGranted() => base.OnGranted();
            public override void OnInstall()
            {
                base.OnInstall();
            }
            public override void OnUninstall()
            {
                base.OnUninstall();
            }
            public override Single ProgressForAchievement() => base.ProgressForAchievement();
            public override BodyIndex LookUpRequiredBodyIndex()
            {
                return base.LookUpRequiredBodyIndex();
            }
            public override void OnBodyRequirementBroken() => base.OnBodyRequirementBroken();
            public override void OnBodyRequirementMet() => base.OnBodyRequirementMet();
            public override bool wantsBodyCallbacks { get => base.wantsBodyCallbacks; }
            #endregion
        }
    }
}

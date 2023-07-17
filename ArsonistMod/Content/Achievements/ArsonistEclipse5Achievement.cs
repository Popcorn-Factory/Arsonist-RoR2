using RoR2;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT",
    ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_REWARD_ID", null, null)]
    internal class ArsonistEclipse5Achievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_ID";

        public override string UnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_REWARD_ID";

        public override string AchievementNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_UNLOCKABLE_NAME";

        public override string UnlockableNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_UNLOCKABLE_NAME";

        public override string AchievementDescToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_DESC";

        public override Sprite Sprite => Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("arsonistMastery");
        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override string PrerequisiteUnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_UNLOCKABLE_REWARD_ID";
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        public bool TyphoonCheckFunc(RunReport runReport) 
        {
            DifficultyIndex typhoonIndex = Moonstorm.Starstorm2.Typhoon.TyphoonIndex;
            return typhoonIndex == runReport.ruleBook.FindDifficulty();
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                bool eclipseDifficultyCheck = (difficultyDef.nameToken == "ECLIPSE_5_NAME")
                    || (difficultyDef.nameToken == "ECLIPSE_6_NAME")
                    || (difficultyDef.nameToken == "ECLIPSE_7_NAME")
                    || (difficultyDef.nameToken == "ECLIPSE_8_NAME");

                bool typhoonCheck = false;
                if (ArsonistPlugin.starstormAvailable) 
                {
                    typhoonCheck = TyphoonCheckFunc(runReport);
                }

                bool difficultyCheck = eclipseDifficultyCheck || typhoonCheck;

                if (difficultyDef != null && difficultyCheck)
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            Run.onClientGameOverGlobal += this.ClearCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            Run.onClientGameOverGlobal -= this.ClearCheck;
        }
    }
}
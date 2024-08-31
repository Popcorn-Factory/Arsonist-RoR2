using RoR2;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT",
        ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_REWARD_ID", 
        ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ARSONISTUNLOCKABLE_ACHIEVEMENT", 0)]
    internal class ArsonistEclipse8Achievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_ID";

        public override string UnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_REWARD_ID";

        public override string AchievementNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_UNLOCKABLE_NAME";

        public override string UnlockableNameToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_UNLOCKABLE_NAME";

        public override string AchievementDescToken { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_DESC";

        public override Sprite Sprite => Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("arsonistInferno");
        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override string PrerequisiteUnlockableIdentifier { get; } = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_UNLOCKABLE_REWARD_ID";
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        public bool InfernoCheckFunc(RunReport runReport) 
        {
            DifficultyIndex infernoIndex = Inferno.Main.InfernoDiffIndex;
            return infernoIndex == runReport.ruleBook.FindDifficulty();
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                bool eclipseDifficultyCheck = (difficultyDef.nameToken == "ECLIPSE_8_NAME");

                bool infernoCheck = false;
                if (ArsonistPlugin.infernoAvailable) 
                {
                    infernoCheck = InfernoCheckFunc(runReport);
                }

                bool difficultyCheck = eclipseDifficultyCheck || infernoCheck;

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
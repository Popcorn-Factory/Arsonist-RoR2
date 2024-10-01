using RoR2.Achievements;
using RoR2;
using UnityEngine;

namespace ArsonistMod.Modules.Achievements
{
    [RegisterAchievement(identifier, unlockableidentifier, prerequisiteAchievementIdentifier, 20)]
    internal class ArsonistEclipse5Achievement : BaseAchievement
    {
        public const string identifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_ACHIEVEMENT";
        public const string unlockableidentifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE5UNLOCKABLE_REWARD_ID";
        public const string prerequisiteAchievementIdentifier = ArsonistUnlockable.identifier;
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        public bool TyphoonCheckFunc(RunReport runReport) 
        {
            DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
            return difficultyDef.nameToken == "SS2_DIFFICULTY_TYPHOON_NAME";
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

                bool typhoonCheck = TyphoonCheckFunc(runReport);

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
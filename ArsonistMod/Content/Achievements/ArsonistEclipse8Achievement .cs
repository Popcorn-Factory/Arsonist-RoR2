using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;
using static ArsonistMod.Modules.Unlockables;

namespace ArsonistMod.Modules.Achievements
{
    //[RegisterAchievement(identifier, unlockableidentifier, prerequisiteAchievementIdentifier, 100)]
    internal class ArsonistEclipse8Achievement : BaseAchievement
    {
        public const string identifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_ACHIEVEMENT";
        public const string unlockableidentifier = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_ECLIPSE8UNLOCKABLE_REWARD_ID";
        public const string prerequisiteAchievementIdentifier = ArsonistUnlockable.identifier;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Arsonist.instance.fullBodyName);
        }

        public bool InfernoCheckFunc(RunReport runReport) 
        {
            DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
            return difficultyDef.nameToken == "SS2_DIFFICULTY_TYPHOON_NAME"; //CHANGE TO HIFU's one.
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

                infernoCheck = InfernoCheckFunc(runReport);

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
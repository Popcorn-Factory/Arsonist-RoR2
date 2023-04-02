using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistPassive : MonoBehaviour
    {
        public SkillDef normalGaugePassive;
        public SkillDef blueGaugePassive;
        public GenericSkill passiveSkillSlot;

        public bool isBlueGauge()
        {
            //Debug.Log($"passiveSkillSlot: {passiveSkillSlot.skillDef.skillNameToken}");
            //Debug.Log($"energyPassive: {energyPassive.skillNameToken}");
            //Debug.Log($"normal: {normalCooldownPassive.skillNameToken}");

            if (blueGaugePassive && this.passiveSkillSlot)
            {
                return this.passiveSkillSlot.skillDef == blueGaugePassive;
            }

            return false;
        }
    }
}

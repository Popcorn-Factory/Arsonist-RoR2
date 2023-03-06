using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastEnd : BaseSkillState
    {
        //Final blast.
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

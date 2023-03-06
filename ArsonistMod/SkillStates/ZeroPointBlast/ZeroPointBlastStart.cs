using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastStart : BaseSkillState
    {
        //From blast to moving.
        //On hit send to blast end stage.
        //If expired before blast end, whiff end.
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

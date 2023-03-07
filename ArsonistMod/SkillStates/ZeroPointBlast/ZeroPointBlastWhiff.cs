using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastWhiff : BaseSkillState
    {
        public float stopwatch;
        public float duration;
        public static float baseDuration = 0.6f;
        public static float earlyExitFrac = 0.48f;
        
        //Whiff anim and end.
        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            duration = baseDuration;
            base.PlayCrossfade("FullBody, Override", "ZPBWhiff", 0.02f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= duration) 
            {
                base.outer.SetNextStateToMain();
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if(this.stopwatch >= duration * earlyExitFrac) 
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Frozen;
        }
    }
}

using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastEnd : BaseSkillState
    {
        public float stopwatch;
        public float duration;
        public static float baseDuration = 0.6f;
        public static float earlyExitFrac = 0.48f;
        //Final blast.
        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            duration = baseDuration;
            base.PlayCrossfade("FullBody, Override", "ZPBHit", 0.02f);
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
            if (this.stopwatch >= duration * earlyExitFrac)
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Frozen;
        }
    }
}

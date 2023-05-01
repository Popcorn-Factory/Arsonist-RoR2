using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates
{
    internal class NeoMasochism : BaseSkillState
    {
        public MasochismController maso;
        public float stopwatch;
        public static float baseActivationTime = 0.4f;
        public static float baseDuration = 1f;
        public float duration;
        public override void OnEnter()
        {
            base.OnEnter();
            maso = gameObject.GetComponent<MasochismController>();
            duration = baseDuration;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= duration * baseActivationTime && base.isAuthority) 
            {
                if (maso && !maso.masochismActive)
                {
                    maso.ActivateMaso();
                }
                else
                {
                    maso.TriggerMasochismAndEXOverheat(false);
                }

                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

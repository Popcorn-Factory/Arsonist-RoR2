using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates
{
    internal class NeoMasochism : BaseSkillState
    {
        public MasochismController maso;
        public float stopwatch;
        public static float baseActivationTime = 0.4f;
        public static float baseDuration = 1f;
        public override void OnEnter()
        {
            base.OnEnter();
            maso = gameObject.GetComponent<MasochismController>();
            
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (maso) 
            {
                maso.ActivateMaso();
            }
            base.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

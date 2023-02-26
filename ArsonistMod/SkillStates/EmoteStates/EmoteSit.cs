using System;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ArsonistMod.SkillStates.EmoteStates
{
    public class EmoteSit : BaseSkillState
    {
        public float stopwatch;
        public override void OnEnter()
        {
            base.OnEnter();
            //Play the animation instantly, no scaling, nothing.
            PlayEmoteAnim();
            stopwatch = 0f;
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
        }

        public override void Update()
        {
            base.Update();
            stopwatch += Time.deltaTime;

            if (base.isAuthority) 
            {
                if (ShouldEndEmoteState())
                {
                    base.outer.SetNextStateToMain();
                }
            }
            //Do not exit state automatically.
        }

        public bool ShouldEndEmoteState()
        {
            return !isGrounded || inputBank.skill1.down || inputBank.skill2.down || inputBank.skill3.down || inputBank.skill4.down;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void PlayEmoteAnim()
        {
            base.PlayAnimation("FullBody, Override", "Emote1");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}


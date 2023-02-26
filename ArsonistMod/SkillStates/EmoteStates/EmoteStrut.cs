using System;
using EntityStates;
using RoR2;

namespace ArsonistMod.SkillStates.EmoteStates
{
    public class EmoteStrut: BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            //Play the animation instantly, no scaling, nothing.
            PlayEmoteAnim();
        }

        public override void OnExit()
        {
            base.OnExit();
            base.PlayAnimation("FullBody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Do not exit state automatically.
            if (base.isAuthority)
            {
                if (ShouldEndEmoteState())
                {
                    base.outer.SetNextStateToMain();
                }
            }
        }

        public bool ShouldEndEmoteState()
        {
            return !isGrounded || inputBank.skill1.down || inputBank.skill2.down || inputBank.skill3.down || inputBank.skill4.down;
        }


        public void PlayEmoteAnim()
        {
            base.PlayAnimation("FullBody, Override", "Emote2");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}


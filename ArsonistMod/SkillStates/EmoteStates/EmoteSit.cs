using System;
using EntityStates;
using RoR2;

namespace ArsonistMod.SkillStates.EmoteStates
{
    public class EmoteSit : BaseEmoteState
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
        }

        public override void PlayEmoteAnim()
        {
            base.PlayAnimation("FullBody, Override", "Emote1");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}


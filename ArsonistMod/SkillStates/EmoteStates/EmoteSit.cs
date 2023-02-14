using System;
using EntityStates;

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

            base.PlayAnimation("Fullbody, Override", "BufferEmpty");
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


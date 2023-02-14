using System;
using EntityStates;

namespace ArsonistMod.SkillStates.EmoteStates
{
    public class BaseEmoteState : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            //Play the animation instantly, no scaling, nothing.
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

        public virtual void PlayEmoteAnim()
        {
            //Play animation
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}


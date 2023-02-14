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

        public override void Update()
        {
            base.Update();

            //Player jumped
            if (ShouldEndEmoteState())
            {
                base.outer.SetNextStateToMain();
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Do not exit state automatically.
        }

        public virtual bool ShouldEndEmoteState() 
        {
            return !isGrounded || inputBank.skill1.down || inputBank.skill2.down || inputBank.skill3.down || inputBank.skill4.down;
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


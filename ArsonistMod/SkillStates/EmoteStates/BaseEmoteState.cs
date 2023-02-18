using System;
using EntityStates;
using RoR2;

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

            if (base.isAuthority)
            {
                //We can execute states now.

                //Check for input down.
                if (Modules.Config.emoteSitKey.Value.IsPressed())
                {
                    //Chat.AddMessage("sit");
                }
                else if (Modules.Config.emoteStrutKey.Value.IsPressed())
                {
                    //Chat.AddMessage("strut");
                }
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


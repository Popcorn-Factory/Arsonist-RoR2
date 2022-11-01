using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates.Arsonist
{
    internal class Masochism : BaseSkillState
    {
        //Play animation, give buff after x amount of seconds
        internal static float giveBuffFraction = 0.3f;
        internal static float baseDuration = 0.5f;
        internal float duration;
        internal bool buffGiven;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            buffGiven = false;
        
        }

        public override void OnExit()
        {
            base.OnExit();
            //I dunno, just in case.
            if (!buffGiven) 
            {
                buffGiven = true;
                if (NetworkServer.active) 
                {
                    base.characterBody.AddTimedBuff(Modules.Buffs.masochismBuff.buffIndex, Modules.StaticValues.masochismBuffDuration);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //All states are executed on every client, we can just check to NetworkServer to determine what to do.
            if (fixedAge > duration * giveBuffFraction) 
            {
                if (!buffGiven) 
                {
                    buffGiven = true;
                    if (NetworkServer.active)
                    {
                        base.characterBody.AddTimedBuff(Modules.Buffs.masochismBuff.buffIndex, Modules.StaticValues.masochismBuffDuration);
                    }
                }
            }
            if (fixedAge > duration) 
            {
                this.outer.SetNextStateToMain();
            }
        
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

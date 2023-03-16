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
    internal class Masochism : BaseSkillState
    {
        //Play animation, give buff after x amount of seconds
        internal static float giveBuffFraction = 0.3f;
        internal static float baseDuration = 0.5f;
        internal float duration;
        internal bool buffGiven;
        internal EnergySystem energySystem;

        public float Energy = Modules.StaticValues.masochismEnergyCost;
        private float energyCost;
        private float energyflatCost;

        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = gameObject.GetComponent<EnergySystem>();
            duration = baseDuration / attackSpeedStat;
            buffGiven = false;

            //energy
            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                energySystem.hasOverheatedSpecial = false;
                energySystem.currentOverheat += energyCost;
            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && isAuthority)
            {
                //Nothing
            }

            if (base.isAuthority) 
            {
                if (!Modules.Config.shouldHaveVoice.Value)
                {
                    new PlaySoundNetworkRequest(characterBody.netId, 1840962711).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else 
                {
                    //Determine if they have a buff and play a non-laughing version if so.
                    bool buffCondition = (characterBody.HasBuff(Modules.Buffs.masochismBuff) || characterBody.HasBuff(Modules.Buffs.fallDamageReductionBuff));
                    uint soundStr = buffCondition ? 1840962711 : 3831238989; //Nonlaugh : laugh
                    new PlaySoundNetworkRequest(characterBody.netId, soundStr).Send(R2API.Networking.NetworkDestination.Clients);
                }
            }
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
                    characterBody.AddTimedBuff(Modules.Buffs.masochismBuff.buffIndex, Modules.StaticValues.masochismBuffDuration);
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
                        characterBody.AddTimedBuff(Modules.Buffs.masochismBuff.buffIndex, Modules.StaticValues.masochismBuffDuration);
                    }
                }
            }
            if (fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

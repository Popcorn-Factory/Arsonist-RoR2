using EntityStates;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ArsonistMod.Content.Controllers;
using static UnityEngine.ParticleSystem.PlaybackState;
using HG;
using System.Collections.Generic;
using System.Linq;

namespace ArsonistMod.SkillStates
{
    public class Flare : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 1f;
        public float duration;

        public float Energy = Modules.StaticValues.flareEnergyCost;
        private float energyCost;
        private float energyflatCost;
        private float speedOverride = Modules.StaticValues.flareSpeedCoefficient;


        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration / attackSpeedStat;

            base.characterBody.SetAimTimer(this.duration);


            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                ProjectileManager.instance.FireProjectile(Modules.Projectiles.strongFlare,
                           aimRay.origin + aimRay.direction,
                           Util.QuaternionSafeLookRotation(aimRay.direction),
                           base.gameObject,
                           1f,
                           0f,
                           base.RollCrit(),
                           DamageColorIndex.Default,
                           null,
                           speedOverride);
                energySystem.currentOverheat += energyCost;
            }
            else
            {
                ProjectileManager.instance.FireProjectile(Modules.Projectiles.weakFlare,
                           aimRay.origin + aimRay.direction,
                           Util.QuaternionSafeLookRotation(aimRay.direction),
                           base.gameObject,
                           1f,
                           0f,
                           base.RollCrit(),
                           DamageColorIndex.Default,
                           null,
                           speedOverride);
            }
        }
               

        public override void OnExit()
        {
            base.OnExit();
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
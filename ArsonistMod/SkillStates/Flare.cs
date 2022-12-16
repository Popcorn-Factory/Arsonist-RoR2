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
using ArsonistMod.Modules;

namespace ArsonistMod.SkillStates
{
    public class Flare : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 0f;
        public float duration;



        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration;

            base.characterBody.SetAimTimer(this.duration);

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                ProjectileManager.instance.FireProjectile(Modules.Projectiles.strongFlare,
                           aimRay.origin,
                           Util.QuaternionSafeLookRotation(aimRay.direction),
                           base.gameObject,
                           StaticValues.flareGunStrongDamageCoefficient * this.damageStat,
                           0f,
                           base.RollCrit(),
                           DamageColorIndex.Default,
                           null,
                           80f);
                energySystem.currentOverheat += 20f;
            }
            else
            {
                ProjectileManager.instance.FireProjectile(Modules.Projectiles.weakFlare,
                           aimRay.origin,
                           Util.QuaternionSafeLookRotation(aimRay.direction),
                           base.gameObject,
                           StaticValues.flareGunWeakDamageCoefficient * this.damageStat,
                           0f,
                           base.RollCrit(),
                           DamageColorIndex.Default,
                           null,
                           40f);
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
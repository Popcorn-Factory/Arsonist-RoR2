using EntityStates;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ArsonistMod.Content.Controllers;

namespace ArsonistMod.SkillStates
{
    public class FireSpray : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 0.5f;
        public float duration;

        public static GameObject effectPrefab;

        private string muzzleString;
        private Animator animator;
        private float damageCoefficient = Modules.StaticValues.firesprayWeakDamageCoefficient;
        private float strongdamageCoefficient = Modules.StaticValues.firesprayStrongDamageCoefficient;
        private float force = 400f;
        private float strongforce = 1000f;
        private float speedOverride = 100f;
        private float strongspeedOverride = 100f;
        private GameObject chargeVfxInstance;

        public float Energy = Modules.StaticValues.firesprayEnergyCost;
        private float energyCost;
        private float energyflatCost;

        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration / attackSpeedStat;

            base.characterBody.SetAimTimer(this.duration);
            //this.muzzleString = "LHand";

            this.animator = base.GetModelAnimator();
            //this.animator.SetBool("attacking", true);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //PlayCrossfade("LeftArm, Override", "LeftArmOut", "Attack.playbackRate", duration / 2, 0.1f);
            //PlayCrossfade("LeftArm, Override", "LeftArmPunch", "Attack.playbackRate", duration/2, 0.1f);

            //energy
            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && base.isAuthority)
            {
                FireBolt();
                energySystem.currentOverheat += Modules.StaticValues.firesprayEnergyCost;
            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && base.isAuthority)
            {
                FireBall();
            }

        }
        public void FireBall()
        {
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.lemurianFireBall, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    base.gameObject, //owner
                    this.damageStat * damageCoefficient, //damage
                    force, //force
                    Util.CheckRoll(this.critStat, base.characterBody.master), //crit
                    DamageColorIndex.Default, //damage color
                    null, //target
                    speedOverride); //speed }

            }

        }
        public void FireBolt()
        {
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.artificerFirebolt, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    base.gameObject, //owner
                    this.damageStat * strongdamageCoefficient, //damage
                    strongforce, //force
                    Util.CheckRoll(this.critStat, base.characterBody.master), //crit
                    DamageColorIndex.WeakPoint, //damage color
                    null, //target
                    strongspeedOverride); //speed }

            }

        }

        public override void OnExit()
        {
            base.OnExit();
            this.animator.SetBool("false", true);
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            if (this.chargeVfxInstance)
            {
                EntityState.Destroy(this.chargeVfxInstance);
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority && base.IsKeyDownAuthority())
            {
                this.outer.SetNextState(new FireSpray());
            }
            else if (base.fixedAge >= this.duration && base.isAuthority)
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
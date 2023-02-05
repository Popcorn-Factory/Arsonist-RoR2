using EntityStates;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace ArsonistMod.SkillStates
{
    public class FireSpray : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 0.5f;
        public float duration;

        public static GameObject effectPrefab;

        private string muzzleString = "GunMuzzle";
        private Transform muzzlePos;
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


            Ray aimRay = GetAimRay();
            duration = baseDuration / attackSpeedStat;

            characterBody.SetAimTimer(duration);
            //this.muzzleString = "LHand";

            animator = GetModelAnimator();
            this.animator.SetBool("attacking", true);
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("Gesture, Override", "FireSpray", "Attack.playbackRate", duration, 0.1f);

            //Get MuzzlePos
            ChildLocator childLoc = GetModelChildLocator();
            muzzlePos = childLoc.FindChild(muzzleString);

            //energy
            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                FireBolt();
                energySystem.currentOverheat += energyCost;

                new PlaySoundNetworkRequest(base.characterBody.netId, 470984906).Send(R2API.Networking.NetworkDestination.Clients);
            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && isAuthority)
            {
                FireBall();

                new PlaySoundNetworkRequest(base.characterBody.netId, 2300744954).Send(R2API.Networking.NetworkDestination.Clients);
            }

        }
        public void FireBall()
        {
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.lemurianFireBall, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat * damageCoefficient, //damage
                    force, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.Default, //damage color
                    null, //target
                    speedOverride); //speed }

            }

        }
        public void FireBolt()
        {
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.artificerFirebolt, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat * strongdamageCoefficient, //damage
                    strongforce, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.WeakPoint, //damage color
                    null, //target
                    strongspeedOverride); //speed }

            }

        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("attacking", false);
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayAnimation("Gesture, Override", "BufferEmpty");
            if (chargeVfxInstance)
            {
                Destroy(chargeVfxInstance);
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority && IsKeyDownAuthority())
            {
                outer.SetNextState(new FireSpray());
            }
            else if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
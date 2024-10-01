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
    public class FireSprayScepter : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 0.5f;
        public float duration;

        public static GameObject effectPrefab;

        private string muzzleString = "GunMuzzle";
        private Transform muzzlePos;
        private Animator animator;
        private float damageCoefficient = Modules.StaticValues.firesprayWeakDamageCoefficient;
        private float altDamageCoefficient = Modules.StaticValues.altFiresprayWeakDamageCoefficient;
        private float strongdamageCoefficient = Modules.StaticValues.firesprayStrongDamageCoefficient;
        private float altStrongDamageCoefficient = Modules.StaticValues.altFiresprayStrongDamageCoefficient;
        public static float spreadBloomValue = 10f;
        private float force = 400f;
        private float strongforce = 1000f;
        private float speedOverride = 100f;
        private float strongspeedOverride = 100f;
        private GameObject chargeVfxInstance;
        private ArsonistPassive passive;
        private bool isBlue;

        public float Energy = Modules.StaticValues.firesprayEnergyCost;
        private float energyCost;
        private float energyflatCost;


        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();
            passive = characterBody.gameObject.GetComponent<ArsonistPassive>();
            isBlue = passive.isBlueGauge();
            characterBody.isSprinting = false;

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
            energyflatCost = Energy;
            if (energyflatCost < 0f) energyflatCost = 1f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 1f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                FireBolt();
                energySystem.AddHeat(energyCost);

                new PlaySoundNetworkRequest(base.characterBody.netId, 470984906).Send(R2API.Networking.NetworkDestination.Clients);
            }
            else if (energySystem.currentOverheat >= energySystem.maxOverheat && base.isAuthority)
            {
                FireBall();

                new PlaySoundNetworkRequest(base.characterBody.netId, 2300744954).Send(R2API.Networking.NetworkDestination.Clients);
            }
            base.characterBody.AddSpreadBloom(spreadBloomValue);
        }    
        public void FireBall()
        {
            Ray aimRay = GetAimRay();
            float coeff = isBlue ? altDamageCoefficient : damageCoefficient;
            Vector3 origin = GetDisplacedOrigin(aimRay);
            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.lemurianFireBall, //prefab
                    origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat * coeff, //damage
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
            float coeff = isBlue ? altStrongDamageCoefficient : strongdamageCoefficient;
            Vector3 origin = GetDisplacedOrigin(aimRay);
            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.artificerFirebolt, //prefab
                    origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat * coeff, //damage
                    strongforce, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.Default, //damage color
                    null, //target
                    strongspeedOverride); //speed }

            }

        }

        public Vector3 GetDisplacedOrigin(Ray aimRay) 
        {
            float displacement = 2.5f;
            float closerDisplacement = 1.25f;
            return CheckLookingDown() ? aimRay.origin + displacement * (aimRay.direction.normalized) : aimRay.origin + closerDisplacement * (aimRay.direction.normalized);
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

        //Fireball collides with player, let's move it down underneath the player if they're looking down. 
        private bool CheckLookingDown()
        {
            if (Vector3.Dot(base.GetAimRay().direction, Vector3.down) > 0.6f)
            {
                return true;
            }
            return false;
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
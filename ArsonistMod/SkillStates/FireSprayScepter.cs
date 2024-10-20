using EntityStates;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using R2API.Networking.Interfaces;
using System;

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
        private float damageCoefficient = Modules.StaticValues.firesprayScepterWeakDamageCoefficient;
        private float altDamageCoefficient = Modules.StaticValues.altFiresprayScepterWeakDamageCoefficient;
        private float strongdamageCoefficient = Modules.StaticValues.firesprayScepterStrongDamageCoefficient;
        private float altStrongDamageCoefficient = Modules.StaticValues.altFiresprayScepterStrongDamageCoefficient;
        private float range = Modules.StaticValues.firesprayScepterRange;
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

        private bool isCharged = false;


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

                new PlaySoundNetworkRequest(base.characterBody.netId, 4235059291).Send(R2API.Networking.NetworkDestination.Clients);
            }
            else if (energySystem.currentOverheat >= energySystem.maxOverheat && base.isAuthority)
            {
                FireBall();

                new PlaySoundNetworkRequest(base.characterBody.netId, 553582860).Send(R2API.Networking.NetworkDestination.Clients);
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
                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageStat * coeff,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.world.mask,
                    weapon = null,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    hitCallback = laserHitCallback,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = null,
                    tracerEffectPrefab = null, // Change this later
                }.Fire();
                //ProjectileManager.instance.FireProjectile(
                //    Modules.Projectiles.lemurianFireBall, //prefab
                //    origin, //position
                //    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                //    gameObject, //owner
                //    damageStat * coeff, //damage
                //    force, //force
                //    Util.CheckRoll(critStat, characterBody.master), //crit
                //    DamageColorIndex.Default, //damage color
                //    null, //target
                //    speedOverride); //speed }

                //TO DO: Rotate to proper ROT.

                EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterWeakTracer, new EffectData
                {
                    origin = muzzlePos.position,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                    scale = 1f
                }, true);
            }

        }

        private bool laserHitCallback(BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo)
        {
            if (hitInfo.hitHurtBox) 
            {
                if (hitInfo.hitHurtBox.healthComponent)
                {
                    if (hitInfo.hitHurtBox.healthComponent.body.teamComponent.teamIndex != TeamIndex.Player)
                    {
                        
                        EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterStrongOnHit, new EffectData
                        {
                            origin = hitInfo.point,
                            rotation = Quaternion.LookRotation(hitInfo.surfaceNormal, Vector3.up),
                            scale = 1f
                        }, true);
                    }
                }
            }
            return BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
        }

        private void FireBeam(bool isCharged, bool isOverheated) 
        {
            // Function should determine what type of bolt to fire based on charge state.
            //Only has two charged states.

            if (isOverheated) 
            {
                FireBall();
            }

            if (isCharged)
            {
                FireChargedBolt();
            }
            else 
            {
                FireBolt();
            }
        }

        public void FireChargedBolt() 
        {
            Ray aimRay = GetAimRay();
            float coeff = isBlue ? altStrongDamageCoefficient : strongdamageCoefficient;
            Vector3 origin = GetDisplacedOrigin(aimRay);
            if (isAuthority)
            {
                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageStat * coeff,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.world.mask,
                    weapon = null,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    hitCallback = laserHitCallback,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = null,
                    tracerEffectPrefab = null, // Change this later
                }.Fire();
                //ProjectileManager.instance.FireProjectile(
                //    Modules.Projectiles.artificerFirebolt, //prefab
                //    origin, //position
                //    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                //    gameObject, //owner
                //    damageStat * coeff, //damage
                //    strongforce, //force
                //    Util.CheckRoll(critStat, characterBody.master), //crit
                //    DamageColorIndex.Default, //damage color
                //    null, //target
                //    strongspeedOverride); //speed }

                RaycastHit hit;
                Physics.Raycast(aimRay, out hit, Mathf.Infinity, ~(1 << LayerIndex.world.mask), QueryTriggerInteraction.Ignore);

                float distFromPoint = hit.distance;

                if (hit.collider && hit.distance > 10f)
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterChargedTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Quaternion.LookRotation(hit.point - muzzlePos.position, Vector3.up),
                        scale = 1f
                    }, true);
                }
                else
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        scale = 1f
                    }, true);
                }
            }
        }

        public void FireBolt()
        {
            Ray aimRay = GetAimRay();
            float coeff = isBlue ? altStrongDamageCoefficient : strongdamageCoefficient;
            Vector3 origin = GetDisplacedOrigin(aimRay);
            if (isAuthority)
            {
                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageStat * coeff,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.world.mask,
                    weapon = null,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    hitCallback = laserHitCallback,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = null,
                    tracerEffectPrefab = null, // Change this later
                }.Fire();
                //ProjectileManager.instance.FireProjectile(
                //    Modules.Projectiles.artificerFirebolt, //prefab
                //    origin, //position
                //    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                //    gameObject, //owner
                //    damageStat * coeff, //damage
                //    strongforce, //force
                //    Util.CheckRoll(critStat, characterBody.master), //crit
                //    DamageColorIndex.Default, //damage color
                //    null, //target
                //    strongspeedOverride); //speed }

                RaycastHit hit;
                Physics.Raycast(aimRay, out hit, Mathf.Infinity, ~(1 << LayerIndex.world.mask), QueryTriggerInteraction.Ignore);

                float distFromPoint = hit.distance;

                if (hit.collider && hit.distance > 10f)
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterChargedTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Quaternion.LookRotation(hit.point - muzzlePos.position, Vector3.up),
                        scale = 1f
                    }, true);
                }
                else 
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        scale = 1f
                    }, true);
                }
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
                outer.SetNextState(new FireSprayScepter());
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
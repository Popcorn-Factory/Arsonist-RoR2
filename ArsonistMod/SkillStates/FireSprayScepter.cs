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
        private float chargedMultiplier = Modules.StaticValues.firesprayScepterChargedMultiplier;
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
        private float baseTimeCharge = 0.75f;
        private float chargeStopwatch = 0f;

        private float skillStopwatch = 0f;
        private bool finishUp = false;

        private uint chargeSound;

        private float surgeTimer;
        private float surgeWait = Modules.StaticValues.firesprayScepterWaitTimer;
        private bool surgeFired;
        private bool fireSurgeShot;
        private bool surgeFiredOverheated;

        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();
            passive = characterBody.gameObject.GetComponent<ArsonistPassive>();
            isBlue = passive.isBlueGauge();
            characterBody.isSprinting = false;

            Ray aimRay = GetAimRay();
            duration = baseDuration / attackSpeedStat;

            characterBody.SetAimTimer(duration + 1f);
            //this.muzzleString = "LHand";

            animator = GetModelAnimator();

            //Get MuzzlePos
            ChildLocator childLoc = GetModelChildLocator();
            muzzlePos = childLoc.FindChild(muzzleString);
            
            //Play Charge sound with variable time stretch.
            AkSoundEngine.SetRTPCValue("PlaybackSpeed_Arsonist_Charge", attackSpeedStat);
            chargeSound = AkSoundEngine.PostEvent(2746588547, this.gameObject);

            fireSurgeShot = characterBody.HasBuff(Modules.Buffs.masochismSurgeActiveBuff);

            if (duration < surgeWait)
            {
                surgeWait = duration;
            }
        }

        public void PlayAttackAnimation() 
        {
            this.animator.SetBool("attacking", true);
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("Gesture, Override", "FireSpray", "Attack.playbackRate", duration, 0.1f);
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
                    damageType = new DamageTypeCombo(DamageType.Generic, DamageTypeExtended.Generic, DamageSource.Primary),
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.laser,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.noCollision.mask,
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
                RaycastHit hit;
                Physics.Raycast(aimRay, out hit, Mathf.Infinity, ~(1 << LayerIndex.world.mask), QueryTriggerInteraction.Ignore);

                float distFromPoint = hit.distance;

                if (hit.collider && hit.distance > 10f)
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterWeakTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Quaternion.LookRotation(hit.point - muzzlePos.position, Vector3.up),
                        scale = 1f
                    }, true);
                }
                else
                {
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterWeakTracer, new EffectData
                    {
                        origin = muzzlePos.position,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        scale = 1f
                    }, true);
                }

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
                        
                        EffectManager.SpawnEffect(isCharged ? Modules.AssetsArsonist.fireballScepterStrongOnHit : Modules.AssetsArsonist.fireballScepterOnHit, new EffectData
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
            AkSoundEngine.StopPlayingID(chargeSound);

            PlayAttackAnimation();

            //energy
            energyflatCost = Energy;
            if (energyflatCost < 0f) energyflatCost = 1f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 1f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                energySystem.AddHeat(energyCost);

                if (isCharged)
                {
                    new PlaySoundNetworkRequest(base.characterBody.netId, 1324654014).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else 
                {
                    new PlaySoundNetworkRequest(base.characterBody.netId, 4235059291).Send(R2API.Networking.NetworkDestination.Clients);
                }
            }
            else if (energySystem.currentOverheat >= energySystem.maxOverheat && base.isAuthority)
            {
                new PlaySoundNetworkRequest(base.characterBody.netId, 553582860).Send(R2API.Networking.NetworkDestination.Clients);
            }
            base.characterBody.AddSpreadBloom(spreadBloomValue);

            if (isOverheated) 
            {
                surgeFiredOverheated = true;
                FireBall();
            }

            if (isCharged)
            {
                surgeFiredOverheated = false;
                FireChargedBolt();
            }
            else 
            {
                surgeFiredOverheated = false;
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
                    damage = damageStat * coeff * chargedMultiplier,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = new DamageTypeCombo(DamageType.IgniteOnHit, DamageTypeExtended.Generic, DamageSource.Secondary),
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.laser,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 1f,
                    sniper = false,
                    stopperMask = LayerIndex.noCollision.mask,
                    weapon = null,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    hitCallback = laserHitCallback,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = null,
                    tracerEffectPrefab = null, // Change this later
                }.Fire();

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
                    damageType = new DamageTypeCombo(DamageType.IgniteOnHit, DamageTypeExtended.Generic, DamageSource.Secondary),
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.laser,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.noCollision.mask,
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
                    EffectManager.SpawnEffect(Modules.AssetsArsonist.fireballScepterTracer, new EffectData
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

            //Check if player is holding down the button
            // Fire bolt if not held for 0.75 seconds / attackspeed
            // Fire charged bolt if held for 0.75 seconds / attackspeed

            if (base.isAuthority && IsKeyDownAuthority() && !finishUp && !isCharged)
            {
                chargeStopwatch += Time.fixedDeltaTime;
                if (chargeStopwatch >= baseTimeCharge / attackSpeedStat)
                {
                    isCharged = true;
                    AkSoundEngine.PostEvent(1852205059, this.gameObject);
                    AkSoundEngine.StopPlayingID(chargeSound);
                }
            }

            if (base.isAuthority && !IsKeyDownAuthority() && !finishUp) 
            {
                FireBeam(isCharged, energySystem.currentOverheat >= energySystem.maxOverheat);
                finishUp = true;
            }

            if (finishUp && base.isAuthority) 
            {
                skillStopwatch += Time.fixedDeltaTime;

                if (fireSurgeShot)
                {
                    surgeTimer += Time.fixedDeltaTime;
                }

                if (surgeTimer > surgeWait && !surgeFired && base.isAuthority && fireSurgeShot)
                {
                    surgeFired = true;
                    if (surgeFiredOverheated)
                    {
                        FireBall();
                        new PlaySoundNetworkRequest(base.characterBody.netId, 553582860).Send(R2API.Networking.NetworkDestination.Clients);
                    }
                    else
                    {
                        if (isCharged)
                        {
                            new PlaySoundNetworkRequest(base.characterBody.netId, 1324654014).Send(R2API.Networking.NetworkDestination.Clients);
                            FireChargedBolt();
                        }
                        else 
                        {

                            new PlaySoundNetworkRequest(base.characterBody.netId, 4235059291).Send(R2API.Networking.NetworkDestination.Clients);
                            FireBolt();
                        }
                    }
                }

                if (skillStopwatch >= duration)
                {
                   outer.SetNextStateToMain();
                }
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
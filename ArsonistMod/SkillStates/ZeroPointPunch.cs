using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using EntityStates.Treebot.Weapon;
using static UnityEngine.ParticleSystem.PlaybackState;
using RoR2.Projectile;
using ArsonistMod.Modules;
using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace ArsonistMod.SkillStates
{
    public class ZeroPointPunch : BaseSkillState

    {
        public EnergySystem energySystem;

        public float previousMass;
        private Ray aimRay;
        private Vector3 aimRayDir;
        private Transform modelTransform;
        private CharacterModel characterModel;

        public static float baseduration = 0.5f;
        public static float duration;
        public static float hitExtraDuration = 0.44f;
        public static float minExtraDuration = 0.2f;
        public static float initialSpeedCoefficient = 5f;
        public float SpeedCoefficient;
        public static float finalSpeedCoefficient = 0f;
        public static float bounceForce = 100f; 
        private Vector3 bounceVector;
        private float stopwatch;
        private OverlapAttack detector;
        private OverlapAttack attack;
        protected string hitboxName2 = "ZeroPoint";
        protected string hitboxName = "ZeroPoint";
        protected float procCoefficient = 1f;
        protected float pushForce = 500f;
        protected Vector3 bonusForce = new Vector3(10f, 400f, 0f);
        protected float baseDuration = 1f;           
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected float baseEarlyExitTime = 0.4f;
        protected float hitStopDuration = 0.15f;
        protected float attackRecoil = 0.75f;
        protected float hitHopVelocity = 250f;
        private float hitPauseTimer;
        protected bool inHitPause;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;
        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = 82f;
        private float extraDuration;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 direction;
        private bool hasHopped;

        public float radius = 5f;
        protected DamageType damageType = DamageType.IgniteOnHit;
        private BlastAttack blastAttack;
        public bool hasHit;
        private float damageCoefficient;

        public override void OnEnter()
        {
            base.OnEnter();

            //Play swing sound
            if (base.isAuthority) 
            {
                new PlaySoundNetworkRequest(characterBody.netId, 3580806227).Send(R2API.Networking.NetworkDestination.Clients);
            }

            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            if (energySystem.currentOverheat < energySystem.maxOverheat && base.isAuthority)
            {
                //halve current heat
                energySystem.currentOverheat -= energySystem.currentOverheat * StaticValues.zeropointHeatReductionMultiplier;
                damageCoefficient = Modules.StaticValues.zeropointpounchDamageCoefficient;
            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && base.isAuthority)
            {
                //halve damage
                damageCoefficient = Modules.StaticValues.zeropointpounchDamageCoefficient * 0.5f;
            }

            hasHit = false;
            this.aimRayDir = aimRay.direction;

            duration = baseduration;
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num = num / base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (float)(num / (base.characterBody.baseMoveSpeed - 1f)); //What the fuck, without the float it casts the result as an int
            SpeedCoefficient = initialSpeedCoefficient * num2;

            this.extraDuration = Math.Max(hitExtraDuration / (num2 + 1f), minExtraDuration);


            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration / 2);
            this.animator = base.GetModelAnimator();
            this.animator.SetBool("attacking", true);
            base.characterBody.SetAimTimer(duration);
            HitBoxGroup hitBoxGroup = null;
            HitBoxGroup hitBoxGroup2 = null;
            Transform modelTransform = base.GetModelTransform();
            bool flag = modelTransform.gameObject.GetComponent<AimAnimator>();
            if (flag)
            {
                modelTransform.gameObject.GetComponent<AimAnimator>().enabled = false;
            }
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }


            bool flag2 = modelTransform;
            if (flag2)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
                hitBoxGroup2 = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName2);
            }


            this.attack = new OverlapAttack();
            this.attack.damageType = this.damageType;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = base.characterBody.damage;
            this.attack.procCoefficient = this.procCoefficient;
            this.attack.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab;
            this.attack.forceVector = this.bonusForce;
            this.attack.pushAwayForce = this.pushForce;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            //this.attack.impactSound = Modules.Assets.kickHitSoundEvent.index;


            this.detector = new OverlapAttack();
            this.detector.damageType = this.damageType;
            this.detector.attacker = base.gameObject;
            this.detector.inflictor = base.gameObject;
            this.detector.teamIndex = base.GetTeam();
            this.detector.damage = 0f;
            this.detector.procCoefficient = 0f;
            this.detector.hitEffectPrefab = null;
            this.detector.forceVector = Vector3.zero;
            this.detector.pushAwayForce = 0f;
            this.detector.hitBoxGroup = hitBoxGroup2;
            this.detector.isCrit = false;
            this.direction = base.GetAimRay().direction.normalized;
            base.characterDirection.forward = base.characterMotor.velocity.normalized;

            this.animator.SetFloat("Attack.playbackRate", attackSpeedStat);

            blastAttack = new BlastAttack();
            blastAttack.radius = radius;
            blastAttack.procCoefficient = 0.5f;
            blastAttack.position = characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = pushForce;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

            //base.PlayCrossfade("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", duration, 0.1f);

            //if (base.isAuthority)
            //{
            //    AkSoundEngine.PostEvent("shootstyedashvoice", this.gameObject);
            //}
            //AkSoundEngine.PostEvent("shootstyedashsfx", this.gameObject);


        }

        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            this.rollSpeed = num * Mathf.Lerp(SpeedCoefficient, finalSpeedCoefficient, base.fixedAge / duration);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.RecalculateRollSpeed();

            if (this.modelTransform)
            {
                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.6f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.7f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
            }
            this.FireAttack();
            this.hitPauseTimer -= Time.fixedDeltaTime;
            bool flag = this.hitPauseTimer <= 0f && this.inHitPause;
            if (flag)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.ApplyForce(this.bounceVector, true, false);
                FireSonicBoom fireSonicBoom = new FireSonicBoom();
                Util.PlaySound(fireSonicBoom.sound, base.gameObject);
                this.attack.Fire(null);
            }
            bool flag2 = !this.inHitPause;
            if (flag2)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                base.characterDirection.forward = this.bounceVector * -1f;
                bool flag3 = base.characterMotor;
                if (flag3)
                {
                    base.characterMotor.velocity = Vector3.zero;
                }
                bool flag4 = this.animator;
                if (flag4)
                {
                    this.animator.SetFloat("Attack.playbackRate", 0f);
                }
            }
            bool flag5 = !this.hasHopped;
            if (flag5)
            {
                base.characterDirection.forward = this.direction;
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += this.direction * this.rollSpeed * Time.fixedDeltaTime;
            }
            bool flag6 = base.cameraTargetParams;
            if (flag6)
            {
                base.cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, base.fixedAge / duration);
            }
            bool flag7 = base.isAuthority && this.stopwatch >= duration;
            if (flag7)
            {
                this.outer.SetNextStateToMain();
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            Transform modelTransform = base.GetModelTransform();
            bool flag = modelTransform.gameObject.GetComponent<AimAnimator>();
            if (flag)
            {
                modelTransform.gameObject.GetComponent<AimAnimator>().enabled = true;
            }
            base.characterMotor.velocity /= 1.75f;
            bool flag2 = base.cameraTargetParams;
            if (flag2)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            if (!hasHit)
            {
                DropBomb();
            }

            this.animator.SetBool("attacking", false);
        }

        public void DropBomb()
        {
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.zeropointBomb, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat * damageCoefficient, //damage
                    pushForce, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.Default, //damage color
                    null, //target
                    0f); //speed }

            }

        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }


        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }

        private void FireAttack()
        {
            bool isAuthority = base.isAuthority;
            if (isAuthority)
            {
                List<HurtBox> list = new List<HurtBox>();
                bool flag = this.detector.Fire(list);
                if (flag)
                {
                    foreach (HurtBox hurtBox in list)
                    {
                        bool flag2 = hurtBox.healthComponent && hurtBox.healthComponent.body;
                        if (flag2)
                        {
                            this.ForceFlinch(hurtBox.healthComponent.body);

                            blastAttack.position = hurtBox.healthComponent.body.corePosition;
                            blastAttack.Fire();
                        }
                    }
                    this.OnHitEnemyAuthority();
                }
            }
        }

        protected virtual void OnHitEnemyAuthority()
        {
            hasHit = true;

            EffectManager.SpawnEffect(Modules.Assets.explosionPrefab, new EffectData
            {
                origin = base.characterBody.corePosition,
                scale = this.radius,
            }, true);
            
            if (base.isAuthority)
            {
                new PlaySoundNetworkRequest(characterBody.netId, 1597575822).Send(R2API.Networking.NetworkDestination.Clients);
            }


            bool flag = !this.hasHopped;
            if (flag)
            {

                this.stopwatch = duration - this.extraDuration;
                bool flag2 = base.cameraTargetParams;
                if (flag2)
                {
                    base.cameraTargetParams.fovOverride = -1f;
                }

                bool flag3 = base.characterMotor;
                if (flag3)
                {
                    base.characterMotor.velocity = Vector3.zero;
                    this.bounceVector = base.GetAimRay().direction * -1f;
                    this.bounceVector.y = 0.1f;
                    this.bounceVector *= bounceForce;
                }
                bool flag4 = !this.inHitPause && this.hitStopDuration > 0f;
                if (flag4)
                {
                    this.storedVelocity = base.characterMotor.velocity;
                    this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Attack.playbackRate");
                    float num = this.moveSpeedStat;
                    bool isSprinting = base.characterBody.isSprinting;
                    if (isSprinting)
                    {
                        num /= base.characterBody.sprintingSpeedMultiplier;
                    }
                    float num2 = 1f + (num / base.characterBody.baseMoveSpeed - 1f);
                    this.hitPauseTimer = this.hitStopDuration / num2;
                    this.inHitPause = true;
                }
                this.hasHopped = true;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        protected virtual void ForceFlinch(CharacterBody body)
        {
            SetStateOnHurt component = body.healthComponent.GetComponent<SetStateOnHurt>();
            bool flag = component == null;
            if (!flag)
            {
                bool canBeHitStunned = component.canBeHitStunned;
                if (canBeHitStunned)
                {
                    component.SetPain();
                    bool flag2 = body.characterMotor;
                    if (flag2)
                    {
                        body.characterMotor.velocity = Vector3.zero;
                    }
                }
            }
        }


    }
}
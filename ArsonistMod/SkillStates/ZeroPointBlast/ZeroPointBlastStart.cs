using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using ArsonistMod.Modules;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using R2API.Networking.Interfaces;
using RoR2;
using EntityStates.Treebot.Weapon;
using HG;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    //From blast to moving.
    //On hit send to blast end stage.
    //If expired before blast end, whiff end.

    public class ZeroPointBlastStart : BaseSkillState
    {
        //Energy System and Controller
        public ArsonistController arsonistController;
        public EnergySystem energySystem;

        //Damage and attack
        private OverlapAttack detector;
        private OverlapAttack attack;
        protected float procCoefficient = 1f;
        protected DamageType damageType = DamageType.IgniteOnHit;
        private float damageCoefficient;
        private Vector3 direction; //For detector
        protected float pushForce = 500f;
        protected Vector3 bonusForce = new Vector3(10f, 400f, 0f);
        protected string hitboxName = "ZeroPoint";
        private BlastAttack blastAttack;
        public float radius = 5f;
        private Transform muzzlePos;
        private string muzzleString = "GunMuzzle";

        //Hit Pause
        private float hitPauseTimer;
        protected bool inHitPause;
        private HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;
        private Vector3 bounceVector;

        //Conditionals for attack
        public bool hasHit;
        public float stopwatch;

        //Animator
        public Animator animator;

        //Roll related
        private Vector3 aimRayDir;
        private Ray aimRay;
        public static float baseduration = 0.8f;
        public static float duration;
        public static float initialSpeedCoefficient = 4f;
        public float SpeedCoefficient;
        public static float finalSpeedCoefficient = 2f;
        private float extraDuration;
        private static float startupFrac = 0.2f;
        private static float freezeFrac = 0.58f;
        private static float blastFrac = 0.3f;
        private bool hasFired = false;
        public static float hitExtraDuration = 0.44f;
        public static float minExtraDuration = 0.2f;
        private Transform modelTransform;
        private CharacterModel characterModel;
        private float rollSpeed;

        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            //Play Start/Whiff sound
            if (base.isAuthority)
            {
                new PlaySoundNetworkRequest(characterBody.netId, 3585665340).Send(R2API.Networking.NetworkDestination.Clients);
            }

            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            //Handle energy consumption/regen
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

            //Code for initializing a "Roll"
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


            //Invicibility buff, disabling aim animator
            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration / 2);
            this.animator = base.GetModelAnimator();
            this.animator.SetBool("attacking", true);
            base.characterBody.SetAimTimer(duration);
            HitBoxGroup hitBoxGroup = null;
            HitBoxGroup hitBoxGroup2 = null;
            Transform modelTransform = base.GetModelTransform();
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }


            //Get Overlap attacks
            bool flag2 = modelTransform;
            if (flag2)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
                hitBoxGroup2 = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
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
            
            base.PlayCrossfade("FullBody, Override", "ZPBStart", "Attack.playbackRate", duration, 0.1f);
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

            arsonistController = base.gameObject.GetComponent<ArsonistController>();
            if (arsonistController) 
            {
                arsonistController.steamDownParticle.Play();
            }

            //Get MuzzlePos
            ChildLocator childLoc = GetModelChildLocator();
            muzzlePos = childLoc.FindChild(muzzleString);
        }

        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            if (num >= 10f)
            {
                num = Mathf.Log10((num - 9f)) + 10f;
            }
            this.rollSpeed = num * Mathf.Lerp(SpeedCoefficient, finalSpeedCoefficient, stopwatch - (duration * blastFrac) / (duration * freezeFrac - duration * blastFrac));
        }

        private void FireAttack()
        {
            //Attack should only be out on clientside.
            if (base.isAuthority)
            {
                //List of hits in detector.
                List<HurtBox> list = new List<HurtBox>();
                if (this.detector.Fire(list))
                {
                    //Send to next state.
                    this.OnHitEnemyAuthority();
                }
            }
        }

        protected virtual void OnHitEnemyAuthority()
        {
            //On hit, send player to End
            if (base.isAuthority) 
            {
                base.characterMotor.velocity = Vector3.zero;
                this.outer.SetState(new ZeroPointBlastEnd { damageCoefficient = damageCoefficient });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.PlayAnimation("FullBody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            //Increment timer
            this.stopwatch += Time.fixedDeltaTime;

            if (this.stopwatch <= duration * startupFrac) 
            {
                base.characterDirection.forward = this.direction;
            }

            if(this.stopwatch >= duration * blastFrac) 
            {
                if (!hasFired) 
                {
                    hasFired = true;
                    blastAttack.position = muzzlePos.position;
                    blastAttack.Fire();
                    arsonistController.fireBeam.Play();

                    EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                    {
                        origin = characterBody.corePosition,
                        scale = 1.5f
                    }, true);
                }
            }

            //Keep Firing and moving during interval 
            if (this.stopwatch >= duration * blastFrac && this.stopwatch <= duration * freezeFrac) 
            {
                //move and fire
                this.RecalculateRollSpeed();
                this.FireAttack();

                base.characterDirection.forward = this.direction;
                //base.characterMotor.velocity = Vector3.zero;
                //base.characterMotor.rootMotion += this.direction * this.rollSpeed * Time.fixedDeltaTime;
                if (base.characterMotor && base.characterDirection)
                {
                    Vector3 vector = this.direction * this.rollSpeed;
                    float d = Mathf.Max(Vector3.Dot(vector, this.direction), 0f);
                    vector = this.direction * d;

                    base.characterMotor.velocity = vector;
                }
            }

            //Whiff if outside the duration.
            if (stopwatch >= duration * freezeFrac && base.isAuthority) 
            {
                this.outer.SetState(new ZeroPointBlastWhiff { damageCoefficient = damageCoefficient });
            }
            
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.damageCoefficient);
        }


        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.damageCoefficient = reader.ReadSingle();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

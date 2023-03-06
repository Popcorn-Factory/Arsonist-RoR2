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

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    //From blast to moving.
    //On hit send to blast end stage.
    //If expired before blast end, whiff end.

    public class ZeroPointBlastStart : BaseSkillState
    {
        //Energy System
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

        //Conditionals for attack
        public bool hasHit;

        //Animator
        public Animator animator;

        //Roll related
        private Vector3 aimRayDir;
        private Ray aimRay;
        public static float baseduration = 0.5f;
        public static float duration;
        public static float initialSpeedCoefficient = 5f;
        public float SpeedCoefficient;
        public static float finalSpeedCoefficient = 1f;
        private float extraDuration;
        public static float hitExtraDuration = 0.44f;
        public static float minExtraDuration = 0.2f;
        private Transform modelTransform;
        private CharacterModel characterModel;

        public override void OnEnter()
        {
            base.OnEnter();
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


            //Get Overlap attacks
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
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //keep rolling
            //check if hit something,
            //If hit something, move to zero point Blast end
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

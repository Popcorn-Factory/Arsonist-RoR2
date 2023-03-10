using EntityStates;
using HG;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastEnd : BaseSkillState
    {
        public float stopwatch;
        public float duration;
        public static float baseDuration = 0.6f;
        public static float earlyExitFrac = 0.48f;
        public BlastAttack blastAttack;
        protected float pushForce = 500f;
        public float damageCoefficient;
        public float radius = 5f;
        public Animator animator;

        //Final blast.
        public override void OnEnter()
        {
            base.OnEnter();
            animator = base.GetModelAnimator();
            stopwatch = 0f;
            duration = baseDuration / base.attackSpeedStat;

            this.animator.SetBool("attacking", true);
            animator.SetFloat("Attack.playbackRate", this.attackSpeedStat);
            base.PlayCrossfade("FullBody, Override", "ZPBHit", "Attack.playbackRate", duration, 0.02f);
            base.characterMotor.velocity = Vector3.zero;

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

            blastAttack.Fire();
        }

        public override void OnExit()
        {
            base.OnExit();
            this.animator.SetBool("attacking", false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= duration)
            {
                base.outer.SetNextStateToMain();
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
            if (this.stopwatch >= duration * earlyExitFrac)
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Frozen;
        }
    }
}

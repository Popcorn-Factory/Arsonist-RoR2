using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates.ZeroPointBlast
{
    public class ZeroPointBlastWhiff : BaseSkillState
    {
        public float stopwatch;
        public float duration;
        public static float baseDuration = 0.6f;
        public static float earlyExitFrac = 0.48f;
        public static float stopFiringFrac = 0.24f;
        public Animator anim;

        public OverlapAttack detector;
        protected DamageType damageType = DamageType.IgniteOnHit;

        protected string hitboxName = "ZeroPoint";

        public float damageCoefficient;

        //Whiff anim and end.
        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            duration = baseDuration;
            this.anim = base.GetModelAnimator();
            this.anim.SetBool("attacking", true);
            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            base.PlayCrossfade("UpperBody, Override", "ZPBWhiff", "Attack.playbackRate", duration, 0.02f);

            Transform modelTransform = base.GetModelTransform();
            HitBoxGroup hitBoxGroup = null;
            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
            }

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
            this.detector.hitBoxGroup = hitBoxGroup;
            this.detector.isCrit = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            this.anim.SetBool("attacking", false);
            base.PlayAnimation("UpperBody, Override", "BufferEmpty");
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;

            //Last chance to change to end.
            if (stopwatch <= duration * stopFiringFrac) 
            {
                FireAttack();
            }

            //Nope, ya missed.
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
            if(this.stopwatch >= duration * earlyExitFrac) 
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Frozen;
        }
    }
}

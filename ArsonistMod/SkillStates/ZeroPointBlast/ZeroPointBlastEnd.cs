using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
using HG;
using R2API.Networking.Interfaces;
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
        public ArsonistController arsonistCon;
        public float stopwatch;
        public float duration;
        public static float baseDuration = 0.6f;
        public static float earlyExitFrac = 0.48f;
        public static float unlockVelocity = 0.2f;
        public BlastAttack blastAttack;
        protected float pushForce = 500f;
        public float damageCoefficient;
        public float radius = 5f;
        public Animator animator;
        private string muzzleString = "GunMuzzle";
        public Transform muzzlePos;

        //Final blast.
        public override void OnEnter()
        {
            base.OnEnter();

            //Get MuzzlePos
            ChildLocator childLoc = GetModelChildLocator();
            muzzlePos = childLoc.FindChild(muzzleString);
            if (base.isAuthority)
            {
                if (!Modules.Config.shouldHaveVoice.Value)
                {
                    new PlaySoundNetworkRequest(characterBody.netId, 1486446844).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else
                {
                    //Determine if they have a buff and play a non-laughing version if so.
                    uint soundStr = characterBody.HasBuff(Modules.Buffs.masochismBuff) ? (uint)1486446844 : 3858611310; //Nonlaugh : laugh
                    new PlaySoundNetworkRequest(characterBody.netId, soundStr).Send(R2API.Networking.NetworkDestination.Clients);
                }
            }
            animator = base.GetModelAnimator();
            stopwatch = 0f;
            duration = baseDuration;

            this.animator.SetBool("attacking", true);
            animator.SetFloat("Attack.playbackRate", 1f);
            if (isGrounded)
            {
                base.PlayAnimation("UpperBody, Override", "BufferEmpty");
                base.PlayCrossfade("FullBody, Override", "ZPBHit", "Attack.playbackRate", duration, 0.02f);
            }
            else 
            {
                base.PlayAnimation("FullBody, Override", "BufferEmpty");
                base.PlayCrossfade("UpperBody, Override", "ZPBHit", "Attack.playbackRate", duration, 0.02f);
            }
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.lastVelocity = Vector3.zero;

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

            EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
            {
                origin = characterBody.corePosition,
                scale = 3f
            }, true);

            arsonistCon = base.gameObject.GetComponent<ArsonistController>();
            arsonistCon.steamDownParticle.Stop();
            arsonistCon.fireBeamForward.Play();

        }

        public override void OnExit()
        {
            base.OnExit();
            this.animator.SetBool("attacking", false);
            arsonistCon.fireBeam.Stop();
            arsonistCon.fireBeamForward.Stop();
            base.PlayAnimation("UpperBody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch <= duration * unlockVelocity) 
            {
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.lastVelocity = Vector3.zero;
            }

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

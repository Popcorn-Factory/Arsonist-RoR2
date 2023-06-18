using System;
using ArsonistMod.Content.Controllers;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ArsonistMod.SkillStates.EmoteStates
{
    public class EmoteLobby : BaseSkillState
    {
        public ArsonistController controller;
        public float stopwatch;
        internal float duration;
        internal static float igniteFraction = 0.38f;
        internal static float flameParticleTrigger = 0.55f;
        public bool ignited;

        public override void OnEnter()
        {
            base.OnEnter();
            //Play the animation instantly, no scaling, nothing.
            PlayEmoteAnim();
            controller = GetComponent<ArsonistController>();
            stopwatch = 0f;
            duration = 1.7f;
            ignited = false;
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            //Turn off all particles
            controller.fingerFireParticle.Stop();
            controller.sparkParticle.Stop();
        }

        public override void Update()
        {
            base.Update();
            stopwatch += Time.deltaTime;
            if (stopwatch >= duration * igniteFraction && !ignited)
            {
                Util.PlaySound("Arsonist_Menu_Match_Strike", this.gameObject);
                ignited = true;
                // Play particle effects
                controller.fingerFireParticle.Play();
                controller.sparkParticle.Play();
            }

            if (base.isAuthority) 
            {
                if (ShouldEndEmoteState())
                {
                    base.outer.SetNextStateToMain();
                }
            }
            //Do not exit state automatically.
        }

        public bool ShouldEndEmoteState()
        {
            return !isGrounded || inputBank.skill1.down || inputBank.skill2.down || inputBank.skill3.down || inputBank.skill4.down;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void PlayEmoteAnim()
        {
            base.PlayAnimation("FullBody, Override", "Armature_LobbyStart");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}


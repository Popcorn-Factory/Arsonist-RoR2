using ArsonistMod.SkillStates.EmoteStates;
using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistController : MonoBehaviour
    {
        Animator anim;
        CharacterBody charBody;
        CharacterMotor characterMotor;
        EntityStateMachine[] entityStateMachines;
        EntityStateMachine weaponStateMachine;
        EntityStateMachine bodyStateMachine;

        public float idleStopwatch;
        public bool isIdle;

        public ParticleSystem steamParticle;
        public ParticleSystem steamDownParticle;
        public ParticleSystem fireBeam;
        public ParticleSystem fireBeamForward;

        // Use this for initialization
        void Start()
        {
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if(childLocator != null)
            {
                steamParticle = childLocator.FindChild("SteamGroup").GetComponent<ParticleSystem>();
                steamDownParticle = childLocator.FindChild("SteamFireDown").GetComponent<ParticleSystem>();
                fireBeam = childLocator.FindChild("FireBeam").GetComponent<ParticleSystem>();
                fireBeamForward = childLocator.FindChild("FireBeamForwardFiring").GetComponent<ParticleSystem>();
            }
            
            charBody = gameObject.GetComponent<CharacterBody>();
            characterMotor = gameObject.GetComponent<CharacterMotor>();
            entityStateMachines = gameObject.GetComponents<EntityStateMachine>();

            foreach (EntityStateMachine entityStateMachine in entityStateMachines) 
            {
                if (entityStateMachine.customName == "Weapon") 
                {
                    weaponStateMachine = entityStateMachine;
                }
                if (entityStateMachine.customName == "Body") 
                {
                    bodyStateMachine = entityStateMachine;
                }
            }

            idleStopwatch = 0f;
            isIdle = false;
            HurtBoxGroup hurtBoxGroup = charBody.hurtBoxGroup;
            anim = hurtBoxGroup.gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //Check here for authority and also executing emote states
            if (charBody.hasEffectiveAuthority)
            {
                //We can execute states now.

                //Check for input down.
                if (Modules.Config.emoteSitKey.Value.IsPressed())
                {
                    //Trigger the emote sit key
                    bodyStateMachine.SetNextState(new EmoteSit());
                }
                else if (Modules.Config.emoteStrutKey.Value.IsPressed())
                {
                    //Trigger the emote strut key
                    weaponStateMachine.SetNextState(new EmoteStrut());
                }
                else if (Modules.Config.dieKey.Value.IsPressed() && Modules.Config.shouldEnableDieKey.Value) 
                {
                    charBody.healthComponent.Suicide();
                }

                if (characterMotor.moveDirection.magnitude == 0
                    && !charBody.inputBank.skill1.down
                    && !charBody.inputBank.skill2.down
                    && !charBody.inputBank.skill3.down
                    && !charBody.inputBank.skill4.down
                    && characterMotor.isGrounded)
                {
                    idleStopwatch += Time.deltaTime;
                    if (idleStopwatch >= 7f && !isIdle)
                    {
                        isIdle = true;
                        anim.SetBool("isIdle", true);
                    }
                }
                else
                {
                    idleStopwatch = 0f;
                    isIdle = false;
                    anim.SetBool("isIdle", false);
                }
            }
        }
    }
}
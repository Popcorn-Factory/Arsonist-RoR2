using ArsonistMod.Modules.Networking;
using ArsonistMod.SkillStates.EmoteStates;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
        public bool flamethrowerSelected;

        public ParticleSystem steamParticle;
        public ParticleSystem steamDownParticle;
        public ParticleSystem fireBeam;
        public ParticleSystem fireBeamForward;
        public ParticleSystem flamethrower;
        public ParticleSystem ringFire;
        public Transform flamethrowerTransform;

        public bool ringFireActive;

        public bool playingFlamethrower;
        public uint flamethrowerPlayingID;

        //FootstepStuff
        public FootstepHandler footstepHandler;

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
                flamethrowerTransform = childLocator.FindChild("Flamethrower");
                flamethrower = childLocator.FindChild("Flamethrower").GetComponent<ParticleSystem>();
                ringFire = childLocator.FindChild("RingFlame").GetComponent<ParticleSystem>();
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

            //check the current primary equipped.
            if (charBody.skillLocator.primary.skillNameToken == "POPCORN_ARSONIST_BODY_PRIMARY_FLAMETHROWER_NAME") 
            {
                flamethrowerSelected = true;
            }
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

            //Check the Aim ray of the characterbody and aim the flamethrower in that direction.
            if (flamethrowerSelected && flamethrowerTransform) 
            {
                Ray ray = charBody.inputBank.GetAimRay();
                flamethrowerTransform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);

                if (!charBody.inputBank.skill1.down && playingFlamethrower) 
                {
                    playingFlamethrower = false;
                    AkSoundEngine.StopPlayingID(flamethrowerPlayingID);
                    new PlaySoundNetworkRequest(charBody.netId, 4168901551).Send(R2API.Networking.NetworkDestination.Clients);
                }
            }

            if (ringFireActive && Modules.Config.cleanseRingFireEffectEnabled.Value) 
            {
                if (!charBody.HasBuff(Modules.Buffs.cleanseSpeedBoost)) 
                { 
                    ringFireActive = false;
                    ringFire.Stop();
                }
            }
            else if (!Modules.Config.cleanseRingFireEffectEnabled.Value) 
            {
                ringFireActive = false;
                ringFire.Stop();
            }
        }
    }
}
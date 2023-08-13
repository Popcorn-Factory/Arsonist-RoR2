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
        public float idleIntervalStopwatch;
        public bool isIdle;
        public bool justEnteredIdle;
        public bool flamethrowerSelected;

        public ParticleSystem steamParticle;
        public ParticleSystem steamDownParticle;
        public ParticleSystem fireBeam;
        public ParticleSystem fireBeamForward;
        public ParticleSystem flamethrower;
        public ParticleSystem weakFlamethrower;
        public ParticleSystem ringFire;
        public ParticleSystem trailFire;
        public ParticleSystem sparkParticle;
        public ParticleSystem fingerFireParticle;
        public ParticleSystem cleanseBlast;
        public Transform flamethrowerTransform;
        public Transform weakFlamethrowerTransform;

        public bool ringFireActive;

        public bool playingFlamethrower;
        public uint flamethrowerPlayingID;

        //FootstepStuff
        public FootstepHandler footstepHandler;

        public bool hasZPBedRecently;

        //Camera rig
        public CameraRigController cameraRigController;

        // Use this for initialization

        void Awake() 
        {
            Hooks();
        }

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
                weakFlamethrowerTransform = childLocator.FindChild("FlamethrowerWeak");
                weakFlamethrower = childLocator.FindChild("FlamethrowerWeak").GetComponent<ParticleSystem>();
                flamethrower = childLocator.FindChild("Flamethrower").GetComponent<ParticleSystem>();
                ringFire = childLocator.FindChild("RingFlame").GetComponent<ParticleSystem>();
                trailFire = childLocator.FindChild("TrailFlame").GetComponent<ParticleSystem>();
                sparkParticle = childLocator.FindChild("SparkEffect").GetComponent<ParticleSystem>();
                fingerFireParticle = childLocator.FindChild("FireThumbParticle").GetComponent<ParticleSystem>();
                cleanseBlast = childLocator.FindChild("CleanseBlast").GetComponent<ParticleSystem>();
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

            charBody._defaultCrosshairPrefab = flamethrowerSelected ? Modules.Assets.flamethrowerCrosshair : Modules.Assets.fireballCrosshair;

        }

        public void StopAllParticleEffects() 
        {
            steamParticle.Stop();
            steamDownParticle.Stop();
            fireBeam.Stop();
            fireBeamForward.Stop();
            weakFlamethrower.Stop();
            flamethrower.Stop();
            ringFire.Stop();
            trailFire.Stop();
            sparkParticle.Stop();
            fingerFireParticle.Stop();
            cleanseBlast.Stop();
        }

        public void Hooks() 
        {
            On.RoR2.CameraRigController.Update += CameraRigController_Update;
        }

        public void Unhook() 
        {
            On.RoR2.CameraRigController.Update -= CameraRigController_Update;
        }

        void CameraRigController_Update(On.RoR2.CameraRigController.orig_Update orig, CameraRigController self)
        {
            orig(self);
            if (!cameraRigController) 
            {
                cameraRigController = self;
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (characterMotor.isGrounded && hasZPBedRecently) 
            {
                //Turn off the damn whiffing sound if they're touching the ground.
                new PlaySoundNetworkRequest(charBody.netId, 1135082308).Send(R2API.Networking.NetworkDestination.Clients);
                hasZPBedRecently = false;
            }
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
                    bodyStateMachine.SetNextStateToMain();
                    weaponStateMachine.SetNextState(new EmoteStrut());
                }
                else if (Modules.Config.emoteLobbyKey.Value.IsPressed())
                {
                    //Trigger the emote lobby
                    bodyStateMachine.SetNextState(new EmoteLobby());
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
                        justEnteredIdle = true;
                        anim.SetBool("isIdle", true);
                    }
                }
                else
                {
                    idleStopwatch = 0f;
                    idleIntervalStopwatch = 0f;
                    justEnteredIdle = false;
                    isIdle = false;
                    anim.SetBool("isIdle", false);
                }

                //Logic to handle Playing idle sounds, wait 30s between each successive play.
                if (isIdle) 
                {
                    idleIntervalStopwatch += Time.deltaTime;
                    if (idleIntervalStopwatch >= 25f || (justEnteredIdle && idleIntervalStopwatch >= 8f) ) 
                    {
                        justEnteredIdle = false;
                        idleIntervalStopwatch = 0f;
                        if (charBody)
                        {
                            uint soundStr = (charBody.skinIndex == Modules.Survivors.Arsonist.FirebugSkinIndex) ? (uint)137978395 : (uint)1362851720;
                            new PlaySoundNetworkRequest(charBody.netId, soundStr).Send(R2API.Networking.NetworkDestination.Clients);
                        }
                    }
                }
            }

            //Check the Aim ray of the characterbody and aim the flamethrower in that direction.
            if (flamethrowerSelected && flamethrowerTransform) 
            {
                Ray ray = charBody.inputBank.GetAimRay();
                flamethrowerTransform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
                weakFlamethrowerTransform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);

                if (!charBody.inputBank.skill1.down && playingFlamethrower && charBody.hasEffectiveAuthority) 
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
                    trailFire.Stop();
                    ringFire.Stop();
                }
            }
            else if (!Modules.Config.cleanseRingFireEffectEnabled.Value) 
            {
                ringFireActive = false;
                trailFire.Stop();
                ringFire.Stop();
            }
        }

        public void OnDestroy() 
        {
            Unhook();
        }
    }
}
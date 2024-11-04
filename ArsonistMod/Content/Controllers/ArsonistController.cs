using ArsonistMod.Modules.Networking;
using ArsonistMod.SkillStates.EmoteStates;
using R2API.Networking;
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
        public ParticleSystem flamethrowerHeatHaze;
        public ParticleSystem weakFlamethrower;
        public ParticleSystem ringFire;
        public ParticleSystem trailFire;
        public ParticleSystem sparkParticle;
        public ParticleSystem fingerFireParticle;
        public ParticleSystem cleanseBlast;
        public Transform flamethrowerTransform;
        public Transform weakFlamethrowerTransform;

        public Transform flamethrowerScepterContainer;
        public Transform flamethrowerScepterTransform;
        public ParticleSystem flamethrowerBeamPS;
        public Animator flamethrowerScepterBeamAnimator;
        public ParticleSystem flamethrowerScepterHeatHaze;

        public bool ringFireActive;

        public bool playingFlamethrower;
        public uint flamethrowerPlayingID;

        //FootstepStuff
        public FootstepHandler footstepHandler;

        public bool hasZPBedRecently;

        //Camera rig
        public CameraRigController cameraRigController;

        //Instance Material
        public Material overheatingMaterial;
        public Material outlineMaterial;

        // Use this for initialization
        void Awake() 
        {
            Hooks();
            overheatingMaterial = new Material(Modules.AssetsArsonist.arsonistOverheatingMaterial);
            outlineMaterial = new Material(Modules.AssetsArsonist.arsonistOutlineMaterial);
            outlineMaterial.renderQueue = 2000;
        }

        void Start()
        {
            ChildLocator childLocator = GetComponent<ModelLocator>().modelTransform.GetComponent<ChildLocator>();
            if(childLocator)
            {
                steamParticle = childLocator.FindChild("SteamGroup").GetComponent<ParticleSystem>();
                steamDownParticle = childLocator.FindChild("SteamFireDown").GetComponent<ParticleSystem>();
                fireBeam = childLocator.FindChild("FireBeam").GetComponent<ParticleSystem>();
                fireBeamForward = childLocator.FindChild("FireBeamForwardFiring").GetComponent<ParticleSystem>();
                flamethrowerTransform = childLocator.FindChild("Flamethrower");
                weakFlamethrowerTransform = childLocator.FindChild("FlamethrowerWeak");
                weakFlamethrower = childLocator.FindChild("FlamethrowerWeak").GetComponent<ParticleSystem>();
                flamethrower = childLocator.FindChild("Flamethrower").GetComponent<ParticleSystem>();
                flamethrowerHeatHaze = childLocator.FindChild("Flamethrower").GetChild(1).GetComponent<ParticleSystem>();
                ringFire = childLocator.FindChild("RingFlame").GetComponent<ParticleSystem>();
                trailFire = childLocator.FindChild("TrailFlame").GetComponent<ParticleSystem>();
                sparkParticle = childLocator.FindChild("SparkEffect").GetComponent<ParticleSystem>();
                fingerFireParticle = childLocator.FindChild("FireThumbParticle").GetComponent<ParticleSystem>();
                cleanseBlast = childLocator.FindChild("CleanseBlast").GetComponent<ParticleSystem>();
                flamethrowerScepterContainer = childLocator.FindChild("ScepterFlamethrowerContainer");
                flamethrowerScepterTransform = childLocator.FindChild("ScepterFlamethrower");
                flamethrowerBeamPS = flamethrowerScepterTransform.GetComponent<ParticleSystem>();
                flamethrowerScepterBeamAnimator = flamethrowerScepterTransform.GetComponent<Animator>();
                flamethrowerScepterHeatHaze = flamethrowerScepterTransform.Find("HeatHaze").GetComponent<ParticleSystem>();
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
            if (charBody.skillLocator.primary.skillNameToken == "POPCORN_ARSONIST_BODY_PRIMARY_FLAMETHROWER_NAME" || charBody.skillLocator.primary.skillNameToken == "POPCORN_ARSONIST_BODY_PRIMARY_FLAMETHROWER_SCEPTER_NAME") 
            {
                flamethrowerSelected = true;
            }

            //Change _defaultCrosshairPrefab when new skill is loaded?
            charBody._defaultCrosshairPrefab = flamethrowerSelected ? Modules.AssetsArsonist.flamethrowerCrosshair : Modules.AssetsArsonist.fireballCrosshair;

            //Updating only when arsonist spawns in.
            if (AkSoundEngine.IsInitialized()) 
            {
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice", Modules.Config.arsonistVoicelineVolume.Value);
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice_Arsonist", Modules.Config.arsonistVoicelineVolumeArsonist.Value);
                AkSoundEngine.SetRTPCValue("Volume_ArsonistVoice_Firebug", Modules.Config.arsonistVoicelineVolumeFirebug.Value);
                AkSoundEngine.SetRTPCValue("Volume_ArsonistSFX", Modules.Config.arsonistSFXVolume.Value);
            }
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
            On.RoR2.CameraRigController.LateUpdate += CameraRigController_LateUpdate;
        }

        public void Unhook() 
        {
            On.RoR2.CameraRigController.LateUpdate -= CameraRigController_LateUpdate;
        }

        void CameraRigController_LateUpdate(On.RoR2.CameraRigController.orig_LateUpdate orig, CameraRigController self)
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
                if (UnityEngine.Input.GetKeyDown(Modules.Config.emoteSitKey.Value.MainKey))
                {
                    //Trigger the emote sit key
                    bodyStateMachine.SetNextState(new EmoteSit());
                }
                else if (UnityEngine.Input.GetKeyDown(Modules.Config.emoteStrutKey.Value.MainKey))
                {
                    //Trigger the emote strut key
                    bodyStateMachine.SetNextStateToMain();
                    weaponStateMachine.SetNextState(new EmoteStrut());
                }
                else if (UnityEngine.Input.GetKeyDown(Modules.Config.emoteLobbyKey.Value.MainKey))
                {
                    //Trigger the emote lobby
                    bodyStateMachine.SetNextState(new EmoteLobby());
                }
                else if (UnityEngine.Input.GetKeyDown(Modules.Config.dieKey.Value.MainKey) && Modules.Config.shouldEnableDieKey.Value)
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
                flamethrowerScepterContainer.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);

                if (charBody.inputBank.skill1.down) 
                {
                    flamethrowerTransform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
                    weakFlamethrowerTransform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
                }

                if (!charBody.inputBank.skill1.down && playingFlamethrower && charBody.hasEffectiveAuthority)
                {
                    playingFlamethrower = false;
                    AkSoundEngine.StopPlayingID(flamethrowerPlayingID);


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

        public void DeactivateScepterFlamethrower() 
        {
            //Stop playing the flamethrower stuff on the animator.
            if (flamethrowerScepterBeamAnimator)
            {
                flamethrowerScepterBeamAnimator.SetBool("active", false);
            }

            if (flamethrowerBeamPS)
            {
                flamethrowerBeamPS.Stop();
            }

            if (flamethrowerScepterHeatHaze) 
            {
                flamethrowerScepterHeatHaze.Stop();
            }
        }

        public void ActivateScepterFlamethrowerBeam() 
        {
            if (flamethrowerScepterBeamAnimator) 
            {
                flamethrowerScepterBeamAnimator.SetBool("active", true);
            }

            if (flamethrowerBeamPS) 
            {
                flamethrowerBeamPS.Play();
            }

            if (Modules.Config.enableNonAggressiveHeatHaze.Value != flamethrowerScepterHeatHaze.isPlaying && flamethrowerScepterHeatHaze)
            {
                if (Modules.Config.enableNonAggressiveHeatHaze.Value)
                {
                    flamethrowerScepterHeatHaze.Play();
                }
                else
                {
                    flamethrowerScepterHeatHaze.Stop();
                }
            }
        }

        public void OnDestroy() 
        {
            Unhook();
            Destroy(overheatingMaterial);
            new PlaySoundNetworkRequest(charBody.netId, (uint)2176930590).Send(NetworkDestination.Clients);
        }
    }
}
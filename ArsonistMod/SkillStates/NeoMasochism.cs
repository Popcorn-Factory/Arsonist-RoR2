using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates
{
    internal class NeoMasochism : BaseSkillState
    {
        public MasochismController maso;
        public ArsonistController arsonistCon;
        public float stopwatch;
        public static float baseActivationTime = 0.47f;
        public static float baseDuration = 5.02f;
        public float duration;
        public float originalFOV;
        public float targetFOV;
        public float originalLerpTime;
        public static float tempLerpTime = 0.25f;
        public float multiplier = 1.1f;

        private CameraTargetParams.CameraParamsOverrideHandle handle;

        public override void OnEnter()
        {
            base.OnEnter();
            maso = gameObject.GetComponent<MasochismController>();
            arsonistCon = gameObject.GetComponent<ArsonistController>();
            duration = baseDuration;

            if (arsonistCon.cameraRigController)
            {
                originalFOV = arsonistCon.cameraRigController.baseFov;
                targetFOV = originalFOV * multiplier;
            }
            CameraTargetParams ctp = base.cameraTargetParams;
            CharacterCameraParamsData characterCameraParamsData = ctp.currentCameraParamsData;
            characterCameraParamsData.fov = targetFOV;

            CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
            {
                cameraParamsData = characterCameraParamsData,
                priority = 0,

            };

            if (base.isAuthority && Modules.Config.ToggleMasochismFOVWarp.Value) 
            {
                handle = ctp.AddParamsOverride(request, baseDuration * baseActivationTime);

            }

            if (maso)
            {
                if (maso)
                {
                    maso.masoRecentlyActivated = true;
                }
            }

            if (maso && maso.masochismActive && base.isAuthority) 
            {
                maso.TriggerMasochismAndEXOverheat(false);
                base.outer.SetNextStateToMain();
                return;
            }

            if (base.isAuthority) 
            {
                if (!Modules.Config.shouldHaveVoice.Value)
                {
                    new PlaySoundNetworkRequest(characterBody.netId, 955478894).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else
                {
                    uint soundStr;
                    if (base.characterBody.skinIndex == Modules.Survivors.Arsonist.FirebugSkinIndex)
                    {
                        soundStr = characterBody.HasBuff(Modules.Buffs.masochismBuff) ? (uint)955478894 : (uint)1845947419; //Nonlaugh : laugh
                    }
                    else
                    {
                        //Determine if they have a buff and play a non-laughing version if so.
                        soundStr = characterBody.HasBuff(Modules.Buffs.masochismBuff) ? (uint)955478894 : (uint)1305067912; //Nonlaugh : laugh
                    }


                    new PlaySoundNetworkRequest(characterBody.netId, soundStr).Send(NetworkDestination.Clients);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.cameraTargetParams.RemoveParamsOverride(handle);
        }

        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            
            if (stopwatch >= duration * baseActivationTime && base.isAuthority) 
            {
                if (maso && !maso.masochismActive)
                {
                    //Start Masochism Sound
                    maso.ActivateMaso();
                }

                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

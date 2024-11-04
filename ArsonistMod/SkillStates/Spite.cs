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
    internal class Spite : BaseSkillState
    {
        public SpiteController maso;
        public ArsonistController arsonistCon;
        public float stopwatch;
        public static float baseActivationTime = 0.16f;
        public static float baseDuration = 3.1f;
        public float duration;
        public float originalFOV;
        public float targetFOV;
        public float originalLerpTime;
        public static float tempLerpTime = 0.25f;
        public float multiplier = 1.1f;
        public EnergySystem energySystem;

        private CameraTargetParams.CameraParamsOverrideHandle handle;

        public override void OnEnter()
        {
            base.OnEnter();
            maso = gameObject.GetComponent<SpiteController>();
            arsonistCon = gameObject.GetComponent<ArsonistController>();
            energySystem = gameObject.GetComponent<EnergySystem>();
            duration = baseDuration;

            if (energySystem && base.isAuthority)
            {
                //Disallow execution if the user has overheat maxed out.
                if (energySystem.ifOverheatMaxed)
                {
                    new PlaySoundNetworkRequest(characterBody.netId, "Arsonist_Masochism_Denied").Send(NetworkDestination.Clients);
                    this.outer.SetNextStateToMain();
                    return;
                }

                energySystem.SetCurrentHeatToLowerBound();
            }

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

            if (maso && maso.masochismActive && base.isAuthority)
            {
                maso.TriggerMasochismAndEXOverheat(false);
                base.outer.SetNextStateToMain();
                return;
            }

            if (base.isAuthority)
            {
                //Always Play the base sound:
                new PlaySoundNetworkRequest(characterBody.netId, 974780206).Send(R2API.Networking.NetworkDestination.Clients);

                //Optionally play the voice on top if they have the buff and it's active. Soundbank has percentage chance set!
                if (Modules.Config.shouldHaveVoice.Value && !characterBody.HasBuff(Modules.Buffs.masochismBuff))
                {
                    uint soundInt;

                    soundInt = base.characterBody.skinIndex == Modules.Survivors.Arsonist.FirebugSkinIndex ? 1951821374 : 2881057417;

                    new PlaySoundNetworkRequest(characterBody.netId, soundInt).Send(NetworkDestination.Clients);
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

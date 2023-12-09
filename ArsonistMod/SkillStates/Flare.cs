using EntityStates;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ArsonistMod.Content.Controllers;
using static UnityEngine.ParticleSystem.PlaybackState;
using HG;
using System.Collections.Generic;
using System.Linq;
using ArsonistMod.Modules.Networking;
using R2API.Networking.Interfaces;
using static RoR2.BulletAttack;
using ArsonistMod.SkillStates.Arsonist.Secondary;
using R2API;
using ArsonistMod.Modules;
using System;
using static RoR2.BlastAttack;

namespace ArsonistMod.SkillStates
{
    public class Flare : BaseSkillState
    {
        public EnergySystem energySystem;
        private Ray aimRay;
        public float baseDuration = 1f;
        public float duration;
        public float fireTime;
        public float recoil = 1f;
        private bool hasFired;
        private bool isStrong;
        private Animator animator;


        private string muzzleString = "FlareMuzzle";
        private float energyCost;
        private float energyflatCost;
        private float speedOverride = Modules.StaticValues.flareSpeedCoefficient;
        private BulletAttack bulletAttack;
        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            hasFired = false;

            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            aimRay = base.GetAimRay();
            duration = baseDuration / attackSpeedStat;
            fireTime = duration * 0.35f;

            base.characterBody.SetAimTimer(this.duration);

            animator = GetModelAnimator();
            this.animator.SetBool("attacking", true);
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("Gesture, Override", "Flare", "Attack.playbackRate", duration, 0.1f);


            //Muzzle Positioning
            ChildLocator childLoc = GetModelChildLocator();
            Transform muzzleTransform = childLoc.FindChild(muzzleString);

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                energySystem.LowerHeat(energySystem.currentOverheat * StaticValues.flareHeatReductionMultiplier);
                energySystem.hasOverheatedSecondary = false;

                new PlaySoundNetworkRequest(base.characterBody.netId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);


                isStrong = true;

            }
            else if (energySystem.currentOverheat >= energySystem.maxOverheat && base.isAuthority)
            {
                new PlaySoundNetworkRequest(base.characterBody.netId, 1608533803).Send(R2API.Networking.NetworkDestination.Clients);
                isStrong = false;

            }

            if (isAuthority && Modules.Config.shouldHaveVoice.Value) 
            {
                float randomNum = UnityEngine.Random.Range(1, 101);
                if (Modules.Config.flareVoicelineChance.Value >= randomNum && Modules.Config.flareVoicelineChance.Value != 0 && base.characterBody.skinIndex != Modules.Survivors.Arsonist.FirebugSkinIndex) 
                {
                    // Calculate chance, send network request for flare noise.
                    new PlaySoundNetworkRequest(base.characterBody.netId, 2767633755).Send(R2API.Networking.NetworkDestination.Clients);
                }
            }

            //This is so fucking stupid fuck fuck fucking goddamn it fuck
            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = Projectiles.strongFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(Damage.arsonistStickyDamageType);

            DamageAPI.ModdedDamageTypeHolderComponent weakDamageTypeComponent = Projectiles.weakFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            weakDamageTypeComponent.Add(Damage.arsonistWeakStickyDamageType);


        }

        //public void FireBullet()
        //{
        //    base.characterBody.AddSpreadBloom(1f);
        //    base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

        //    ChildLocator childLoc = GetModelChildLocator();
        //    Transform muzzleTransform = childLoc.FindChild(muzzleString);

        //    hasFired = true;


        //    bulletAttack.Fire();


        //    //EffectManager.SpawnEffect(Assets.arsonistFlare, new EffectData
        //    //{
        //    //    origin = FindModelChild(this.muzzleString).position,
        //    //    scale = 1f,
        //    //    rotation = Quaternion.LookRotation(aimRay.direction)

        //    //}, true);
        //}


        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("attacking", false);
            PlayAnimation("Gesture, Override", "BufferEmpty");

        }

        public void StrongFlare()
        {
            //Strong flare chains more flares.
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                base.characterBody.AddSpreadBloom(1f);
                base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.strongFlare, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat, //damage
                    0f, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.Count, //damage color
                    null, //target
                    speedOverride); //speed }

            }
        }

        public void WeakFlare()
        {
            //Weak flare is old strong flare.
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                base.characterBody.AddSpreadBloom(1f);
                base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                ProjectileManager.instance.FireProjectile(
                    Modules.Projectiles.weakFlare, //prefab
                    aimRay.origin, //position
                    Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
                    gameObject, //owner
                    damageStat, //damage
                    0f, //force
                    Util.CheckRoll(critStat, characterBody.master), //crit
                    DamageColorIndex.Count, //damage color
                    null, //target
                    speedOverride); //speed }

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!hasFired && base.fixedAge > fireTime && base.isAuthority)
            {
                hasFired = true;

                if (isStrong)
                {
                    //DamageAPI.AddModdedDamageType(bulletAttack, Damage.arsonistStickyDamageType);
                    StrongFlare();
                }
                else if (!isStrong)
                {
                    WeakFlare();
                    //DamageAPI.AddModdedDamageType(bulletAttack, Damage.arsonistWeakStickyDamageType);
                }
                
                //FireBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

    }
}
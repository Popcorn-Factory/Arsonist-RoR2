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


        private string muzzleString = "FlareMuzzle";
        public float Energy = Modules.StaticValues.flareEnergyCost;
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
            fireTime = duration/ 3f;

            base.characterBody.SetAimTimer(this.duration);


            bulletAttack = new BulletAttack
            {
                bulletCount = (uint)(1U),
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = damageStat,
                damageColorIndex = DamageColorIndex.Count,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                maxDistance = 100f,
                force = 0f,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = 0f,
                maxSpread = 0f,
                isCrit = base.RollCrit(),
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = true,
                procChainMask = default(ProcChainMask),
                procCoefficient = 0.1f,
                radius = 1f,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = Modules.Assets.arsonistFlare,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
            };

            //Muzzle Positioning
            ChildLocator childLoc = GetModelChildLocator();
            Transform muzzleTransform = childLoc.FindChild(muzzleString);

            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                energySystem.currentOverheat += energyCost;

                new PlaySoundNetworkRequest(base.characterBody.netId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);


                isStrong = true;

            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && isAuthority)
            {
                new PlaySoundNetworkRequest(base.characterBody.netId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);
                isStrong = false;

            }
        }
        
        public void FireBullet()
        {
            base.characterBody.AddSpreadBloom(1f);
            base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

            ChildLocator childLoc = GetModelChildLocator();
            Transform muzzleTransform = childLoc.FindChild(muzzleString);

            hasFired = true;


            bulletAttack.Fire();


            //EffectManager.SpawnEffect(Assets.arsonistFlare, new EffectData
            //{
            //    origin = FindModelChild(this.muzzleString).position,
            //    scale = 1f,
            //    rotation = Quaternion.LookRotation(aimRay.direction)

            //}, true);
        }


        public override void OnExit()
        {
            base.OnExit();
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            
        }

        //public void StrongFlare()
        //{
        //    Ray aimRay = GetAimRay();
        //    if (isAuthority)
        //    {
        //        ProjectileManager.instance.FireProjectile(
        //            Modules.Projectiles.strongFlare, //prefab
        //            aimRay.direction, //position
        //            Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
        //            gameObject, //owner
        //            damageStat , //damage
        //            0f, //force
        //            Util.CheckRoll(critStat, characterBody.master), //crit
        //            DamageColorIndex.Count, //damage color
        //            null, //target
        //            speedOverride); //speed }

        //    }
        //}

        //public void WeakFlare()
        //{
        //    Ray aimRay = GetAimRay();
        //    if (isAuthority)
        //    {
        //        ProjectileManager.instance.FireProjectile(
        //            Modules.Projectiles.weakFlare, //prefab
        //            aimRay.direction, //position
        //            Util.QuaternionSafeLookRotation(aimRay.direction), //rotation
        //            gameObject, //owner
        //            damageStat, //damage
        //            0f, //force
        //            Util.CheckRoll(critStat, characterBody.master), //crit
        //            DamageColorIndex.Count, //damage color
        //            null, //target
        //            speedOverride); //speed }

        //    }
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!hasFired && base.fixedAge > fireTime && base.isAuthority)
            {
                hasFired = true;

                if (isStrong)
                {
                    DamageAPI.AddModdedDamageType(bulletAttack, Damage.arsonistStickyDamageType);
                }
                else if (!isStrong)
                {
                    DamageAPI.AddModdedDamageType(bulletAttack, Damage.arsonistWeakStickyDamageType);
                }
                
                FireBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
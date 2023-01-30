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

namespace ArsonistMod.SkillStates
{
    public class Flare : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 1f;
        public float duration;
        public float fireTime;
        public float recoil = 1f;
        public bool hasFired;


        private string muzzleString = "FlareMuzzle";
        public float Energy = Modules.StaticValues.flareEnergyCost;
        private float energyCost;
        private float energyflatCost;
        private float speedOverride = Modules.StaticValues.flareSpeedCoefficient;
        private BulletAttack bulletAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration / attackSpeedStat;
            fireTime = duration/ 3f;

            base.characterBody.SetAimTimer(this.duration);

            //Muzzle Positioning
            ChildLocator childLoc = GetModelChildLocator();
            Transform muzzleTransform = childLoc.FindChild(muzzleString);

            energyflatCost = Energy - energySystem.costflatOverheat;
            if (energyflatCost < 0f) energyflatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyflatCost;
            if (energyCost < 0f) energyCost = 0f;

            if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
            {
                //ProjectileManager.instance.FireProjectile(Modules.Projectiles.strongFlare,
                //           muzzleTransform.position + aimRay.direction,
                //           Util.QuaternionSafeLookRotation(aimRay.direction),
                //           base.gameObject,
                //           1f,
                //           0f,
                //           base.RollCrit(),
                //           DamageColorIndex.Default,
                //           null,
                //           speedOverride);
                energySystem.currentOverheat += energyCost;

                new PlaySoundNetworkRequest(base.characterBody.netId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);

                base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);



                bool hasHit = false;
                Vector3 hitPoint = Vector3.zero;
                float hitDistance = 0f;
                HealthComponent hitHealthComponent = null;

                bulletAttack = new BulletAttack
                {
                    bulletCount = (uint)(1U),
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageStat,
                    damageColorIndex = DamageColorIndex.Count,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = 70f,
                    force = 0f,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0.1f,
                    radius = 1.5f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = Modules.Assets.arsonistFlare,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,

                };

                DamageAPI.AddModdedDamageType(bulletAttack, Damage.arsonistStickyDamageType);

                //bulletAttack.hitCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit hitInfo)
                //{
                //    var result = BulletAttack.defaultHitCallback(bulletAttackRef, ref hitInfo);
                //    if (hitInfo.hitHurtBox)
                //    {
                //        hasHit = true;
                //        hitPoint = hitInfo.point;
                //        hitDistance = hitInfo.distance;

                //        hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
                //        //hitHealthComponent.body.AddBuff();

                //    }
                //    return result;
                //};

                //bulletAttack.filterCallback = delegate (BulletAttack bulletAttackRef, ref BulletAttack.BulletHit info)
                //{
                //    return (!info.entityObject || info.entityObject != bulletAttack.owner) && BulletAttack.defaultFilterCallback(bulletAttackRef, ref info);
                //};
                //bulletAttack.Fire();
                //if (hasHit)
                //{
                //    if (hitHealthComponent != null)
                //    {
                //        FlareEffectController flareCon = hitHealthComponent.body.gameObject.AddComponent<FlareEffectController>();
                //        flareCon.arsonistBody = gameObject.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
                //        flareCon.charbody = hitHealthComponent.body;

                //    }
                //}

            }
        }
               

        public override void OnExit()
        {
            base.OnExit();
            //PlayCrossfade("RightArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            PlayCrossfade("LeftArm, Override", "BufferEmpty", "Attack.playbackRate", 0.1f, 0.1f);
            
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!hasFired && base.fixedAge > fireTime && base.isAuthority)
            {
                hasFired = true;
                bulletAttack.Fire();
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
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
using ArsonistMod.Modules;
using R2API.Networking;
using ArsonistMod.Modules.Networking;
using R2API.Networking.Interfaces;

namespace ArsonistMod.SkillStates
{
    public class Cleanse : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 1f;
        public float duration;



        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration;

            base.characterBody.SetAimTimer(this.duration);

            //Cleanse regardless of energy.
            if (NetworkServer.active) 
            {
                Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
                base.characterBody.ApplyBuff(RoR2Content.Buffs.OnFire.buffIndex, 0);
            }

            //energy
            if (energySystem.currentOverheat < energySystem.maxOverheat && base.isAuthority)
            {
                //cleanse, remove half of total energy, do blast attack
                energySystem.currentOverheat -= energySystem.maxOverheat * StaticValues.cleanseHeatReductionMultiplier;
                energySystem.hasOverheatedUtility = false;

                //enemy burn
                ApplyBurn();

                //self burn
                new BurnNetworkRequest(characterBody.master.netId, characterBody.master.netId).Send(NetworkDestination.Clients);
                

                //hop character to avoid fall damage if in air
                if (!characterBody.characterMotor.isGrounded)
                {
                    base.SmallHop(characterBody.characterMotor, 3f);
                }

                EffectManager.SpawnEffect(Modules.Assets.explosionPrefab, new EffectData
                {
                    origin = characterBody.transform.position,
                    scale = StaticValues.cleanseBlastRadius,
                    rotation = new Quaternion(0, 0, 0, 0)
                }, false);


            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && base.isAuthority)
            {
                //cleanse, no blast attack
                //making sure the cooldown is still the longer version
                //hop character to avoid fall damage if in air
                if (!characterBody.characterMotor.isGrounded)
                {
                    base.SmallHop(characterBody.characterMotor, 3f);
                }

                //Accelerate cooling
                energySystem.isAcceleratedCooling = true;
            }

        }

        public void ApplyBurn()
        {
            Ray aimRay = base.GetAimRay();
            BullseyeSearch search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = base.characterBody.footPosition,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = StaticValues.cleanseBlastRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);

            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        new BurnNetworkRequest(characterBody.master.netId, singularTarget.healthComponent.body.master.netId).Send(NetworkDestination.Clients);
                        //InflictDotInfo info = new InflictDotInfo();
                        //info.damageMultiplier = characterBody.damage * Modules.StaticValues.cleanseDamageCoefficient;
                        //info.attackerObject = base.gameObject;
                        //info.victimObject = singularTarget.healthComponent.body.gameObject;
                        //info.duration = Modules.StaticValues.cleanseDuration;
                        //info.dotIndex = DotController.DotIndex.Burn;

                        //DotController.InflictDot(ref info);
                    }
                }
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

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

    }
}
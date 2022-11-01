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

namespace ArsonistMod.SkillStates
{
    public class Cleanse : BaseSkillState
    {
        public EnergySystem energySystem;

        public float baseDuration = 0f;
        public float duration;



        public override void OnEnter()
        {
            base.OnEnter();
            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();

            Ray aimRay = base.GetAimRay();
            duration = baseDuration;

            base.characterBody.SetAimTimer(this.duration);

            ////blastattack
            //blastAttack = new BlastAttack();
            //blastAttack.radius = blastRadius;
            //blastAttack.procCoefficient = 0.1f;
            //blastAttack.position = base.transform.position;
            //blastAttack.attacker = base.gameObject;
            //blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            //blastAttack.baseDamage = base.characterBody.damage * 1f;
            //blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            //blastAttack.baseForce = 1f;
            //blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            //blastAttack.damageType = DamageType.IgniteOnHit;
            //blastAttack.attackerFiltering = AttackerFiltering.AlwaysHitSelf;

            //energy
            if (energySystem.currentOverheat < energySystem.maxOverheat && base.isAuthority)
            {
                //cleanse, remove half of total energy, do blast attack
                energySystem.currentOverheat -= energySystem.maxOverheat / 2f;
                energySystem.hasOverheatedUtility = false;

                if (NetworkServer.active)
                {
                    Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
                }

                //enemy burn
                ApplyBurn();
                //self burn
                InflictDotInfo info = new InflictDotInfo();
                info.damageMultiplier = characterBody.damage * Modules.StaticValues.cleanseDamageCoefficient;
                info.damageMultiplier = 1f;
                info.attackerObject = base.gameObject;
                info.victimObject = characterBody.gameObject;
                info.duration = Modules.StaticValues.cleanseDuration;
                info.dotIndex = DotController.DotIndex.Burn;
                DotController.InflictDot(ref info);

                //hop character to avoid fall damage if in air
                if (!characterBody.characterMotor.isGrounded)
                {
                    base.SmallHop(characterBody.characterMotor, 3f);
                }


            }
            else if (energySystem.currentOverheat == energySystem.maxOverheat && base.isAuthority)
            {
                //cleanse, reduce overheat timer, no blast attack
                //making sure the cooldown is still the longer version
                float remainingTimer = 5f - energySystem.overheatDecayTimer;
                energySystem.overheatDecayTimer += remainingTimer - 0.2f;
                if (NetworkServer.active)
                {
                    Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
                }
                //hop character to avoid fall damage if in air
                if (!characterBody.characterMotor.isGrounded)
                {
                    base.SmallHop(characterBody.characterMotor, 3f);
                }
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
                maxDistanceFilter = 4f,
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
                        InflictDotInfo info = new InflictDotInfo();
                        info.damageMultiplier = characterBody.damage * Modules.StaticValues.cleanseDamageCoefficient;
                        info.attackerObject = base.gameObject;
                        info.victimObject = singularTarget.healthComponent.body.gameObject;
                        info.duration = Modules.StaticValues.cleanseDuration;
                        info.dotIndex = DotController.DotIndex.Burn;

                        DotController.InflictDot(ref info);
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
            return InterruptPriority.PrioritySkill;
        }

    }
}
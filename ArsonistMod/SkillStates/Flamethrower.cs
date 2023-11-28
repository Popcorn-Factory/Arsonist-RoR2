using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static RoR2.BulletAttack;

namespace ArsonistMod.SkillStates
{
    internal class Flamethrower : BaseSkillState
    {

        //Fire a constant stream of bullet attacks
        //By holding down the button you constantly fire bullets
        //By tapping you fire a burst of x bullets
        //This fire ticking can be controlled according to attackspeed.
        public int tickRate;
        public int baseTickRate = Modules.StaticValues.flamethrowerBaseTickRate; // Per second.
        public float weakCoefficient = Modules.StaticValues.flamethrowerWeakDamageCoefficient;//per tick.
        public float strongCoefficient = Modules.StaticValues.flamethrowerStrongDamageCoefficient;
        public float altWeakCoefficient = Modules.StaticValues.altFlamethrowerWeakDamageCoefficient;
        public float altStrongCoefficient = Modules.StaticValues.altFlamethrowerStrongDamageCoefficient;
        internal BulletAttack bulletAttack;
        public float flamethrowerRange = Modules.StaticValues.flamethrowerRange;
        public float procCoefficient = Modules.StaticValues.flamethrowerProcCoefficient;
        public float radius = Modules.StaticValues.flamethowerRadius;
        public static float spreadBloomValue = 10f;

        private string muzzleString = "GunMuzzle";

        private float baseDuration = 0.5f;
        private float duration;

        private float interval;
        private float stopwatch = 0f;
        private EnergySystem energySystem;
        private ArsonistPassive passive;
        private ArsonistController controller;
        private bool isBlue;
        private float energyFlatCost;

        public float Energy = Modules.StaticValues.flamethrowerEnergyCost;
        private float energyCost;

        public override void OnEnter()
        {
            base.OnEnter();

            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();
            passive = characterBody.gameObject.GetComponent<ArsonistPassive>();
            controller = characterBody.gameObject.GetComponent<ArsonistController>();
            isBlue = passive.isBlueGauge();
            characterBody.isSprinting = false;

            //Calculate how much damage/stats whatever using the energy system 
            //energy
            energyFlatCost = Energy - energySystem.costflatOverheat;
            if (energyFlatCost < 0f) energyFlatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyFlatCost;
            if (energyCost < 0f) energyCost = 0f;

            duration = baseDuration / base.attackSpeedStat;
            tickRate = (int)(baseTickRate * base.attackSpeedStat);
            interval = duration / (float)tickRate;

            characterBody.SetAimTimer(duration * 2f);

            bulletAttack = new BulletAttack
            {
                bulletCount = 1,
                damage = strongCoefficient * this.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = flamethrowerRange,
                force = 0f,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = 1f,
                maxSpread = 4f,
                isCrit = base.RollCrit(),
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
                radius = radius,
                sniper = false,
                stopperMask = LayerIndex.noCollision.mask,
                weapon = null,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitCallback = flamethrowerFlameChanceHitCallback
            };

            if (energySystem.ifOverheatMaxed)
            {
                controller.weakFlamethrower.Play();
            }
            else 
            {
                controller.flamethrower.Play();
            }

            if (controller) 
            {
                if (!controller.playingFlamethrower) 
                {
                    controller.playingFlamethrower = true;
                    if (base.isAuthority) 
                    {
                        new PlaySoundNetworkRequest(characterBody.netId, 934132236).Send(R2API.Networking.NetworkDestination.Clients);
                        controller.flamethrowerPlayingID = AkSoundEngine.PostEvent(587096102, characterBody.gameObject);
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            controller.flamethrower.Stop();
            controller.weakFlamethrower.Stop();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Fire bullets in volleys over the duration of the move.
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= interval && base.isAuthority) 
            {
                float coeff = strongCoefficient;
                float range = flamethrowerRange;
                if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
                {
                    //Increment energy and Damage stuff
                    energySystem.AddHeat(energyCost / (float)tickRate);
                    coeff = isBlue ? altStrongCoefficient : strongCoefficient;
                }
                else if (energySystem.currentOverheat >= energySystem.maxOverheat && isAuthority)
                {
                    //Set damage stuff
                    coeff = isBlue ? altWeakCoefficient : weakCoefficient;
                    range = flamethrowerRange * 0.66f;
                }
                Ray aimRay = base.GetAimRay();
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.damage = coeff * this.damageStat;
                bulletAttack.maxDistance = range;
                bulletAttack.Fire();
                base.characterBody.AddSpreadBloom(spreadBloomValue);
                stopwatch = 0f;
            }

            if (base.fixedAge >= this.duration && base.isAuthority) 
            {
                if (base.inputBank.skill1.down) 
                {
                    this.outer.SetState(new Flamethrower { });
                    return;
                }
                controller.flamethrower.Stop();
                controller.weakFlamethrower.Stop();
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public bool flamethrowerFlameChanceHitCallback(BulletAttack bulletRef, ref BulletHit hitInfo)
        {
            //Default damage
            BulletAttack.defaultHitCallback.Invoke(bulletRef, ref hitInfo);
            if (hitInfo.hitHurtBox)
            {
                //Check the distance between the enemy and player, then plug into an inverse parabola to determine how high of a proc chance.
                //Clamped so we always get a clean 0-1 value.
                float ratio = hitInfo.distance / Modules.StaticValues.flamethrowerRange;

                float dist_chance = (-1f * Mathf.Pow(ratio, 0.8f)) + 1.2f;
                dist_chance = Mathf.Clamp(dist_chance, 0f, 1f);

                //Attempt to deal Fire
                float randomNum = UnityEngine.Random.Range(1f, 100f);
                float luck = characterBody.master.luck;
                if (luck < 1.0f)
                {
                    luck = 1.0f;
                }
                if (randomNum * luck * dist_chance >= Modules.StaticValues.flamethrowerFireChance)
                {
                    if (hitInfo.hitHurtBox.healthComponent.body.teamComponent.teamIndex != TeamIndex.Player)
                    {
                        //InflictDotInfo info = new InflictDotInfo();
                        //info.attackerObject = characterBody.gameObject;
                        //info.victimObject = hitInfo.hitHurtBox.healthComponent.body.gameObject;
                        //info.duration = 5.0f;
                        //info.damageMultiplier = 1.0f;
                        //info.dotIndex = DotController.DotIndex.Burn;
                        //RoR2.StrengthenBurnUtils.CheckDotForUpgrade(characterBody.inventory, ref info);
                        //DotController.InflictDot(ref info);

                        if (hitInfo.hitHurtBox.healthComponent.body.master) 
                        {
                            new FlamethrowerDotNetworkRequest(
                            characterBody.master.netId,
                            hitInfo.hitHurtBox.healthComponent.body.master.netId,
                            5f)
                            .Send(R2API.Networking.NetworkDestination.Clients);
                        }
                        return true;
                    }
                }
                return true;
            }

            return false;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

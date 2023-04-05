using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using EntityStates;
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
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private string muzzleString = "GunMuzzle";

        private float baseDuration = 0.5f;
        private float duration;

        private float interval;
        private float stopwatch = 0f;
        private EnergySystem energySystem;
        private ArsonistPassive passive;
        private bool isBlue;
        private float energyFlatCost;

        public float Energy = Modules.StaticValues.flamethrowerEnergyCost;
        private float energyCost;

        public override void OnEnter()
        {
            base.OnEnter();

            energySystem = characterBody.gameObject.GetComponent<EnergySystem>();
            passive = characterBody.gameObject.GetComponent<ArsonistPassive>();
            isBlue = passive.isBlueGauge();

            //Calculate how much damage/stats whatever using the energy system 
            //energy
            energyFlatCost = Energy - energySystem.costflatOverheat;
            if (energyFlatCost < 0f) energyFlatCost = 0f;

            energyCost = energySystem.costmultiplierOverheat * energyFlatCost;
            if (energyCost < 0f) energyCost = 0f;

            duration = baseDuration / base.attackSpeedStat;
            tickRate = (int)(baseTickRate * base.attackSpeedStat);
            interval = duration / (float)tickRate;

            characterBody.SetAimTimer(duration);

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
                maxSpread = 1f,
                isCrit = base.RollCrit(),
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
                radius = radius,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = tracerEffectPrefab,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                hitCallback = laserHitCallback
            };
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Fire bullets in volleys over the duration of the move.
            stopwatch += Time.fixedDeltaTime;

            if (stopwatch >= interval && base.isAuthority) 
            {
                float coeff = strongCoefficient;
                if (energySystem.currentOverheat < energySystem.maxOverheat && isAuthority)
                {
                    //Increment energy and Damage stuff
                    energySystem.currentOverheat += energyCost;
                    coeff = isBlue ? altStrongCoefficient : strongCoefficient;
                }
                else if (energySystem.currentOverheat >= energySystem.maxOverheat && isAuthority)
                {
                    //Set damage stuff
                    coeff = isBlue ? altWeakCoefficient : weakCoefficient;
                }
                Ray aimRay = base.GetAimRay();
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.damage = coeff * this.damageStat;
                bulletAttack.Fire();
                stopwatch = 0f;
            }

            if (base.fixedAge >= this.duration && base.isAuthority) 
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public bool laserHitCallback(BulletAttack bulletRef, ref BulletHit hitInfo)
        {
            //Default damage
            BulletAttack.defaultHitCallback.Invoke(bulletRef, ref hitInfo);
            if (hitInfo.hitHurtBox)
            {
                //Attempt to deal Fire
                float randomNum = UnityEngine.Random.Range(1f, 100f);
                float luck = characterBody.master.luck;
                if (luck < 1.0f)
                {
                    luck = 1.0f;
                }
                if (randomNum * luck >= Modules.StaticValues.flamethrowerFireChance)
                {
                    if (hitInfo.hitHurtBox.healthComponent.body.teamComponent.teamIndex != TeamIndex.Player)
                    {
                        InflictDotInfo info = new InflictDotInfo();
                        info.attackerObject = characterBody.gameObject;
                        info.victimObject = hitInfo.hitHurtBox.healthComponent.body.gameObject;
                        info.duration = 5.0f;
                        info.damageMultiplier = 1.0f;
                        info.dotIndex = DotController.DotIndex.Burn;
                        RoR2.StrengthenBurnUtils.CheckDotForUpgrade(characterBody.inventory, ref info);
                        DotController.InflictDot(ref info);
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

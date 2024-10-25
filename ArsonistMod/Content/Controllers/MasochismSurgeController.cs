using ArsonistMod.Modules;
using ArsonistMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    //Added from Energy System
    public class MasochismSurgeController : MonoBehaviour
    {
        public CharacterBody characterBody;
        public SkillLocator skillLocator;
        public EnergySystem energySystem;
        public HealthComponent healthComponent;

        //Masochism monitoring
        public float heatChanged;
        public int masoStacks;
        public bool masochismActive;
        public bool forceReset = false;

        //Actual Masochism Attacks
        public float damageOverTimeStopwatch;
        public float selfDamageStopwatch;
        public BlastAttack finalBlastAttack;
        public float stopwatch;

        //Sound loop
        public uint masochismActiveLoop;

        public void Start() 
        {
            skillLocator = GetComponent<SkillLocator>();
            characterBody = GetComponent<CharacterBody>();
            healthComponent = GetComponent<HealthComponent>();
            energySystem = GetComponent<EnergySystem>();

            Hook();

            //Final blast
            finalBlastAttack = new BlastAttack
            {
                attacker = this.gameObject,
                inflictor = null,
                teamIndex = TeamIndex.Player,
                radius = Modules.StaticValues.masochismSurgeBlastRadius,
                falloffModel = BlastAttack.FalloffModel.None,
                baseDamage = characterBody.damage * Modules.StaticValues.masochismSurgeBlastRadius,
                baseForce = 0f,
                bonusForce = Vector3.up,
                damageType = DamageType.IgniteOnHit,
                damageColorIndex = DamageColorIndex.Default,
                canRejectForce = false,
                procCoefficient = 1f
            };
        }

        public void Update()
        { 

        }

        public void FixedUpdate()
        {
            if (characterBody.hasEffectiveAuthority)
            {
                MasochismBuffApplication();
                DetermineMasoActivateable();

                if (masochismActive)
                {
                    RunMasochismLoop();
                }
                else 
                {
                    DisableMasochism();
                }
            }
        }

        private void RunMasochismLoop()
        {
            selfDamageStopwatch += Time.fixedDeltaTime;
            stopwatch += Time.fixedDeltaTime;

            //Apply appropriate buff
            characterBody.ApplyBuff(Modules.Buffs.masochismSurgeActiveBuff.buffIndex, 1, -1f);

            // Self inflict damage 
            if (selfDamageStopwatch >= Modules.StaticValues.masochismBasePulseSelfDamageTimer)
            {
                new TakeDamageNetworkRequest(characterBody.master.netId, characterBody.master.netId, healthComponent.fullHealth * Modules.StaticValues.masochismSurgeSelfDamage, false, true, false).Send(NetworkDestination.Clients);
                selfDamageStopwatch = 0f;
            }

            // Accumulate heat over time
            energySystem.AddHeat(energySystem.maxOverheat * Modules.StaticValues.masochismEnergyIncreaseOverTimePercentage * Time.fixedDeltaTime);

            if (stopwatch >= (float)masoStacks)
            {
                TriggerMasochismAndEXOverheat(false);
            }

            if (energySystem.ifOverheatMaxed)
            {
                TriggerMasochismAndEXOverheat(true);
            }
        }

        public void TriggerMasochismAndEXOverheat(bool applyDebuff)
        {
            //Disable early in case the other systems return null and break this shit.
            masochismActive = false;
            stopwatch = 0f;
            damageOverTimeStopwatch = 0f;
            energySystem.lowerBound = 0f;
            energySystem.ifOverheatRegenAllowed = true;

            int masoStacksAccumulated = masoStacks;

            masoStacks = 0;
            heatChanged = 0f;
            forceReset = true;

            // Trigger EX OVERHEAT (hamper movement speed, decrease damage output) for short period of time
            energySystem.AddHeat(energySystem.maxOverheat * 2f);
            AkSoundEngine.StopPlayingID(masochismActiveLoop);
            new PlaySoundNetworkRequest(characterBody.netId, 3765159379).Send(NetworkDestination.Clients);

            //Trigger massive explosion around Arsonist Scales according to stacks maintained.
            finalBlastAttack.position = gameObject.transform.position;
            finalBlastAttack.crit = characterBody.RollCrit();
            finalBlastAttack.baseDamage = characterBody.baseDamage * Modules.StaticValues.masochismSurgeFinalBlastCoefficient * Modules.StaticValues.masochismSurgeMultiplierPerStack * masoStacksAccumulated;
            float radMultiplier = Mathf.Lerp(1f, Modules.StaticValues.masochismSurgeMaxMultipliedRange, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
            finalBlastAttack.radius = Modules.StaticValues.masochismSurgeBlastRadius * radMultiplier;
            finalBlastAttack.damageType = DamageType.IgniteOnHit;

            finalBlastAttack.Fire();
            //Debug.Log($"Applied damage on blast: {finalBlastAttack.baseDamage} masoStack: {masoStacksAccumulated} radMultiplier: {finalBlastAttack.radius}");


            //Apply the debuff
            try
            {
                if (applyDebuff)
                {
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedDebuff.buffIndex, 1, -1);
                }
                else
                {
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedNonDebuff.buffIndex, 1, -1);
                }


                characterBody.ApplyBuff(Modules.Buffs.masochismSurgeActiveBuff.buffIndex, 0, -1f);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public void ActivateMaso()
        {
            // Heat raised must be raised by 15%.
            masochismActive = true;
            energySystem.lowerBound = energySystem.maxOverheat * Modules.StaticValues.masochismActiveLowerBoundHeat;
            energySystem.ifOverheatRegenAllowed = false;


            masochismActiveLoop = AkSoundEngine.PostEvent(1419365914, characterBody.gameObject);
        }


        public void DisableMasochism()
        {
            if (characterBody.hasEffectiveAuthority)
            {
                //Remove the buff if they're not overheated.
                if (!energySystem.ifOverheatMaxed)
                {
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedDebuff.buffIndex, 0, -1);
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedNonDebuff.buffIndex, 0, -1);
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self)
            {
                if (self.body)
                {
                    #region Neo-masochism

                    // Arsonist will heal from damage dealt
                    if (damageInfo.attacker)
                    {
                        CharacterBody attackerCharacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                        if (attackerCharacterBody)
                        {
                            bool damageTypeCheck = damageInfo.damageType == (DamageType.Generic | DamageType.AOE | DamageType.DoT);
                            if (attackerCharacterBody.HasBuff(Modules.Buffs.masochismSurgeActiveBuff) && attackerCharacterBody.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME" && !damageTypeCheck)
                            {
                                attackerCharacterBody.healthComponent.Heal(damageInfo.damage * Modules.Config.masochismSurgeHealOnHitPercentage.Value, new ProcChainMask(), true);
                            }
                        }
                    }
                    #endregion
                }
            }

            orig(self, damageInfo);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {

            if (self.healthComponent)
            {
                orig(self);

                if (self)
                {
                    if (self.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                    {
                        // increase damage for a set amount of time
                        if (self.HasBuff(Modules.Buffs.masochismSurgeActiveBuff))
                        {
                            self.moveSpeed *= StaticValues.masochismSurgeMoveSpeedMultiplier;
                        }

                        //Debuff arsonist in EX overheat
                        if (self.HasBuff(Modules.Buffs.masochismDeactivatedDebuff))
                        {
                            self.damage *= StaticValues.masochismDamagePenalty;
                            self.moveSpeed *= StaticValues.masochismMoveSpeedPenalty;
                        }
                    }

                }
            }
        }

        public void DetermineMasoActivateable()
        {
            if (masoStacks >= Modules.Config.masochismMinimumRequiredToActivate.Value)
            {
                if (skillLocator.special.stock < 1)
                {
                    skillLocator.special.AddOneStock();
                }
            }
            else
            {
                skillLocator.special.stock = 0;
            }

        }

        public void MasochismBuffApplication()
        {
            //Ensure Stacks never exceed 10.
            //Calculate if we need to add to stacks.
            if (heatChanged > Modules.Config.masochismHeatChangedThreshold.Value)
            {
                masoStacks++;
                heatChanged -= Modules.Config.masochismHeatChangedThreshold.Value;
            }

            if (masoStacks > Modules.Config.masochismMaximumStack.Value)
            {
                masoStacks = Modules.Config.masochismMaximumStack.Value;
            }

            if (forceReset)
            {
                forceReset = false;
                masoStacks = 0;
                heatChanged = 0f;
            }

            //Apply the maso Stacks as buffs in a stack with no duration 
            characterBody.ApplyBuff(Modules.Buffs.newMasochismBuff.buffIndex, masoStacks, -1);
        }


        private void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void Unhook() 
        {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
        }

        public void OnDestroy() 
        {
            Unhook();
        }
    }
}

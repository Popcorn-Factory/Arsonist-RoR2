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
    public class SpiteController : MonoBehaviour
    {
        public CharacterBody characterBody;
        public SkillLocator skillLocator;
        public EnergySystem energySystem;
        public HealthComponent healthComponent;
        public ArsonistController arsonistController;

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

        public int masoStackOnUse;

        public void Start() 
        {
            skillLocator = GetComponent<SkillLocator>();
            characterBody = GetComponent<CharacterBody>();
            healthComponent = GetComponent<HealthComponent>();
            energySystem = GetComponent<EnergySystem>();
            arsonistController = GetComponent<ArsonistController>();

            Hook();
            ResetMaterial();

            //Final blast
            finalBlastAttack = new BlastAttack
            {
                attacker = this.gameObject,
                inflictor = null,
                teamIndex = TeamIndex.Player,
                radius = Modules.StaticValues.masochismSurgeBlastRadius,
                falloffModel = BlastAttack.FalloffModel.None,
                baseDamage = characterBody.damage * Modules.StaticValues.masochismSurgeBlastRadius,
                baseForce = 1000f,
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
            damageOverTimeStopwatch += Time.fixedDeltaTime;

            //Apply appropriate buff
            characterBody.ApplyBuff(Modules.Buffs.masochismSurgeActiveBuff.buffIndex, 1, -1f);

            MaterialChangeOverTime();

            // Self inflict damage 
            if (selfDamageStopwatch >= Modules.StaticValues.masochismBasePulseSelfDamageTimer)
            {
                new TakeDamageNetworkRequest(characterBody.master.netId, characterBody.master.netId, healthComponent.fullHealth * Modules.StaticValues.masochismSurgeSelfDamage, false, true, false).Send(NetworkDestination.Clients);
                selfDamageStopwatch = 0f;
            }


            // What is my purpose?
            // You pulse an SFX over time.
            // Oh my god.
            if (damageOverTimeStopwatch >= Modules.StaticValues.spiteBasePulseTimer)
            {
                new PlaySoundNetworkRequest(characterBody.netId, "Arsonist_Spite_Pulse").Send(NetworkDestination.Clients);
                damageOverTimeStopwatch = 0f;
            }

            // Accumulate heat over time
            energySystem.AddHeat(energySystem.maxOverheat * Modules.StaticValues.masochismEnergyIncreaseOverTimePercentage * Time.fixedDeltaTime);

            if (stopwatch >= (float)masoStackOnUse)
            {
                TriggerMasochismAndEXOverheat(false);
            }

            if (energySystem.ifOverheatMaxed)
            {
                TriggerMasochismAndEXOverheat(true);
            }
        }

        public void MaterialChangeOverTime() 
        {
            //Change the material of the character over time.
            Color firstOutline = Modules.StaticValues.firstOutlineColour;
            Color secondOutline = Modules.StaticValues.secondOutlineColour;

            firstOutline.a = Mathf.Lerp(0f, 0.5f, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
            secondOutline.a = Mathf.Lerp(0f, 0.5f, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);

            arsonistController.outlineMaterial.SetColor("_FirstOutlineColor", firstOutline);
            arsonistController.outlineMaterial.SetColor("_SecondOutlineColor", secondOutline);
        }

        public void ResetMaterial() 
        {
            arsonistController.outlineMaterial.SetColor("_FirstOutlineColor", Modules.StaticValues.firstOutlineColour);
            arsonistController.outlineMaterial.SetColor("_SecondOutlineColor", Modules.StaticValues.secondOutlineColour);
        }

        public void TriggerMasochismAndEXOverheat(bool applyDebuff)
        {
            //Do not reset stacks, instead delete stacks that have been used depending on time spent in maso surge.
            int stocksToRemove = (int)stopwatch;
         
            //Disable early in case the other systems return null and break this shit.
            masochismActive = false;
            stopwatch = 0f;
            damageOverTimeStopwatch = 0f;
            energySystem.lowerBound = 0f;
            energySystem.ifOverheatRegenAllowed = true;

            // Trigger EX OVERHEAT (hamper movement speed, decrease damage output) for short period of time
            energySystem.AddHeat(energySystem.maxOverheat * 2f);
            // Prevent overheat from adding heat gain to gauge before this happens.
            energySystem.disableHeatGainMaso = false;

            int masoStacksAccumulated = masoStackOnUse;

            ResetMaterial();


            if (stocksToRemove > Modules.Config.masochismMaximumStack.Value) 
            {
                stocksToRemove = Modules.Config.masochismMaximumStack.Value;
            }

            if (stocksToRemove <= 0) 
            {
                stocksToRemove = 1;
            }

            //Debug.Log($"Masostacks: {masoStacks} StocksToRemove: {stocksToRemove} masoStackOnUse: {masoStackOnUse}");

            masoStacks -= stocksToRemove;
            if (masoStacks < 0) 
            {
                masoStacks = 0;
            }

            heatChanged = 0f;

            AkSoundEngine.StopPlayingID(masochismActiveLoop);
            new PlaySoundNetworkRequest(characterBody.netId, "Arsonist_Spite_End").Send(NetworkDestination.Clients);

            //Trigger massive explosion around Arsonist Scales according to stacks maintained.
            finalBlastAttack.position = gameObject.transform.position;
            finalBlastAttack.crit = characterBody.RollCrit();
            finalBlastAttack.baseDamage = characterBody.baseDamage * Modules.StaticValues.masochismSurgeFinalBlastCoefficient * Modules.StaticValues.masochismSurgeMultiplierPerStack * masoStacksAccumulated;
            float radMultiplier = Mathf.Lerp(1f, Modules.StaticValues.masochismSurgeMaxMultipliedRange, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
            finalBlastAttack.radius = Modules.StaticValues.masochismSurgeBlastRadius * radMultiplier;
            finalBlastAttack.damageType = DamageType.IgniteOnHit;

            finalBlastAttack.Fire();
            //Debug.Log($"Applied damage on blast: {finalBlastAttack.baseDamage} masoStack: {masoStacksAccumulated} radMultiplier: {finalBlastAttack.radius}");
            
            EffectManager.SpawnEffect(Modules.AssetsArsonist.masoExplosionBlue,
                new EffectData
                {
                    origin = gameObject.transform.position,
                    rotation = Quaternion.identity,
                    scale = Modules.StaticValues.masochismSurgeBlastRadius * radMultiplier
                }, true);

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
            energySystem.disableHeatGainMaso = true;

            masoStackOnUse = masoStacks;

            masoStacks--;

            // Play Sound: Arsonist_Spite_Duration
            masochismActiveLoop = AkSoundEngine.PostEvent(4157809569, characterBody.gameObject);
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
            else if (masochismActive) 
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

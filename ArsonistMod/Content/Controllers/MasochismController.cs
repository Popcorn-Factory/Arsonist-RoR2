using ArsonistMod.Modules;
using MonoMod.RuntimeDetour;
using R2API.Networking;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class MasochismController : MonoBehaviour
    {
        //energy system
        public EnergySystem energySystem;

        //CharacterBody 
        public CharacterBody characterBody;

        //HealthComponent
        public HealthComponent healthComponent;

        //Skill Loc
        public SkillLocator skillLoc;

        //Masochism monitoring
        public float heatChanged;
        public int masoStacks;
        public bool masochismActive;

        //Actual Masochism Attacks
        public float damageOverTimeStopwatch;
        public float selfDamageStopwatch;
        public BlastAttack damageOverTimeSphere;
        public BlastAttack finalBlastAttack;
        public float stopwatch;

        public void Awake()
        {

        }

        public void Start()
        {
            energySystem = GetComponent<EnergySystem>();
            characterBody = GetComponent<CharacterBody>();
            healthComponent = characterBody.healthComponent;
            skillLoc = characterBody.skillLocator;
            Hook();

            //Short Range Blast attack
            damageOverTimeSphere = new BlastAttack
            {
                attacker = this.gameObject,
                inflictor = null,
                teamIndex = TeamIndex.Player,
                radius = Modules.StaticValues.masochismPulseRadius,
                falloffModel = BlastAttack.FalloffModel.None,
                baseDamage = characterBody.damage * Modules.StaticValues.masochismPulseCoefficient,
                baseForce = 0f,
                bonusForce = Vector3.up,
                damageType = DamageType.IgniteOnHit,
                damageColorIndex = DamageColorIndex.Default,
                canRejectForce = false,
                procCoefficient = 0.4f
            };
            
            //Final blast
            finalBlastAttack = new BlastAttack 
            {
                attacker = this.gameObject,
                inflictor = null,
                teamIndex = TeamIndex.Player,
                radius = Modules.StaticValues.masochismPulseRadius,
                falloffModel = BlastAttack.FalloffModel.None,
                baseDamage = characterBody.damage * Modules.StaticValues.masochismPulseCoefficient,
                baseForce = 0f,
                bonusForce = Vector3.up,
                damageType = DamageType.IgniteOnHit,
                damageColorIndex = DamageColorIndex.Default,
                canRejectForce = false,
                procCoefficient = 1f
            };
        }

        public void Hook() 
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            if (self) 
            {
                if (self.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME" && self.HasBuff(Modules.Buffs.masochismActiveBuff))
                {
                    // increase damage for a set amount of time
                    self.damage *= StaticValues.masochismDamageBoost;
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
                    CharacterBody attackerCharacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (attackerCharacterBody)
                    {
                        if (attackerCharacterBody.HasBuff(Modules.Buffs.masochismActiveBuff) && attackerCharacterBody.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME")
                        {
                            attackerCharacterBody.healthComponent.Heal(damageInfo.damage * Modules.Config.masochismActiveMultipliedActive.Value, new ProcChainMask(), true);
                        }
                    }
                    #endregion
                }
            }
        }

        public void Unhook() 
        {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
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

        public void DisableMasochism() 
        {
            // UN apply buff.
        }

        public void RunMasochismLoop()
        {
            //Apply buff
            characterBody.ApplyBuff(Modules.Buffs.masochismActiveBuff.buffIndex, 1, -1f);

            damageOverTimeStopwatch += Time.fixedDeltaTime;
            selfDamageStopwatch += Time.fixedDeltaTime;
            stopwatch += Time.fixedDeltaTime;

            if (damageOverTimeStopwatch >= Modules.StaticValues.masochismBasePulseTimer * characterBody.attackSpeed)
            {
                damageOverTimeSphere.position = gameObject.transform.position;
                damageOverTimeSphere.crit = characterBody.RollCrit();
                damageOverTimeSphere.baseDamage = characterBody.baseDamage * Modules.StaticValues.masochismPulseCoefficient;

                damageOverTimeSphere.Fire();
                damageOverTimeStopwatch = 0f;
            }

            // Radiate Heat DONE
            // increase damage for a set amount of time DONE
            // Arsonist will heal from damage dealt DONE

            // Self inflict damage 
            if (selfDamageStopwatch >= Modules.StaticValues.masochismBasePulseSelfDamageTimer) 
            {
                healthComponent.TakeDamage(new DamageInfo
                {
                    damage = healthComponent.fullHealth * Modules.StaticValues.masochismSelfDamage,
                    crit = false,
                    inflictor = null,
                    attacker = this.gameObject,
                    position = gameObject.transform.position,
                    force = Vector3.zero,
                    rejected = false,
                    procChainMask = new ProcChainMask(),
                    damageType = DamageType.Generic,
                    damageColorIndex = DamageColorIndex.Bleed,
                    canRejectForce = false
                });
            }

            // Accumulate heat over time
            energySystem.AddHeat(energySystem.maxOverheat * Modules.StaticValues.masochismEnergyIncreaseOverTimePercentage * Time.fixedDeltaTime);


            if (stopwatch >= (float)masoStacks) 
            {
                TriggerMasochismAndEXOverheat();
            }
        }

        public void TriggerMasochismAndEXOverheat() 
        {
            //Trigger massive explosion around Arsonist Scales according to stacks maintained.
            // Exhaust all maso stocks
            // Trigger EX OVERHEAT (hamper movement speed, decrease damage output) for short period of time

            masochismActive = false;
            damageOverTimeStopwatch = 0f;
            energySystem.lowerBound = 0f;
            energySystem.ifOverheatRegenAllowed = true;

        }

        public void DetermineMasoActivateable() 
        {
            if (masoStacks >= Modules.Config.masochismMinimumRequiredToActivate.Value)
            {
                if (skillLoc.special.stock < 1)
                {
                    skillLoc.special.AddOneStock();
                }
            }
            else 
            {
                skillLoc.special.stock = 0;
            }

        }

        public void ActivateMaso() 
        {
            // Heat raised must be raised by 15%.
            masochismActive = true;
            energySystem.lowerBound = 15f;
            energySystem.ifOverheatRegenAllowed = false;
        }

        public void MasochismActiveLoop() 
        {
        
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

            if (masoStacks >= Modules.Config.masochismMinimumRequiredToActivate.Value)
            {
                //I dunno change the skill locator or something.
            }
            else
            {
                //Do the opposite I guess.
            }

            //Apply the maso Stacks as buffs in a stack with no duration 
            characterBody.ApplyBuff(Modules.Buffs.newMasochismBuff.buffIndex, masoStacks, -1);
        }

        public void OnDestroy() 
        {
            Unhook();
        }
    }
}

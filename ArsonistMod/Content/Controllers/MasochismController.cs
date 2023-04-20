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

        //Skill Loc
        public SkillLocator skillLoc;

        //Masochism monitoring
        public float heatChanged;
        public int masoStacks;
        public bool masochismActive;

        //Actual Masochism Attacks
        public float damageOverTimeStopwatch;
        public BlastAttack damageOverTimeSphere;
        public BlastAttack finalBlastAttack;

        public void Awake()
        {

        }

        public void Start()
        {
            energySystem = GetComponent<EnergySystem>();
            characterBody = GetComponent<CharacterBody>();
            skillLoc = characterBody.skillLocator;

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

            if (damageOverTimeStopwatch >= Modules.StaticValues.masochismBasePulseTimer * characterBody.attackSpeed) 
            {
                damageOverTimeSphere.position = gameObject.transform.position;
                damageOverTimeSphere.crit = characterBody.RollCrit();

                damageOverTimeSphere.Fire();
                damageOverTimeStopwatch = 0f;
            }

            // Radiate Heat
            // increase damage for a set amount of time
            // Arsonist will heal from damage dealt
            // Self inflict damage 
            // Accumulate heat over time
            // Heat raised must be raised by 15%.
        }

        public void TriggerFinalMsaochismAndReset() 
        {
            //Trigger massive explosion around Arsonist Scales according to stacks maintained.
            // Exhaust all maso stocks
            // Trigger EX OVERHEAT (hamper movement speed, decrease damage output) for short period of time
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
            masochismActive = true;
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

    }
}

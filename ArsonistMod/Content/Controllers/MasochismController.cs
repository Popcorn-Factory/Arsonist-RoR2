using ArsonistMod.Modules;
using ArsonistMod.Modules.Networking;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
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

        //Arsonist Controller
        public ArsonistController arsonistCon;

        //Masochism monitoring
        public float heatChanged;
        public int masoStacks;
        public bool masochismActive;
        public bool forceReset = false;

        //Temp: Masochism range Indicator.
        public GameObject masochismRangeIndicator;
        public ParticleSystem masochismEffect;
        public ParticleSystem heatHazeEffect;
        public Transform pulseEffect;

        //Actual Masochism Attacks
        public float damageOverTimeStopwatch;
        public float selfDamageStopwatch;
        public BlastAttack damageOverTimeSphere;
        public BlastAttack finalBlastAttack;
        public float stopwatch;

        public uint masochismActiveLoop;

        public bool masoRecentlyActivated;
        public float masoRadiusStopwatch = 0f;
        public static float masoRadiusRampUpTime = 0.8f;

        private CameraTargetParams.CameraParamsOverrideHandle handle;
        public CameraTargetParams cameraTargetParams;

        public float targetFOV;
        public float baseFOV;

        public bool scepterUpgrade;

        public bool shouldBeActive;

        public DamageTypeCombo AOEDamageType;

        public void Awake()
        {

        }

        public void Start()
        {
            energySystem = GetComponent<EnergySystem>();
            characterBody = GetComponent<CharacterBody>();
            cameraTargetParams = GetComponent<CameraTargetParams>();
            healthComponent = characterBody.healthComponent;
            skillLoc = characterBody.skillLocator;

            masochismRangeIndicator = UnityEngine.Object.Instantiate<GameObject>(Modules.AssetsArsonist.masoSphereIndicator);
            masochismEffect = masochismRangeIndicator.GetComponent<ParticleSystem>();
            pulseEffect = masochismRangeIndicator.transform.GetChild(0);
            heatHazeEffect = masochismRangeIndicator.transform.GetChild(1).GetComponent<ParticleSystem>();

            masochismRangeIndicator.SetActive(false);
            new ToggleMasochismEffectNetworkRequest(characterBody.netId, false).Send(NetworkDestination.Clients);

            Hook();

            AOEDamageType = new DamageTypeCombo(DamageType.IgniteOnHit, DamageTypeExtended.Generic, DamageSource.Special);

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
                damageType = AOEDamageType,
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
                baseForce = 1000f,
                bonusForce = Vector3.up,
                damageType = AOEDamageType,
                damageColorIndex = DamageColorIndex.Default,
                canRejectForce = false,
                procCoefficient = 1f
            };

            arsonistCon = gameObject.GetComponent<ArsonistController>();

            // Check if the player has selected Masochism, or something else.
            // If not, disable the controller fixed update loop.
        }

        public void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
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
                        if (self.HasBuff(Modules.Buffs.masochismActiveBuff))
                        {
                            self.damage *= StaticValues.masochismDamageBoost;
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
                            bool damageTypeCheck = damageInfo.damageType.damageType == (DamageType.Generic | DamageType.AOE | DamageType.DoT);
                            if (attackerCharacterBody.HasBuff(Modules.Buffs.masochismActiveBuff) && attackerCharacterBody.baseNameToken == ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_NAME" && !damageTypeCheck)
                            {
                                attackerCharacterBody.healthComponent.Heal(damageInfo.damage * Modules.Config.masochismActiveMultipliedActive.Value, new ProcChainMask(), true);
                            }
                        }
                    }
                    #endregion
                }
            }

            orig(self, damageInfo);
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
                //Chat.AddMessage($"Masostacks:{masoStacks} heatChanged:{heatChanged}");


                if (masochismActive)
                {
                    RunMasochismLoop();
                }
                else
                {
                    DisableMasochism();
                }

                //Check if HeatHaze was disabled/enabled
                if (heatHazeEffect) 
                {
                    if (Modules.Config.enableAggressiveHeatHaze.Value != heatHazeEffect.isPlaying) 
                    {
                        if (Modules.Config.enableAggressiveHeatHaze.Value)
                        {
                            heatHazeEffect.Play();
                        }
                        else 
                        {
                            heatHazeEffect.Stop();
                        }
                    }
                }
            }

        }

        public void Update()
        {
            //update the indicator if active
            if (masochismActive || !characterBody.hasEffectiveAuthority)
            {
                //Slowly ramp up size for the first second.
                float rampMultiplier = 1f;
                if (masoRecentlyActivated)
                {
                    masoRadiusStopwatch += Time.deltaTime;
                    rampMultiplier = Mathf.Clamp01(Mathf.Lerp(0f, 1f, masoRadiusStopwatch / masoRadiusRampUpTime));
                    if (masoRadiusStopwatch >= masoRadiusRampUpTime)
                    {
                        rampMultiplier = 1f;
                        masoRecentlyActivated = false;
                        masoRadiusStopwatch = 0f;
                    }
                }

                float radMultiplier = Mathf.Lerp(1f, Modules.StaticValues.masochismMaxMultipliedRange, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
                masochismRangeIndicator.transform.localScale = Vector3.one * Modules.StaticValues.masochismPulseRadius * radMultiplier * rampMultiplier;
                masochismRangeIndicator.transform.position = this.gameObject.transform.position;
                pulseEffect.localScale = Vector3.one * Modules.StaticValues.masochismPulseRadius * radMultiplier * rampMultiplier;
            }

        }

        public void ToggleMasochismRangeIndicator(bool value) 
        {
            if (masochismRangeIndicator) 
            {
                masochismRangeIndicator.SetActive(value);
            }
        }

        public void DisableMasochism() 
        {
            if (characterBody.hasEffectiveAuthority) 
            {

                masochismRangeIndicator.SetActive(false);
                new ToggleMasochismEffectNetworkRequest(characterBody.netId, false).Send(NetworkDestination.Clients);
                //Remove the buff if they're not overheated.
                if (!energySystem.ifOverheatMaxed) 
                {
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedDebuff.buffIndex, 0, -1);
                    characterBody.ApplyBuff(Modules.Buffs.masochismDeactivatedNonDebuff.buffIndex, 0, -1);
                }
            }
        }

        public void RunMasochismLoop()
        {
            if (scepterUpgrade && !damageOverTimeSphere.HasModdedDamageType(Modules.Damage.arsonistScepterMasochismPing)) 
            {
                damageOverTimeSphere.AddModdedDamageType(Modules.Damage.arsonistScepterMasochismPing);
            }

            if (!scepterUpgrade && damageOverTimeSphere.HasModdedDamageType(Modules.Damage.arsonistScepterMasochismPing))
            {
                damageOverTimeSphere.RemoveModdedDamageType(Modules.Damage.arsonistScepterMasochismPing);
            }

            //Apply buff
            characterBody.ApplyBuff(Modules.Buffs.masochismActiveBuff.buffIndex, 1, -1f);
            masochismRangeIndicator.SetActive(true);
            new ToggleMasochismEffectNetworkRequest(characterBody.netId, true).Send(NetworkDestination.Clients);

            damageOverTimeStopwatch += Time.fixedDeltaTime;
            selfDamageStopwatch += Time.fixedDeltaTime;
            stopwatch += Time.fixedDeltaTime;

            if (damageOverTimeStopwatch >= Modules.StaticValues.masochismBasePulseTimer)
            {
                new PlaySoundNetworkRequest(characterBody.netId, 1578712289).Send(NetworkDestination.Clients);

                damageOverTimeSphere.position = gameObject.transform.position;
                damageOverTimeSphere.crit = characterBody.RollCrit();
                damageOverTimeSphere.baseDamage = characterBody.baseDamage * Modules.StaticValues.masochismPulseCoefficient;
                float radMultiplier = Mathf.Lerp(1f, Modules.StaticValues.masochismMaxMultipliedRange, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
                damageOverTimeSphere.radius = Modules.StaticValues.masochismPulseRadius * radMultiplier;

                damageOverTimeSphere.Fire();
                damageOverTimeStopwatch = 0f;
            }

            // Radiate Heat DONE
            // increase damage for a set amount of time DONE
            // Arsonist will heal from damage dealt DONE

            // Self inflict damage 
            if (selfDamageStopwatch >= Modules.StaticValues.masochismBasePulseSelfDamageTimer) 
            {
                new TakeDamageNetworkRequest(characterBody.master.netId, characterBody.master.netId, healthComponent.fullHealth * Modules.StaticValues.masochismSelfDamage, false, true, false).Send(NetworkDestination.Clients);
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

            if (characterBody.hasEffectiveAuthority && Modules.Config.ToggleMasochismFOVWarp.Value)
            {
                cameraTargetParams.RemoveParamsOverride(handle);
            }

            //Trigger massive explosion around Arsonist Scales according to stacks maintained.
            finalBlastAttack.position = gameObject.transform.position;
            finalBlastAttack.crit = characterBody.RollCrit();
            finalBlastAttack.baseDamage = characterBody.baseDamage * Modules.StaticValues.masochismFinalBlastCoefficient * Modules.StaticValues.masochismDamageMultiplierPerStack * masoStacksAccumulated;
            float radMultiplier = Mathf.Lerp(1f, Modules.StaticValues.masochismMaxMultipliedRange, stopwatch / (float)Modules.Config.masochismMaximumStack.Value);
            finalBlastAttack.radius = Modules.StaticValues.masochismPulseRadius * radMultiplier;
            finalBlastAttack.damageType = AOEDamageType;

            EffectManager.SpawnEffect(Modules.AssetsArsonist.masoExplosion,
            new EffectData
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = Modules.StaticValues.masochismPulseRadius * radMultiplier
            }, true);

            finalBlastAttack.Fire();
            //Debug.Log($"Applied damage on blast: {finalBlastAttack.baseDamage} masoStack: {masoStacksAccumulated} radMultiplier: {finalBlastAttack.radius}");

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


                characterBody.ApplyBuff(Modules.Buffs.masochismActiveBuff.buffIndex, 0, -1f);
            }
            catch (Exception ex) 
            {
                Debug.Log(ex);
            }
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
            energySystem.lowerBound = energySystem.maxOverheat * Modules.StaticValues.masochismActiveLowerBoundHeat;
            energySystem.ifOverheatRegenAllowed = false;
            

            masochismActiveLoop = AkSoundEngine.PostEvent(1419365914, characterBody.gameObject);

            //Set FOV to a multiplier amount for the duration of Maso



            if (characterBody.hasEffectiveAuthority && Modules.Config.ToggleMasochismFOVWarp.Value) 
            {
                CharacterCameraParamsData cameraParamsData = cameraTargetParams.currentCameraParamsData;
                cameraParamsData.fov = arsonistCon.cameraRigController.baseFov * Modules.StaticValues.masochismFOVHoldPosition;

                CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = cameraParamsData,
                    priority = 0,
                };
                handle = cameraTargetParams.AddParamsOverride(request, 0.5f);
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

            if (masoStacks >= Modules.Config.masochismMinimumRequiredToActivate.Value)
            {
                //I dunno change the skill locator or something.
            }
            else
            {
                //Do the opposite I guess.
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

        public void OnDestroy() 
        {
            Unhook();
            Destroy(masochismRangeIndicator);
            AkSoundEngine.StopPlayingID(masochismActiveLoop);
            new PlaySoundNetworkRequest(characterBody.netId, (uint)2176930590).Send(NetworkDestination.Clients);
        }
    }
}

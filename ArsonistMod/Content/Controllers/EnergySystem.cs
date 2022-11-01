using System;
using ArsonistMod.Modules;
using RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace ArsonistMod.Content.Controllers
{
    public class EnergySystem : MonoBehaviour
    {
        public CharacterBody characterBody;

        //UI energyMeter
        public GameObject CustomUIObject;
        public RectTransform energyMeter;
        public RectTransform energyMeterGlowRect;
        public Image energyMeterGlowBackground;
        public HGTextMeshProUGUI energyNumber;

        //Energy system
        public float maxOverheat;
        public float currentOverheat;
        public float regenOverheat;
        public float costmultiplierOverheat;
        public float costflatOverheat;
        public float overheatDecayTimer;
        public bool SetActiveTrue;
        public bool ifOverheatRegenAllowed;
        public bool ifOverheatMaxed;
        public bool hasOverheatedSecondary;
        public bool hasOverheatedUtility;
        public bool hasOverheatedSpecial;

        //Energy bar glow
        private enum GlowState
        {
            STOP,
            FLASH,
            DECAY
        }
        private float decayConst;
        private float flashConst;
        private float glowStopwatch;
        private Color targetColor;
        private Color originalColor;
        private Color currentColor;
        private GlowState state;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
        }

        public void Start()
        {
            //characterBody = gameObject.GetComponent<CharacterBody>();
            //characterMaster = characterBody.master;
            //if (!characterMaster.gameObject.GetComponent<NidusMasterController>())
            //{
            //    Nidusmastercon = characterMaster.gameObject.AddComponent<NidusMasterController>();
            //}


            //Energy
            maxOverheat = StaticValues.baseEnergy + ((characterBody.level - 1) * StaticValues.levelEnergy);
            currentOverheat = 0f;
            regenOverheat = characterBody.attackSpeed * StaticValues.regenOverheatFraction;
            costmultiplierOverheat = 1f;
            costflatOverheat = 0f;
            ifOverheatMaxed = false;
            ifOverheatRegenAllowed = true;
            hasOverheatedSecondary = false;
            hasOverheatedUtility = false;
            hasOverheatedSpecial = false;

            //UI objects 
            CustomUIObject = UnityEngine.Object.Instantiate(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("arsonistCustomUI"));
            CustomUIObject.SetActive(false);
            SetActiveTrue = false;

            energyMeter = CustomUIObject.transform.GetChild(0).GetComponent<RectTransform>();
            energyMeterGlowBackground = CustomUIObject.transform.GetChild(1).GetComponent<Image>();
            energyMeterGlowRect = CustomUIObject.transform.GetChild(1).GetComponent<RectTransform>();

            //setup the UI element for the min/max
            energyNumber = this.CreateLabel(CustomUIObject.transform, "energyNumber", $"{(int)currentOverheat} / {maxOverheat}", new Vector2(0, -110), 24f);
            

            // Start timer on 1f to turn off the timer.
            state = GlowState.STOP;
            decayConst = 1f;
            flashConst = 1f;
            glowStopwatch = 1f;
            originalColor = new Color(1f, 1f, 1f, 0f);
            targetColor = new Color(1f, 1f, 1f, 1f);
            currentColor = originalColor;

        }

        //Creates the label.
        private HGTextMeshProUGUI CreateLabel(Transform parent, string name, string text, Vector2 position, float textScale)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.AddComponent<CanvasRenderer>();
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            HGTextMeshProUGUI hgtextMeshProUGUI = gameObject.AddComponent<HGTextMeshProUGUI>();
            hgtextMeshProUGUI.enabled = true;
            hgtextMeshProUGUI.text = text;
            hgtextMeshProUGUI.fontSize = textScale;
            hgtextMeshProUGUI.color = Color.white;
            hgtextMeshProUGUI.alignment = TextAlignmentOptions.Center;
            hgtextMeshProUGUI.enableWordWrapping = false;
            rectTransform.localPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = position;
            return hgtextMeshProUGUI;
        }

        private void CalculateEnergyStats()
        {
            //Energy updates
            if (characterBody)
            {
                maxOverheat = StaticValues.baseEnergy + ((characterBody.level - 1) * StaticValues.levelEnergy)
                    + (StaticValues.backupEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                    + (StaticValues.hardlightEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine));
                regenOverheat = characterBody.attackSpeed * StaticValues.regenOverheatFraction * maxOverheat;

                costmultiplierOverheat = (float)Math.Pow(0.75f, characterBody.master.inventory.GetItemCount(RoR2Content.Items.AlienHead));
                costflatOverheat = (5 * characterBody.master.inventory.GetItemCount(RoR2Content.Items.LunarBadLuck));

                if (costmultiplierOverheat > 1f)
                {
                    costmultiplierOverheat = 1f;
                }
            }

            //Energy used
            if (ifOverheatMaxed)
            {
                hasOverheatedSecondary = true;
                hasOverheatedUtility = true;
                hasOverheatedSpecial = true;
                if (overheatDecayTimer > 5f / characterBody.attackSpeed)
                {
                    overheatDecayTimer = 0f;
                    ifOverheatRegenAllowed = true;
                    ifOverheatMaxed = false;
                }
                else
                {
                    currentOverheat = maxOverheat;
                    ifOverheatRegenAllowed = false;
                    overheatDecayTimer += Time.fixedDeltaTime;
                }
            }

            //Energy Currently have
            if (ifOverheatRegenAllowed)
            {
                currentOverheat -= regenOverheat * Time.fixedDeltaTime;
            }

            if (currentOverheat > maxOverheat)
            {
                currentOverheat = maxOverheat;
                ifOverheatMaxed = true;
            }

            if(currentOverheat < 0f)
            {
                currentOverheat = 0f;
            }

            if (energyNumber)
            {
                energyNumber.SetText($"{(int)currentOverheat} / {maxOverheat}");
            }

            if (energyMeter)
            {
                // 2f because meter is too small probably.
                // Logarithmically scale.
                float logVal = Mathf.Log10(((maxOverheat / StaticValues.baseEnergy) * 10f) + 1) * (currentOverheat / maxOverheat);
                energyMeter.localScale = new Vector3(2.0f * logVal, 0.05f, 1f);
                energyMeterGlowRect.localScale = new Vector3(2.3f * logVal, 0.1f, 1f);
            }

            //Chat.AddMessage($"{currentOverheat}/{maxOverheat}");
        }

        public void FixedUpdate()
        {
            if (characterBody.hasEffectiveAuthority)
            {
                CalculateEnergyStats();
            }

            if (characterBody.hasEffectiveAuthority && !SetActiveTrue)
            {
                CustomUIObject.SetActive(true);
                SetActiveTrue = true;
            }
        }

        public void Update()
        {
            if (state != GlowState.STOP)
            {
                glowStopwatch += Time.deltaTime;
                float lerpFraction;
                switch (state)
                {
                    // Lerp to target color
                    case GlowState.FLASH:

                        lerpFraction = glowStopwatch / flashConst;
                        currentColor = Color.Lerp(originalColor, targetColor, lerpFraction);

                        if (glowStopwatch > flashConst)
                        {
                            state = GlowState.DECAY;
                            glowStopwatch = 0f;
                        }
                        break;

                    //Lerp back to original color;
                    case GlowState.DECAY:
                        //Linearlly lerp.
                        lerpFraction = glowStopwatch / decayConst;
                        currentColor = Color.Lerp(targetColor, originalColor, lerpFraction);

                        if (glowStopwatch > decayConst)
                        {
                            state = GlowState.STOP;
                            glowStopwatch = 0f;
                        }
                        break;
                    case GlowState.STOP:
                        //State does nothing.
                        break;
                }
            }

            energyMeterGlowBackground.color = currentColor;
        }


        public void SpendEnergy(float Energy)
        {
            //float energyflatCost = Energy - costflatOverheat;
            //if (energyflatCost < 0f) energyflatCost = 0f;

            //float energyCost = rageEnergyCost * costmultiplierOverheat * energyflatCost;
            //if (energyCost < 0f) energyCost = 0f;

            currentOverheat += Energy;

        }

        public void TriggerGlow(float newDecayTimer, float newFlashTimer, Color newStartingColor)
        {
            decayConst = newDecayTimer;
            flashConst = newFlashTimer;
            originalColor = new Color(newStartingColor.r, newStartingColor.g, newStartingColor.b, 0f);
            targetColor = newStartingColor;
            glowStopwatch = 0f;
            state = GlowState.FLASH;
        }


        public void OnDestroy()
        {
            Destroy(CustomUIObject);
        }
    }
}


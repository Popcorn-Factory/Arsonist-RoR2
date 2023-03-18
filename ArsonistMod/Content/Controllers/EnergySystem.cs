using System;
using ArsonistMod.Modules;
using RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R2API.Networking;
using R2API.Networking.Interfaces;
using static ArsonistMod.Content.Controllers.EnergySystem;
using UnityEngine.UIElements;
using ArsonistMod.Modules.Networking;
using RoR2.CharacterAI;

namespace ArsonistMod.Content.Controllers
{
    public class EnergySystem : MonoBehaviour
    {
        public CharacterBody characterBody;
        public ArsonistController arsonistController;

        //UI energyMeter
        public GameObject CustomUIObject;
        public GameObject EnergyNumberContainer;
        public Canvas CustomUIObjectCanvas;
        public LineRenderer fullSegment;
        public LineRenderer levelSegment;
        public LineRenderer segment1;
        public LineRenderer segment2;
        public LineRenderer segment3;
        public Vector3[] segmentList;
        public Camera mainCamera;

        //Energy Bar Overheat State
        public enum OverheatState 
        {
            START,
            HOLD,
            END,
            DORMANT
        }
        public OverheatState overheatState;
        public float decayTimer = 0.6f;
        public float overheatTimer = 0f;
        public bool overheatTriggered;
        public Vector3[] originalRedLength;
        public float additionalRed = 0;
        public bool enabledUI;

        public bool isAcceleratedCooling;

        

        //OLD
        public RectTransform energyMeter;
        public RectTransform energyMeterGlowRect;
        //public Image energyMeterGlowBackground;
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

        //whether base or alt m1 used
        public bool baseHeatGauge;
        public string prefix = ArsonistPlugin.DEVELOPER_PREFIX;

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
        private int whiteSegment;
        private int blueSegment;
        private int redSegment;
        private float blueRatio;
        private float whiteRatio;
        public float currentBlueNumber;

        //BaseAI check
        public bool baseAIPresent;

        //Sound vars
        uint tickingSound;

        //Animator
        Animator anim;

        //vibration Vars
        TextVibration textVibration;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();

            //lets hook it up!
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;


            if (ArsonistPlugin.photoMode)
            {
                On.RoR2.CameraRigController.Update += CameraRigController_Update;
            }

            enabledUI = false;
            isAcceleratedCooling = false;
        }

        //reuse the segment making
        public void SegmentMake()
        {
            if (baseHeatGauge)
            {
                //calcing blue and red ratios for base
                blueRatio = 0;
                whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
            }
            else
            {
                blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment * (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                    + (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                    + StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));
                if (blueRatio > StaticValues.maxBlueWhiteSegment)
                {
                    blueRatio = StaticValues.maxBlueWhiteSegment;
                }
                whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
                if (whiteRatio < 0f)
                {
                    whiteRatio = 0f;
                }

            }

            //updating segments with the ratios
            whiteSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * whiteRatio);
            blueSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * blueRatio);
            redSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeMain.z);


            Vector3[] whiteArray = new Vector3[whiteSegment];
            Vector3[] blueArray = new Vector3[blueSegment];
            Vector3[] redArray = new Vector3[redSegment];

            for (int i = 0; i < Modules.StaticValues.noOfSegmentsOnOverheatGauge; i++)
            {
                if (i < whiteSegment)
                {
                    whiteArray[i] = segmentList[i];
                }
                if (i > whiteSegment && i < whiteSegment + blueSegment)
                {
                    blueArray[i - whiteSegment - 1] = segmentList[i];
                }
                if (i > whiteSegment + blueSegment && i < whiteSegment + blueSegment + redSegment)
                {
                    redArray[i - whiteSegment - blueSegment - 1] = segmentList[i];
                }
            }

            segment1.positionCount = whiteArray.Length - 1;
            segment1.SetPositions(whiteArray);
            segment2.positionCount = blueArray.Length - 1;
            segment2.SetPositions(blueArray);
            segment3.positionCount = redArray.Length - 1;
            segment3.SetPositions(redArray);
            originalRedLength = redArray;
        }

        private void CameraRigController_Update(On.RoR2.CameraRigController.orig_Update orig, RoR2.CameraRigController self) 
        {
            orig(self);
            //Perform a check to see if the hud is disabled and enable/disable our hud if necessary.
            if (self.hud.combatHealthBarViewer.enabled)
            {
                if (CustomUIObject && energyNumber && characterBody.hasEffectiveAuthority)
                {
                    CustomUIObject.SetActive(true);
                    energyNumber.gameObject.SetActive(true);
                }
            }
            else 
            {
                if (CustomUIObject && energyNumber && characterBody.hasEffectiveAuthority)
                {
                    CustomUIObject.SetActive(false);
                    energyNumber.gameObject.SetActive(false);
                }
            }
        }

        private void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self)
        {
            orig(self);

            if (self && self.master.inventory)
            {
                SegmentMake();
            }
        }

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            if (self && self.teamIndex == TeamIndex.Player && self.inventory)
            {
                GameObject bodyobject = self.GetBodyObject();
                if (bodyobject != null)
                {
                    SegmentMake();
                }
            }
        }

        public void Start()
        {
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
            overheatTriggered = false;
            additionalRed = 0;
            overheatState = OverheatState.DORMANT;
            mainCamera = Camera.main;

            arsonistController = base.gameObject.GetComponent<ArsonistController>();

            if (characterBody.skillLocator.primary.skillNameToken == prefix + "_ARSONIST_BODY_PRIMARY_FIRESPRAY_NAME")
            {

                baseHeatGauge = true;

                blueRatio = 0;
                whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
            }
            else
            {
                baseHeatGauge = false;
                
                blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment * (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                    + (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                    + StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));
                if (blueRatio > StaticValues.maxBlueWhiteSegment)
                {
                    blueRatio = StaticValues.maxBlueWhiteSegment;
                }
                whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
                if (whiteRatio < 0f)
                {
                    whiteRatio = 0f;
                }
            }

            currentBlueNumber = whiteRatio * maxOverheat;

            CharacterMaster master = characterBody.master;
            BaseAI baseAI = master.GetComponent<BaseAI>();

            baseAIPresent = baseAI;

            SetupCustomUI();

            // Start timer on 1f to turn off the timer.
            state = GlowState.STOP;
            decayConst = 1f;
            flashConst = 1f;
            glowStopwatch = 1f;
            originalColor = new Color(1f, 1f, 1f, 0f);
            targetColor = new Color(1f, 1f, 1f, 1f);
            currentColor = originalColor;

            anim = characterBody.hurtBoxGroup.gameObject.GetComponent<Animator>();
        }

        private void SetupCustomUI() 
        {
            //UI objects 
            CustomUIObject = UnityEngine.Object.Instantiate(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("arsonistOverheatGauge"));
            EnergyNumberContainer = UnityEngine.Object.Instantiate(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("arsonistCustomUI"));
            //Get the line renderers for all the objects in the overheat gauge
            //Since we can't use Line renderers for the screen space overlay, we have to assign camera.main
            CustomUIObjectCanvas = CustomUIObject.GetComponent<Canvas>();
            CustomUIObjectCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            CustomUIObjectCanvas.worldCamera = mainCamera;
            segmentList = new Vector3[Modules.StaticValues.noOfSegmentsOnOverheatGauge];
            levelSegment = CustomUIObject.transform.GetChild(0).GetComponent<LineRenderer>();
            segment1 = CustomUIObject.transform.GetChild(1).GetComponent<LineRenderer>();
            segment2 = CustomUIObject.transform.GetChild(2).GetComponent<LineRenderer>();
            segment3 = CustomUIObject.transform.GetChild(3).GetComponent<LineRenderer>();
            //Calculate the segments and slap them into an array.
            CalculateSemiCircle(6f, 0.85f);

            //Determine the partitions from a set of static values.
            if (baseHeatGauge)
            {
                whiteSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeMain.x);
                blueSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeMain.y);
                redSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeMain.z);

            }
            else
            {
                whiteSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeAlt.x);
                blueSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeAlt.y);
                redSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeAlt.z);
            }


            SegmentMake();


            //setup the UI element for the min/max
            energyNumber = this.CreateLabel(EnergyNumberContainer.transform, "energyNumber", $"{(int)currentOverheat} / {maxOverheat}", new Vector2(0, -60f), 24f);
            textVibration = energyNumber.gameObject.AddComponent<TextVibration>();

            CustomUIObject.SetActive(false);
            energyNumber.gameObject.SetActive(false);
        }

        //Calculate segments
        //we use percentage to figure out how far we should go for radius. do not go beyond 1 or below 0!
        private void CalculateSemiCircle(float radius, float percentage) 
        {
            float range = percentage * radius * 2f;
            float incrementX = range / (float)Modules.StaticValues.noOfSegmentsOnOverheatGauge;

            float x = percentage * radius * -1f;

            //assuming a percentage, we want the center to be where x = 0
            for (int i = 0; i < segmentList.Length; i++) 
            {
                //y = sqrt(r^2 - x^2)
                float y = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2));

                //write positions into array.
                segmentList[i] = new Vector3(x, y, 0f);
                x += incrementX;
            }
        }

        //Creates the label.
        private HGTextMeshProUGUI CreateLabel(Transform parent, string name, string text, Vector2 position, float textScale)
        {
            GameObject textObj;
            if (!parent)
            {
                EnergyNumberContainer = new GameObject(name);
                textObj = EnergyNumberContainer;
                Canvas canvas = textObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            else
            {
                textObj = new GameObject(name);
                textObj.transform.parent = parent;

            }

            textObj.AddComponent<CanvasRenderer>();
            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            HGTextMeshProUGUI hgtextMeshProUGUI = textObj.AddComponent<HGTextMeshProUGUI>();
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
                if (baseHeatGauge)
                {
                    if (characterBody.master) 
                    {
                        //max heat increases
                        maxOverheat = StaticValues.baseEnergy + ((characterBody.level - 1) * StaticValues.levelEnergy)
                            + (StaticValues.backupEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                            + (StaticValues.hardlightEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine)
                            + StaticValues.lysateEnergyGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));

                    }
                    //regen increases based off current overheat value
                    //halfheat ratio means that at 50% or lower = 1, 100% heat = 2, 
                    float halfheat = currentOverheat / (maxOverheat / 2f);
                    if(halfheat < 1f)
                    {
                        halfheat = 1f;
                    }
                    regenOverheat = characterBody.attackSpeed * StaticValues.regenOverheatFraction * maxOverheat * halfheat;
                    //calcing blue and red ratios for base
                    blueRatio = 0;
                    whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
                    
                }
                else
                {
                    //max overheat doesn't increase
                    maxOverheat = StaticValues.baseEnergy;
                    //regen remains static
                    regenOverheat = characterBody.attackSpeed * StaticValues.regenOverheatFraction * maxOverheat;
                    //calcing blue and red ratios for alt
                    if (characterBody.master) 
                    {
                        blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment * (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                        + (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                        + StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));
                    }

                    if (blueRatio > StaticValues.maxBlueWhiteSegment)
                    {
                        blueRatio = StaticValues.maxBlueWhiteSegment;
                    }
                    whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
                    if (whiteRatio < 0f)
                    {
                        whiteRatio = 0f;
                    }

                }

                if (characterBody.master) 
                {
                    costmultiplierOverheat = (float)Math.Pow(0.75f, characterBody.master.inventory.GetItemCount(RoR2Content.Items.AlienHead));
                    costflatOverheat = (5 * characterBody.master.inventory.GetItemCount(RoR2Content.Items.LunarBadLuck));
                }

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

                float coolingRate = 1.0f;
                if (isAcceleratedCooling) 
                {
                    coolingRate = 30.0f;
                }

                //set current heat to 0 once over!
                if (overheatDecayTimer > (Modules.Config.timeBeforeHeatGaugeDecays.Value / characterBody.attackSpeed))
                {
                    currentOverheat = 0f;
                    overheatDecayTimer = 0f;
                    ifOverheatRegenAllowed = true;
                    ifOverheatMaxed = false;
                    overheatTriggered = false;
                    isAcceleratedCooling = false;
                    AkSoundEngine.StopPlayingID(tickingSound);

                    //Finish sound.
                    new PlaySoundNetworkRequest(characterBody.netId, 3787943995).Send(NetworkDestination.Clients);
                }
                else
                {
                    currentOverheat = maxOverheat;
                    ifOverheatRegenAllowed = false;
                    overheatDecayTimer += Time.fixedDeltaTime * coolingRate;
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
                //Overheat Start sound
                new PlaySoundNetworkRequest(characterBody.netId, 3152162514).Send(NetworkDestination.Clients);
                //Overheat duration sound, not networked.
                tickingSound = AkSoundEngine.PostEvent(3408252638, characterBody.gameObject);
            }

            if(currentOverheat < 0f)
            {
                currentOverheat = 0f;
            }

            if (energyNumber)
            {
                if (isAcceleratedCooling)
                {
                    energyNumber.SetText($"COOLING...!");
                }
                else 
                {
                    energyNumber.SetText(ifOverheatMaxed ? $"OVERHEAT!" : $"{(int)currentOverheat} / {maxOverheat}");
                }
            }

            //Chat.AddMessage($"{currentOverheat}/{maxOverheat}");
        }

        public void FixedUpdate()
        {
            if (characterBody.hasEffectiveAuthority)
            {
                CalculateEnergyStats();

                if (!enabledUI && !baseAIPresent) 
                {
                    enabledUI = true;
                    CustomUIObject.SetActive(true);
                    energyNumber.gameObject.SetActive(true);
                }
            }
        }

        public void SetOverheatMaterialParameters() 
        {
            if (ifOverheatMaxed)
            {
                //Overheat check
                //overheatDecayTimer > (Modules.Config.timeBeforeHeatGaugeDecays.Value / characterBody.attackSpeed)
                float o = overheatDecayTimer;
                float h = (Modules.Config.timeBeforeHeatGaugeDecays.Value / characterBody.attackSpeed);
                float alphaCalc = (((h - o) / 4.0f) / h);

                Modules.Assets.arsonistOverheatingMaterial.SetColor("_Color", new Vector4(1f, 0.262f, 0f, alphaCalc));
                Modules.Assets.arsonistOverheatingMaterial.SetFloat("_VertexTimeMultiplier", (((h - o) / 4.0f) / h) * 25f);
            }
            else 
            {
                float alphaCalc = Mathf.Clamp(Mathf.Pow(currentOverheat / maxOverheat, 8), 0, 1) / 4f;
                //Default material

                if (baseHeatGauge)
                {
                    Modules.Assets.arsonistOverheatingMaterial.SetColor("_Color", new Vector4(1f, 0.4f, 0f, alphaCalc));
                }
                else
                {
                    Modules.Assets.arsonistOverheatingMaterial.SetColor("_Color", new Vector4(0f, 0.537f, 1f, alphaCalc));
                }
                Modules.Assets.arsonistOverheatingMaterial.SetFloat("_VertexTimeMultiplier", Mathf.Clamp(Mathf.Pow(currentOverheat / maxOverheat, 8), 0, 1) * 25f);
            }
        }

        public void CheckAndSetOverheatingCanister() 
        {
            if (ifOverheatMaxed)
            {
                anim.SetBool("Overheated", true);
                arsonistController.steamDownParticle.Play();
            }
            else 
            {
                anim.SetBool("Overheated", false);
                arsonistController.steamDownParticle.Stop();
            }
        }

        public void HandleTextVibration() 
        {
            if (ifOverheatMaxed)
            {
                textVibration.vibrating = true;
                textVibration.intensity = Mathf.Clamp( (1f / (0.03f * textVibration.stopwatch)) , 0, 15f);
                textVibration.speed = (1f / (0.03f * textVibration.stopwatch));
            }
            else 
            {
                textVibration.vibrating = false;
            }
        }

        public void Update()
        {
            //Update material for overheating tex
            if (Modules.Assets.arsonistOverheatingMaterial) 
            {
                SetOverheatMaterialParameters();
            }

            if (anim) 
            {
                CheckAndSetOverheatingCanister();
            }

            if (textVibration && Modules.Config.overheatTextShouldVibrate.Value) 
            {
                HandleTextVibration();
            }

            //checking which m1 is equipped for different heat passive
            if (characterBody.skillLocator.primary.skillNameToken == prefix + "_ARSONIST_BODY_PRIMARY_FIRESPRAY_NAME")
            {
                if (!baseHeatGauge)
                {
                    baseHeatGauge = true;
                }
            }
            else
            {
                if (baseHeatGauge)
                {
                    baseHeatGauge = false;
                }
            }

            //ui color change

            //blue gauge checks
            currentBlueNumber = whiteRatio * maxOverheat;
            if (energyNumber)
            {
                if (currentOverheat == maxOverheat)
                {
                    if (isAcceleratedCooling) 
                    {
                        energyNumber.color = Color.cyan;
                    }
                    else
                    {
                        energyNumber.color = Color.red;
                    }
                    if (characterBody.HasBuff(Buffs.blueBuff.buffIndex))
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 0);
                    }
                }
                else if (currentOverheat < currentBlueNumber)
                {
                    energyNumber.color = Color.white;
                    if (characterBody.HasBuff(Buffs.blueBuff.buffIndex))
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 0);
                    }
                }
                else if (currentOverheat >= currentBlueNumber && currentOverheat < maxOverheat && !baseHeatGauge)
                {
                    energyNumber.color = Color.cyan;
                    if (!characterBody.HasBuff(Buffs.blueBuff.buffIndex))
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 1);
                    }
                }                   
                
            }

            

            if (!mainCamera) 
            {
                mainCamera = Camera.main;
                CustomUIObjectCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                CustomUIObjectCanvas.worldCamera = mainCamera;
            }


            if (ifOverheatMaxed) 
            {
                //Do fancy stuff regarding the red section
                if (!overheatTriggered) 
                {
                    overheatTriggered = true;
                    overheatState = OverheatState.START;
                    overheatTimer = 0f;
                    additionalRed = 0;
                }
            }

            //updating segments with the ratios
            whiteSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * whiteRatio);
            blueSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * blueRatio);
            redSegment = (int)(Modules.StaticValues.noOfSegmentsOnOverheatGauge * Modules.StaticValues.SegmentedValuesOnGaugeMain.z);
            int maxSegment = whiteSegment + blueSegment;

            int calculatedLastSegment = (int)((float)maxSegment * (float)(currentOverheat / maxOverheat));
            Vector3[] proposedPositions = new Vector3[calculatedLastSegment];
            Array.Copy(segmentList, proposedPositions, calculatedLastSegment);

            levelSegment.positionCount = proposedPositions.Length;
            levelSegment.SetPositions(proposedPositions);

            switch (overheatState) 
            {
                case OverheatState.START:
                    //Red is increasing to the start of the bar.
                    overheatTimer += Time.deltaTime;

                    float coolingRate = 1.0f;
                    if (isAcceleratedCooling)
                    {
                        coolingRate = 2.0f;
                    }

                    additionalRed = maxSegment * ( 
                        overheatDecayTimer / (
                            (Modules.Config.timeBeforeHeatGaugeDecays.Value / characterBody.attackSpeed) ) 
                        ); //to calculate, it's a fraction of the amount of time left.

                    if(!ifOverheatMaxed || overheatTimer >= (Modules.Config.timeBeforeHeatGaugeDecays.Value / characterBody.attackSpeed) ) 
                    {
                        overheatState = OverheatState.END;
                        overheatTriggered = false;
                        overheatTimer = 0f;
                    }

                    //add to the red segment, and determine how many segments should be allocated to red.
                    int firstIndex = Modules.StaticValues.noOfSegmentsOnOverheatGauge - ((int)additionalRed + redSegment);
                    if ( firstIndex >= 0 )
                    {
                        //only copy from the start index to the end of the array.
                        Vector3[] redProposedPositions = new Vector3[(int)(additionalRed + redSegment)];
                        Array.Copy(segmentList, firstIndex, redProposedPositions, 0, (int)(additionalRed + redSegment));

                        segment3.positionCount = (int)(additionalRed + redSegment);
                        segment3.SetPositions(redProposedPositions);                        
                    }
                    else 
                    {
                        //Set to max.
                        segment3.positionCount = Modules.StaticValues.noOfSegmentsOnOverheatGauge;
                        segment3.SetPositions(segmentList);
                    }
                    break;
                case OverheatState.END:
                    //Red is decaying back to the beginning
                    overheatTimer += Time.deltaTime;

                    float decayRate = (maxSegment / decayTimer);
                    additionalRed -= (decayRate * Time.deltaTime);

                    int index = Modules.StaticValues.noOfSegmentsOnOverheatGauge - ((int)additionalRed + redSegment);
                    if (index >= redSegment)
                    {
                        //only copy from the start index to the end of the array.
                        Vector3[] redProposedPositions = new Vector3[(int)(additionalRed + redSegment)];
                        Array.Copy(segmentList, index, redProposedPositions, 0, (int)(additionalRed + redSegment));

                        segment3.positionCount = (int)(additionalRed + redSegment);
                        segment3.SetPositions(redProposedPositions);
                    }
                    if (overheatTimer >= decayTimer || additionalRed <= 0) 
                    {
                        overheatState = OverheatState.DORMANT;
                        additionalRed = 0;
                    }
                    break;
                case OverheatState.DORMANT:
                    //Red stays at the end of the bar.
                    segment3.positionCount = originalRedLength.Length - 1;
                    segment3.SetPositions(originalRedLength);
                    break;
            }

            //if (state != GlowState.STOP)
            //{
            //    glowStopwatch += Time.deltaTime;
            //    float lerpFraction;
            //    switch (state)
            //    {
            //        // Lerp to target color
            //        case GlowState.FLASH:

            //            lerpFraction = glowStopwatch / flashConst;
            //            currentColor = Color.Lerp(originalColor, targetColor, lerpFraction);

            //            if (glowStopwatch > flashConst)
            //            {
            //                state = GlowState.DECAY;
            //                glowStopwatch = 0f;
            //            }
            //            break;

            //        //Lerp back to original color;
            //        case GlowState.DECAY:
            //            //Linearlly lerp.
            //            lerpFraction = glowStopwatch / decayConst;
            //            currentColor = Color.Lerp(targetColor, originalColor, lerpFraction);

            //            if (glowStopwatch > decayConst)
            //            {
            //                state = GlowState.STOP;
            //                glowStopwatch = 0f;
            //            }
            //            break;
            //        case GlowState.STOP:
            //            //State does nothing.
            //            break;
            //    }
            //}

            //energyMeterGlowBackground.color = currentColor;
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
            Destroy(EnergyNumberContainer);
            On.RoR2.CharacterMaster.OnInventoryChanged -= CharacterMaster_OnInventoryChanged;
            On.RoR2.CharacterBody.OnLevelUp -= CharacterBody_OnLevelUp;

            if (ArsonistPlugin.photoMode)
            {
                On.RoR2.CameraRigController.Update -= CameraRigController_Update;
            }
        }
    }
}


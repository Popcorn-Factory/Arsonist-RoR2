using System;
using ArsonistMod.Modules;
using RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R2API.Networking;
using R2API.Networking.Interfaces;
using ArsonistMod.Modules.Networking;
using RoR2.CharacterAI;

namespace ArsonistMod.Content.Controllers
{
    public class EnergySystem : MonoBehaviour
    {
        public CharacterBody characterBody;
        public ArsonistPassive passive;
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

        //ScepterFireballGauge
        public GameObject FireballGauge;

        

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
        public bool ifOverheatMaxed;
        public bool hasOverheatedSecondary;
        public bool hasOverheatedUtility;
        public bool hasOverheatedSpecial;
        public float lowerBound;

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

        //Masochism monitoring
        public MasochismController masoCon;
        public MasochismSurgeController masoSurgeController;

        //Overheated?
        public bool hasOverheatedThisStage = false;

        //Prevent Cooling for x seconds 
        public float regenPreventionDuration = 0f;
        public float regenPreventionStopwatch = 0f;
        public bool ifOverheatRegenAllowed;

        public float updateSegment = 0.5f;
        public float segmentTimer = 0f;

        //maso specific
        public bool disableHeatGainMaso = false;
        
        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();

            //lets hook it up!
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            On.RoR2.CameraRigController.LateUpdate += CameraRigController_LateUpdate;

            enabledUI = false;
            isAcceleratedCooling = false;
            hasOverheatedThisStage = false;
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
                //blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment * (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                //    + (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                //    + StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid)
                //    + ((characterBody.level - 1) * StaticValues.levelBlueEnergy));

                blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment
                * (1 + (StaticValues.backupBlueGain * ((float)characterBody.skillLocator.secondary.maxStock - 1f))
                + (StaticValues.hardlightBlueGain * ((float)characterBody.skillLocator.utility.maxStock - 1f))
                + (StaticValues.lysateBlueGain * (characterBody.skillLocator.special.maxStock >= 0 ? 0f : (float)characterBody.skillLocator.special.maxStock - 1f))
                + ((characterBody.level - 1) * StaticValues.levelBlueEnergy));

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

            
            segment1.positionCount = whiteArray.Length >= 1 ? whiteArray.Length - 1 : whiteArray.Length;
            segment1.SetPositions(whiteArray);
            segment2.positionCount = blueArray.Length >= 1 ? blueArray.Length - 1 : blueArray.Length;
            segment2.SetPositions(blueArray);
            segment3.positionCount = redArray.Length >= 1 ? redArray.Length - 1 : redArray.Length;
            segment3.SetPositions(redArray);
            originalRedLength = redArray;
        }

        private void CameraRigController_LateUpdate(On.RoR2.CameraRigController.orig_LateUpdate orig, RoR2.CameraRigController self) 
        {
            orig(self);
            //Perform a check to see if the hud is disabled and enable/disable our hud if necessary.
            if (self.hud.mainUIPanel.activeInHierarchy)
            {               
                if (CustomUIObject && energyNumber && characterBody.hasEffectiveAuthority && !baseAIPresent)
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
                try
                {
                    SegmentMake();
                }
                catch (IndexOutOfRangeException e) 
                {
                    Debug.Log("OH FUCK");
                    Debug.Log(e);
                }
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
                    try
                    {
                        SegmentMake();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.Log("OH FUCK");
                        Debug.Log(e);
                    }
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
            lowerBound = 0f;
            overheatState = OverheatState.DORMANT;
            mainCamera = Camera.main;

            passive = base.gameObject.GetComponent<ArsonistPassive>();
            arsonistController = base.gameObject.GetComponent<ArsonistController>();

            if (!passive.isBlueGauge())
            {

                baseHeatGauge = true;

                blueRatio = 0;
                whiteRatio = StaticValues.maxBlueWhiteSegment - blueRatio;
            }
            else
            {
                baseHeatGauge = false;

                //blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment * (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                //    + (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                //    + StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));

                blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment
                * (1 + (StaticValues.backupBlueGain * ((float)characterBody.skillLocator.secondary.maxStock - 1f))
                + (StaticValues.hardlightBlueGain * ((float)characterBody.skillLocator.utility.maxStock - 1f))
                + (StaticValues.lysateBlueGain * (characterBody.skillLocator.special.maxStock >= 0 ? 0f : (float)characterBody.skillLocator.special.maxStock - 1f))
                + ((characterBody.level - 1) * StaticValues.levelBlueEnergy));

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

            //For some reason on goboo's first spawn the master is just not there. However subsequent spawns work.
            // Disable the UI in this event.
            //Besides, there should never be a UI element related to a non-existant master on screen if the attached master/charbody does not exist.
            if (!master) 
            {
                baseAIPresent = true; // Disable UI Just in case.
            }

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

            //Check if we have masochism selected and run the right logic.
            if (characterBody.skillLocator.special.skillNameToken == "POPCORN_ARSONIST_BODY_SPECIAL_MASOCHISM_NAME") 
            {
                //Add the component.
                masoCon = gameObject.AddComponent<MasochismController>();
                //This should be destroyed with the body I guess.
            }

            if (characterBody.skillLocator.special.skillNameToken == "POPCORN_ARSONIST_BODY_SPECIAL_MASOCHISM_SURGE_NAME") 
            {
                masoSurgeController = gameObject.AddComponent<MasochismSurgeController>();
            }
        }

        private void SetupCustomUI() 
        {
            //UI objects 
            CustomUIObject = UnityEngine.Object.Instantiate(Modules.AssetsArsonist.mainAssetBundle.LoadAsset<GameObject>("arsonistOverheatGauge"));
            EnergyNumberContainer = UnityEngine.Object.Instantiate(Modules.AssetsArsonist.mainAssetBundle.LoadAsset<GameObject>("arsonistCustomUI"));
            //Get the line renderers for all the objects in the overheat gauge
            //Since we can't use Line renderers for the screen space overlay, we have to assign camera.main
            CustomUIObjectCanvas = CustomUIObject.GetComponent<Canvas>();
            CustomUIObjectCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            CustomUIObjectCanvas.worldCamera = mainCamera;
            CanvasScaler scaler = CustomUIObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
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
            energyNumber = this.CreateLabel(EnergyNumberContainer.transform, "energyNumber", $"{(int)currentOverheat} / {maxOverheat}", new Vector2(0f, 110f), 24f);
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
                GameObject energyNumberContainer = new GameObject(name);
                textObj = energyNumberContainer;
                Canvas canvas = textObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            else
            {
                textObj = new GameObject(name);
                textObj.transform.parent = parent;

            }
            CanvasScaler scaler = parent.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
            scaler.referenceResolution = new Vector2(2560f, 1440f);

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
                        //maxOverheat = StaticValues.baseEnergy + ((characterBody.level - 1) * StaticValues.levelEnergy)
                        //    + (StaticValues.backupEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                        //    + (StaticValues.hardlightEnergyGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine)
                        //    + StaticValues.lysateEnergyGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid));

                        maxOverheat = StaticValues.baseEnergy + ((characterBody.level - 1) * StaticValues.levelEnergy)
                            + (StaticValues.backupEnergyGain * ((float)characterBody.skillLocator.secondary.maxStock - 1f))
                            + (StaticValues.hardlightEnergyGain * ((float)characterBody.skillLocator.utility.maxStock - 1f))
                            + (StaticValues.lysateEnergyGain * (characterBody.skillLocator.special.maxStock >= 0 ? 0f : (float)characterBody.skillLocator.special.maxStock - 1f));

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
                        //blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment 
                        //* (1 + (StaticValues.backupBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine))
                        //+ (StaticValues.hardlightBlueGain * characterBody.master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine))
                        //+ StaticValues.lysateBlueGain * characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid)
                        //+ ((characterBody.level - 1) * StaticValues.levelBlueEnergy));


                        blueRatio = StaticValues.SegmentedValuesOnGaugeAlt.y / StaticValues.maxBlueWhiteSegment
                        * (1 + (StaticValues.backupBlueGain * ((float)characterBody.skillLocator.secondary.maxStock - 1f))
                        + (StaticValues.hardlightBlueGain * ((float)characterBody.skillLocator.utility.maxStock - 1f))
                        + (StaticValues.lysateBlueGain * (characterBody.skillLocator.special.maxStock >= 0 ? 0f : (float)characterBody.skillLocator.special.maxStock - 1f))
                        + ((characterBody.level - 1) * StaticValues.levelBlueEnergy));
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

                if (characterBody.skillLocator.secondary.stock != 0) 
                {
                    hasOverheatedSecondary = true;
                }
                if (characterBody.skillLocator.utility.stock != 0) 
                {
                    hasOverheatedUtility = true;
                }
                hasOverheatedSpecial = true;

                float coolingRate = 1.0f;
                if (isAcceleratedCooling) 
                {
                    coolingRate = 30.0f;
                }

                //set current heat to 0 once over
                float attackspeedcheck = characterBody.attackSpeed > 1f ? characterBody.attackSpeed : 1f;
                if (overheatDecayTimer > (Modules.Config.timeBeforeHeatGaugeDecays.Value / attackspeedcheck))
                {
                    currentOverheat = 0f;
                    overheatDecayTimer = 0f;
                    ifOverheatRegenAllowed = true;
                    ifOverheatMaxed = false;
                    overheatTriggered = false;
                    isAcceleratedCooling = false;

                    //Unset overheated skills when cooldown is completed.
                    //hasOverheatedSecondary = false;
                    //hasOverheatedUtility = false;
                    //hasOverheatedSpecial = false;

                    AkSoundEngine.StopPlayingID(tickingSound);
                    characterBody.ApplyBuff(Modules.Buffs.overheatDebuff.buffIndex, 0, -1);

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
                //Regen needs to change so that more heat = more cooling rate for base gauge
                if (passive.isBlueGauge())
                {
                    LowerHeat(regenOverheat * Time.fixedDeltaTime, true);
                }
                else
                {
                    //Cooling rate is determined by level of heat ratio and plugged into a parabola which ranges from lower bound to 
                    //lowerbound + 1 at maximum if uncapped.
                    float ratio = (float)currentOverheat / (float)maxOverheat;
                    float coolingRate = Mathf.Pow(ratio, 4f) + Modules.Config.baseGaugeLowerBoundRecharge.Value;
                    if (coolingRate >= Modules.Config.baseGaugeUpperBoundRecharge.Value)
                    {
                        coolingRate = Modules.Config.baseGaugeUpperBoundRecharge.Value;
                    }
                    if (coolingRate <= Modules.Config.baseGaugeLowerBoundRecharge.Value)
                    {
                        coolingRate = Modules.Config.baseGaugeLowerBoundRecharge.Value;
                    }

                    LowerHeat(regenOverheat * coolingRate * Time.fixedDeltaTime, true);
                }
            }
            else 
            {
                regenPreventionStopwatch += Time.fixedDeltaTime;
                if (regenPreventionStopwatch > regenPreventionDuration) 
                {
                    ifOverheatRegenAllowed = true;
                }
            }

            if (currentOverheat > maxOverheat)
            {
                currentOverheat = maxOverheat;
                ifOverheatMaxed = true;
                hasOverheatedThisStage = true;
                characterBody.ApplyBuff(Modules.Buffs.overheatDebuff.buffIndex, 1, -1);
                //Overheat Start sound
                new PlaySoundNetworkRequest(characterBody.netId, 3152162514).Send(NetworkDestination.Clients);
                //Overheat duration sound, not networked.
                tickingSound = AkSoundEngine.PostEvent(3408252638, characterBody.gameObject);
            }

            if (currentOverheat < lowerBound)
            {
                currentOverheat = lowerBound;
            }

            if (energyNumber)
            {
                if (isAcceleratedCooling)
                {
                    
                    energyNumber.SetText(Language.GetString($"{ArsonistPlugin.DEVELOPER_PREFIX}_ARSONIST_BODY_COOLING_TEXT"));
                }
                else 
                {
                    if (characterBody.HasBuff(Modules.Buffs.masochismDeactivatedDebuff))
                    {
                        energyNumber.SetText(ifOverheatMaxed ? Language.GetString($"{ArsonistPlugin.DEVELOPER_PREFIX}_ARSONIST_BODY_OVERHEAT_EX_TEXT") : $"{(int)currentOverheat} / {maxOverheat}");
                    }
                    else 
                    {
                        energyNumber.SetText(ifOverheatMaxed ? Language.GetString($"{ArsonistPlugin.DEVELOPER_PREFIX}_ARSONIST_BODY_OVERHEAT_TEXT") : $"{(int)currentOverheat} / {maxOverheat}");
                    }
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

                if (arsonistController) 
                {
                    arsonistController.overheatingMaterial.SetColor("_Color", new Vector4(1f, 0.262f, 0f, alphaCalc));
                    arsonistController.overheatingMaterial.SetFloat("_VertexTimeMultiplier", (((h - o) / 4.0f) / h) * 25f);
                }
            }
            else 
            {
                float alphaCalc = Mathf.Clamp(Mathf.Pow(currentOverheat / maxOverheat, 8), 0, 1) / 4f;
                //Default material

                if (arsonistController) 
                {
                    if (baseHeatGauge)
                    {
                        arsonistController.overheatingMaterial.SetColor("_Color", new Vector4(1f, 0.4f, 0f, alphaCalc));
                    }
                    else
                    {
                        arsonistController.overheatingMaterial.SetColor("_Color", new Vector4(0f, 0.537f, 1f, alphaCalc));
                    }
                    arsonistController.overheatingMaterial.SetFloat("_VertexTimeMultiplier", Mathf.Clamp(Mathf.Pow(currentOverheat / maxOverheat, 8), 0, 1) * 25f);
                }
            }
        }

        public void CheckAndSetOverheatingCanister() 
        {
            if (ifOverheatMaxed)
            {
                anim.SetBool("Overheated", true);
                if (arsonistController)
                {
                    arsonistController.steamDownParticle.Play();
                }
            }
            else
            {
                anim.SetBool("Overheated", false);
                if (arsonistController) 
                {
                    arsonistController.steamDownParticle.Stop();
                }
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
            if (Modules.AssetsArsonist.arsonistOverheatingMaterial && characterBody.hasEffectiveAuthority) 
            {
                SetOverheatMaterialParameters();
            }

            if (anim && characterBody.hasEffectiveAuthority) 
            {
                CheckAndSetOverheatingCanister();
            }

            if (textVibration && Modules.Config.overheatTextShouldVibrate.Value) 
            {
                HandleTextVibration();
            }

            if (characterBody.hasEffectiveAuthority && !baseAIPresent) 
            {
                segmentTimer += Time.deltaTime;
                if (segmentTimer >= updateSegment) 
                {
                    SegmentMake();
                }
            }

            //checking which m1 is equipped for different heat passive
            if (!passive.isBlueGauge())
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
                if (currentOverheat >= maxOverheat)
                {
                    if (isAcceleratedCooling) 
                    {
                        energyNumber.color = Color.cyan;
                    }
                    else
                    {
                        energyNumber.color = Color.red;
                    }
                    if (characterBody.HasBuff(Buffs.blueBuff.buffIndex) && characterBody.hasEffectiveAuthority)
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 0);
                        characterBody.ApplyBuff(Buffs.lowerBuff.buffIndex, 1);
                    }
                }
                else if (currentOverheat < currentBlueNumber)
                {
                    energyNumber.color = Color.white;
                    if (characterBody.HasBuff(Buffs.blueBuff.buffIndex) && characterBody.hasEffectiveAuthority)
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 0);
                        characterBody.ApplyBuff(Buffs.lowerBuff.buffIndex, 1);
                    }
                }
                else if (currentOverheat >= currentBlueNumber && currentOverheat < maxOverheat && !baseHeatGauge)
                {
                    energyNumber.color = Color.cyan;
                    if (!characterBody.HasBuff(Buffs.blueBuff.buffIndex) && characterBody.hasEffectiveAuthority)
                    {
                        characterBody.ApplyBuff(Buffs.blueBuff.buffIndex, 1);
                        characterBody.ApplyBuff(Buffs.lowerBuff.buffIndex, 0);
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
            if (calculatedLastSegment >= maxSegment) 
            {
                calculatedLastSegment = maxSegment;
            }
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

                    float attackspeedcheck = characterBody.attackSpeed > 1f ? characterBody.attackSpeed : 1f;

                    additionalRed = maxSegment * ( 
                        overheatDecayTimer / (
                            (Modules.Config.timeBeforeHeatGaugeDecays.Value / attackspeedcheck) ) 
                        ); //to calculate, it's a fraction of the amount of time left.

                    if (!ifOverheatMaxed || overheatTimer >= (Modules.Config.timeBeforeHeatGaugeDecays.Value / attackspeedcheck) ) 
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

        }

        public void AddHeat(float Energy, bool isNatural) 
        {
            currentOverheat += Energy;

            float flatOverheat = costflatOverheat;           

            //Add to masochism monitoring
            if (masoCon) 
            {
                masoCon.heatChanged += Energy + flatOverheat;
            }

            if (masoSurgeController && !disableHeatGainMaso) 
            {
                masoSurgeController.heatChanged += Energy + flatOverheat;
            }
        }

        public void AddHeat(float Energy) 
        {
            AddHeat(Energy, false);
        }

        public void LowerHeat(float Energy) 
        {
            LowerHeat(Energy, false);
        }

        public void LowerHeat(float Energy, bool isNatural)
        {
            float realHeatGained = Energy;
            currentOverheat -= Energy;
            if (currentOverheat < lowerBound) 
            {
                realHeatGained = Energy - Mathf.Abs(lowerBound - currentOverheat);
                currentOverheat = lowerBound;
            }

            float flatOverheat = costflatOverheat;
            if (isNatural) 
            {
                flatOverheat = costflatOverheat * Time.fixedDeltaTime;
            }
            //Add to masochism monitoring
            if (masoCon)
            {
                masoCon.heatChanged += realHeatGained + flatOverheat;
            }

            if (masoSurgeController && !disableHeatGainMaso)
            {
                masoSurgeController.heatChanged += realHeatGained + flatOverheat;
            }
        }

        public void SetCurrentHeatToLowerBound() 
        {
            // Clear out heat but do not add to heat changed.
            currentOverheat = lowerBound;
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
            On.RoR2.CameraRigController.LateUpdate -= CameraRigController_LateUpdate;

            new PlaySoundNetworkRequest(characterBody.netId, (uint)2176930590).Send(NetworkDestination.Clients);
            AkSoundEngine.StopPlayingID(tickingSound);
        }
    }
}


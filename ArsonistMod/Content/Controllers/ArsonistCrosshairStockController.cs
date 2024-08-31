using RoR2.UI;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UI;

namespace ArsonistMod.Content.Controllers
{
    internal class ArsonistCrosshairStockController : MonoBehaviour
    {
        public List<Image> stockObjects;

        public static List<Vector3> stockPositionsFlamethrower = new List<Vector3>
        {
            new Vector3(-45, -30, 0),
            new Vector3(-35, -30, 0),
            new Vector3(-25, -30, 0),
            new Vector3(-15, -30, 0),
            new Vector3(-5, -30, 0),
            new Vector3(5, -30, 0),
            new Vector3(15, -30, 0),
            new Vector3(25, -30, 0),
            new Vector3(35, -30, 0),
            new Vector3(45, -30, 0),
        };
        public static List<Vector3> stockPositionsFireball = new List<Vector3>
        {
            new Vector3(-20, -13, 0),
            new Vector3(-17, -23, 0),
            new Vector3(-14, -33, 0),
            new Vector3(-11, -43, 0),
            new Vector3(-8, -53, 0),
            new Vector3(20, -13, 0),
            new Vector3(17, -23, 0),
            new Vector3(14, -33, 0),
            new Vector3(11, -43, 0),
            new Vector3(8, -53, 0),
        };

        public static Vector3 stockSize = new Vector3(0.08f, 0.08f, 0.08f);
        public static Color opacityColor = new Color(0f, 0f, 0f, 0.5f);
        private static int maxUIStockLimit = 10;
        public static int maxStock = Modules.Config.masochismMaximumStack.Value;
        public static int requiredStock = Modules.Config.masochismMinimumRequiredToActivate.Value;

        public GameObject crosshairStockContainer;
        public ArsonistController arsonistController;
        public CharacterBody charBody;
        public MasochismController masochismController;

        public bool activateable;
        public bool isFlamethrower;

        public void Awake() 
        {
            activateable = false;
        }
        public void Start() 
        {
            //Get HUDelement from object, point to charobject and get ArsonistController
            charBody = gameObject.GetComponent<HudElement>().targetCharacterBody;
            arsonistController = charBody.gameObject.GetComponent<ArsonistController>();
            masochismController = charBody.gameObject.GetComponent<MasochismController>();

            GameObject stockContainer = new GameObject("Stock Container");
            crosshairStockContainer = UnityEngine.Object.Instantiate(stockContainer, gameObject.transform, false);

            isFlamethrower = arsonistController.flamethrowerSelected;

            if (isFlamethrower)
            {
                RenderFlamethrowerPositions();
            }
            else 
            {
                RenderFireballPositions();
            }

            Destroy(stockContainer);
        }

        public void RenderFlamethrowerPositions() 
        {
            //Create stock icons and parent them under the current gameObject.
            for (int i = 0; i < maxUIStockLimit; i++)
            {
                GameObject stockPrefab = new GameObject("Stock");
                GameObject stockObject = UnityEngine.Object.Instantiate(stockPrefab, crosshairStockContainer.transform, false);
                
                Image imageComponent = stockObject.AddComponent<Image>();
                imageComponent.sprite = Modules.AssetsArsonist.deactivatedStackSprite;

                RectTransform rectTransform = stockObject.GetComponent<RectTransform>();
                rectTransform.localPosition = stockPositionsFlamethrower[i];
                rectTransform.localScale = stockSize;

                stockObjects.Add(imageComponent);
                stockObject.SetActive(false);

                Destroy(stockPrefab);
            }
        }

        public void RenderFireballPositions() 
        {
            //Create stock icons and parent them under the current gameObject.
            for (int i = 0; i < maxUIStockLimit; i++)
            {
                GameObject stockPrefab = new GameObject("Stock");
                GameObject stockObject = UnityEngine.Object.Instantiate(stockPrefab, crosshairStockContainer.transform, false);
                
                Image imageComponent = stockObject.AddComponent<Image>();
                imageComponent.sprite = Modules.AssetsArsonist.deactivatedStackSprite;

                RectTransform rectTransform = stockObject.GetComponent<RectTransform>();
                rectTransform.localPosition = stockPositionsFireball[i];
                rectTransform.localScale = stockSize;

                stockObjects.Add(imageComponent);
                stockObject.SetActive(false);

                Destroy(stockPrefab);
            }
        }

        public void SetAllImageType(bool activateable) 
        {
            for (int i = 0; i < stockObjects.Count; i++) 
            {
                stockObjects[i].sprite = activateable ? Modules.AssetsArsonist.activatedStackSprite : Modules.AssetsArsonist.deactivatedStackSprite;
                stockObjects[i].color = activateable ? Color.white : opacityColor;
            }
        }

        public void Update() 
        {
            if (masochismController) 
            {
                //Hide/show appropriate stock objects
                for (int i = 0; i < stockObjects.Count; i++) 
                {
                    if (i <= masochismController.masoStacks - 1)
                    {
                        stockObjects[i].gameObject.SetActive(true);
                    }
                    else 
                    {
                        stockObjects[i].gameObject.SetActive(false);
                    }
                }

                //Change everything to Activateable sprites
                if (masochismController.masoStacks >= Modules.Config.masochismMinimumRequiredToActivate.Value)
                {
                    if (!activateable) 
                    {
                        //Change Stock
                        SetAllImageType(true);
                        activateable = true;
                    }
                }
                else 
                {
                    if(activateable) 
                    {
                        //Change Stock
                        SetAllImageType(false);
                        activateable = false;
                    }
                }
            }
        }

        public void OnDestroy() 
        {
            for (int i = 0; i < stockObjects.Count; i++)
            {
                Destroy(stockObjects[i].gameObject);
            }

            Destroy(crosshairStockContainer.gameObject);
        }
    }
}

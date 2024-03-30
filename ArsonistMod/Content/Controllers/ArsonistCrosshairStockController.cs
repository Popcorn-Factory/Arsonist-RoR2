using RoR2.UI;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace ArsonistMod.Content.Controllers
{
    internal class ArsonistCrosshairStockController : MonoBehaviour
    {
        public List<GameObject> stockObjects;
        private static int maxUIStockLimit = 10;
        public static int maxStock = Modules.Config.masochismMaximumStack.Value;
        public static int requiredStock = Modules.Config.masochismMinimumRequiredToActivate.Value;

        public GameObject crosshairStockContainer;
        public ArsonistController arsonistController;
        public CharacterBody charBody;

        public void Awake() 
        {
        
        }
        public void Start() 
        {
            //Get HUDelement from object, point to charobject and get ArsonistController
            charBody = gameObject.GetComponent<HudElement>().targetCharacterBody;
            arsonistController = charBody.gameObject.GetComponent<ArsonistController>();
            

            crosshairStockContainer = UnityEngine.Object.Instantiate(new GameObject(), gameObject.transform, false);
        }

        public void RenderFlamethrowerPositions() 
        {
            //Create stock icons and parent them under the current gameObject.
            for (int i = 0; i < maxUIStockLimit; i++)
            {

            }
        }

        public void RenderFireballPositions() 
        {
            //Create stock icons and parent them under the current gameObject.
            for (int i = 0; i < maxUIStockLimit; i++)
            {

            }
        }

        public void Update() 
        {
        
        }

        public void OnDestroy() 
        {
            
        }
    }
}

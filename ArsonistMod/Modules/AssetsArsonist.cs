using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;
using UnityEngine.UI;
using ArsonistMod.Content.Controllers;

namespace ArsonistMod.Modules
{
    internal static class AssetsArsonist
    {
        #region henry's stuff
        // particle effects
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;

        internal static GameObject bombExplosionEffect;

        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        #endregion

        //game effects
        public static GameObject lemfireBall = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/Fireball.prefab").WaitForCompletion();
        public static GameObject lemfireBallGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/FireballGhost.prefab").WaitForCompletion();
        public static GameObject elderlemurianexplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/OmniExplosionVFXLemurianBruiserFireballImpact.prefab").WaitForCompletion();
        public static GameObject artificerFireboltGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFireboltGhost.prefab").WaitForCompletion();
        public static GameObject explosionPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ExplosionVFX.prefab").WaitForCompletion();

        //arsonist effects
        internal static GameObject arsonistFiresprayExplosion;
        internal static GameObject arsonistFlare;
        internal static GameObject arsonistFlareAttached;
        internal static GameObject displayFire;
        internal static Material arsonistOverheatingMaterial;
        internal static Material emissionRingMat;
        internal static Material emissionRingMatLesser;

        internal static GameObject fireballStrongGhost;
        internal static GameObject fireballWeakGhost;

        internal static GameObject fireballScepterTracer;
        internal static GameObject fireballScepterWeakTracer;
        internal static GameObject fireballScepterOnHit;

        //Crosshair
        internal static GameObject fireballCrosshair;
        internal static GameObject flamethrowerCrosshair;

        //MasoSphere
        internal static GameObject masoSphereIndicator;

        //buffs
        public static Sprite blazingBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion().iconSprite;

        //Image icons
        internal static Sprite activatedStackSprite;
        internal static Sprite deactivatedStackSprite;

        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        // CHANGE THIS
        private const string assetbundleName = "arsonistassetbundle";
        //change this to your project's name if/when you've renamed it
        private const string csProjName = "ArsonistMod";
        private const string soundbankName = "arsonistSoundbank";


        internal static void Initialize()
        {
            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            try
            {
                if (mainAssetBundle == null)
                {
                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.{assetbundleName}"))
                    {
                        mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to load assetbundle. Make sure your assetbundle name is setup correctly\n" + e);
                return;
            }
        }

        internal static void LoadSoundbank()
        {                                                                
            //soundbank currently broke, but this is how you should load yours
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.{soundbankName}.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Log.Error("There is no AssetBundle to load assets from.");
                return;
            }

            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle
            bombExplosionEffect = LoadEffect("BombExplosionEffect", "Arsonist_Secondary_Flare_Punch_Explosion", false, false);

            if (bombExplosionEffect)
            {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            }

            //arsonist flare
            arsonistFlareAttached = LoadEffect("flareAttached", "", false, false);
            arsonistFlare = LoadEffect("flareShot", "", false, false);
            displayFire = LoadEffect("Fire", "", false, false);

            //arsonist firespray explosion
            arsonistFiresprayExplosion = PrefabAPI.InstantiateClone(elderlemurianexplosionEffect, "arsonistFireSprayExplosion");
            arsonistFiresprayExplosion.AddComponent<NetworkIdentity>();
            EffectComponent effect = arsonistFiresprayExplosion.GetComponent<EffectComponent>();
            if (!effect)
            {
                effect = arsonistFiresprayExplosion.AddComponent<EffectComponent>();
            }
            effect.applyScale = true;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = false;
            effect.positionAtReferencedTransform = true;
            effect.soundName = "Arsonist_Primary_Fire_Explosion";

            AddNewEffectDef(arsonistFiresprayExplosion, "Arsonist_Primary_Fire_Explosion");

            fireballStrongGhost = LoadEffect("FireballStrongProjectile", "", false, false);
            fireballStrongGhost.AddComponent<ProjectileGhostController>();

            fireballWeakGhost = LoadEffect("FireballWeakProjectile", "", false, false);
            fireballWeakGhost.AddComponent<ProjectileGhostController>();

            arsonistOverheatingMaterial = AssetsArsonist.mainAssetBundle.LoadAsset<Material>("OverheatingMaterial");

            emissionRingMat = Materials.CreateHopooMaterial("emissionRingMat", false, 10);
            emissionRingMatLesser = new Material(emissionRingMat);
            emissionRingMatLesser.SetFloat("_EmPower", 2f);

            //Create the damn reticle
            //Structured like this:
            /*
             Main obj (CrosshairController) (HudElement) (RawImage) (CanvasRenderer) (RectTransform)
                Images of each segment (Image component) (CanvasRenderer) (RectTransform)
             */

            //The Raw image of the parent object just contains the center dot.
            fireballCrosshair = PrefabAPI.InstantiateClone(Modules.AssetsArsonist.LoadCrosshair("Standard"), "ArsonistFireballCrosshair");
            //Change the bottom section reticle to the drop off sprite
            Transform bottomSection = fireballCrosshair.transform.GetChild(3);
            Image bottomSectionImage = bottomSection.gameObject.GetComponent<Image>();
            Sprite bottomImageSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("FireballReticle");
            bottomSectionImage.sprite = bottomImageSprite;
            bottomSectionImage.overrideSprite = bottomImageSprite;
            //Return the rotation back to normal.
            bottomSection.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 0f);
            bottomSection.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            fireballCrosshair.AddComponent<ArsonistCrosshairStockController>();

            CrosshairController fireballCrosshairController = fireballCrosshair.GetComponent<CrosshairController>();
            fireballCrosshairController.spriteSpreadPositions[0].zeroPosition = new Vector3(15f, 0f, 0f);
            fireballCrosshairController.spriteSpreadPositions[0].onePosition = new Vector3(30f, 0f, 0f);

            fireballCrosshairController.spriteSpreadPositions[1].zeroPosition = new Vector3(-15f, 0f, 0f);
            fireballCrosshairController.spriteSpreadPositions[1].onePosition = new Vector3(-30f, 0f, 0f);

            fireballCrosshairController.spriteSpreadPositions[2].zeroPosition = new Vector3(0f, 15f, 0f);
            fireballCrosshairController.spriteSpreadPositions[2].onePosition = new Vector3(0f, 30f, 0f);

            fireballCrosshairController.spriteSpreadPositions[3].zeroPosition = new Vector3(0f, -30f, 0f);
            fireballCrosshairController.spriteSpreadPositions[3].onePosition = new Vector3(0f, -30f, 0f);
            fireballCrosshairController.maxSpreadAngle = 1f;


            flamethrowerCrosshair = PrefabAPI.InstantiateClone(Modules.AssetsArsonist.LoadCrosshair("Standard"), "ArsonistFlamethrowerCrosshair");
            //Delete the last two elements as we only need L and R
            GameObject.Destroy(flamethrowerCrosshair.transform.GetChild(3).gameObject);
            GameObject.Destroy(flamethrowerCrosshair.transform.GetChild(2).gameObject);

            Transform rightSection = flamethrowerCrosshair.transform.GetChild(0);
            Image rightSectionImage = rightSection.gameObject.GetComponent<Image>();
            Sprite rightImageSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("FlamethrowerReticle");
            rightSectionImage.sprite = rightImageSprite;
            rightSectionImage.overrideSprite = rightImageSprite;
            rightSection.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 180f);
            rightSection.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);

            Transform leftSection = flamethrowerCrosshair.transform.GetChild(1);
            Image leftSectionImage = leftSection.gameObject.GetComponent<Image>();
            leftSectionImage.sprite = rightImageSprite;
            leftSectionImage.overrideSprite = rightImageSprite;
            leftSectionImage.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
            leftSectionImage.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);

            flamethrowerCrosshair.AddComponent<ArsonistCrosshairStockController>();

            CrosshairController flamethowerCrosshairController = flamethrowerCrosshair.GetComponent<CrosshairController>();

            CrosshairController.SpritePosition[] flamethrowerSpritePosition = new CrosshairController.SpritePosition[2];
            flamethrowerSpritePosition[0] = new CrosshairController.SpritePosition
            {
                target = rightSection.GetComponent<RectTransform>(),
                zeroPosition = new Vector3(30f, 0f, 0f),
                onePosition = new Vector3(60f, 0f, 0f)
            };
            flamethrowerSpritePosition[1] = new CrosshairController.SpritePosition
            {
                target = leftSection.GetComponent<RectTransform>(),
                zeroPosition = new Vector3(-30f, 0f, 0f),
                onePosition = new Vector3(-60f, 0f, 0f)
            };

            flamethowerCrosshairController.spriteSpreadPositions = flamethrowerSpritePosition;
            flamethowerCrosshairController.maxSpreadAngle = 1f;


            //Masochism Sphere
            masoSphereIndicator = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<GameObject>("Masosphere");

            //Masochism stack sprite
            activatedStackSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("masochismIconActivated");
            deactivatedStackSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("masochismIconDeactivated");

            fireballScepterTracer = LoadEffect("ScepterFireball", "", false, true, 1f);
            fireballScepterWeakTracer = LoadEffect("ScepterFireballWeak", "", false, true, 1f);
            fireballScepterOnHit = LoadEffect("OnHitScepterFireball", "Arsonist_Primary_Scepter_OnHit", false, true, 1f);
        }

        private static GameObject CreateOGTracer(string ogTracerPrefab, float speed = 100f, float length = 100f)
        {            
            GameObject gameobject = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<GameObject>(ogTracerPrefab);

            if (!gameobject.GetComponent<EffectComponent>()) gameobject.AddComponent<EffectComponent>();
            if (!gameobject.GetComponent<VFXAttributes>()) gameobject.AddComponent<VFXAttributes>();
            if (!gameobject.GetComponent<NetworkIdentity>()) gameobject.AddComponent<NetworkIdentity>();

            if(!gameobject.GetComponent<Tracer>()) gameobject.AddComponent<Tracer>(); 
            gameobject.GetComponent<Tracer>().speed = speed;
            gameobject.GetComponent<Tracer>().length = length;

            AddNewEffectDef(gameobject);

            return gameobject;
        }


        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (Renderer i in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                i?.material?.SetHopooMaterial();
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }


        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = mainAssetBundle.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Log.Error("Trying to load a null model- check to see if the BodyName in your code matches the prefab name of the object in Unity\nFor Example, if your prefab in unity is 'mdlArsonist', then your BodyName must be 'Arsonist'");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        internal static Texture LoadCharacterIconGeneric(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false, true);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false, true);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform, true);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform, bool addEffectComponent, float duration = 12f)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            newEffect.AddComponent<DestroyOnTimer>().duration = duration;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            if (addEffectComponent) 
            {
                var effect = newEffect.AddComponent<EffectComponent>();
                effect.applyScale = false;
                effect.effectIndex = EffectIndex.Invalid;
                effect.parentToReferencedTransform = parentToTransform;
                effect.positionAtReferencedTransform = true;
                effect.soundName = soundName;

                AddNewEffectDef(newEffect, soundName);
            }
            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }
    }
}   
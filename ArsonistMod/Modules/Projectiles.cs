using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using ArsonistMod.Modules.ProjectileControllers;
using ArsonistMod.SkillStates.Arsonist.Secondary;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject lemurianFireBall;
        internal static GameObject artificerFirebolt;
        internal static GameObject weakFlare;
        internal static GameObject strongFlare;
        internal static GameObject zeropointBomb;
        internal static GameObject flareChildPrefab;

        internal static void RegisterProjectiles()
        {
            CreateZeroPointBomb();
            AddProjectile(zeropointBomb);

            CreateLemurianFireBall();
            AddProjectile(lemurianFireBall);

            CreateArtificerFireBolt();
            AddProjectile(artificerFirebolt);

            CreateWeakFlare();
            AddProjectile(weakFlare);

            CreateStrongFlare();
            AddProjectile(strongFlare);

            CreateFlareChildPrefab();
            AddProjectile(flareChildPrefab);

        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }


        private static void CreateFlareChildPrefab()
        {
            flareChildPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareCollision");
            flareChildPrefab.AddComponent<NetworkIdentity>();

            ProjectileController projectileController = flareChildPrefab.AddComponent<ProjectileController>();
            projectileController.procCoefficient = 1f;
            projectileController.flightSoundLoop = ScriptableObject.CreateInstance<LoopSoundDef>();
            projectileController.flightSoundLoop.startSoundName = "Arsonist_Secondary_Flare_Projectile_Travel";
            projectileController.flightSoundLoop.stopSoundName = "Arsonist_Secondary_Flare_Projectile_Travel_Stop";
            projectileController.shouldPlaySounds = true;

            ProjectileDamage projectileDamage = flareChildPrefab.AddComponent<ProjectileDamage>();
            projectileDamage.damage = 10f;
            projectileDamage.crit = false;
            projectileDamage.force = 1000f;
            projectileDamage.damageType = DamageType.Generic;

            ProjectileImpactExplosion projectileExplosion = flareChildPrefab.AddComponent<ProjectileImpactExplosion>();
            projectileExplosion.explosionEffect = Assets.elderlemurianexplosionEffect;
            projectileExplosion.impactEffect = Assets.elderlemurianexplosionEffect;
            projectileExplosion.blastRadius = Modules.StaticValues.flareBlastRadius;
            projectileExplosion.blastDamageCoefficient = 1f;
            projectileExplosion.falloffModel = BlastAttack.FalloffModel.None;
            projectileExplosion.destroyOnEnemy = false;
            projectileExplosion.destroyOnWorld = true;
            projectileExplosion.lifetimeAfterImpact = 2f;
            projectileExplosion.lifetime = 5f;

            NetworkSoundEventDef soundEvent = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            soundEvent.akId = 3061346618;
            soundEvent.eventName = "Arsonist_Secondary_Flare_Explosion";
            Modules.Content.AddNetworkSoundEventDef(soundEvent);

            projectileExplosion.lifetimeExpiredSound = soundEvent;

            GameObject explosionEffect = projectileExplosion.impactEffect;
            explosionEffect.name = "explosion_effect_strong_child";
            EffectComponent effect = explosionEffect.GetComponent<EffectComponent>();
            effect.soundName = "Arsonist_Secondary_Flare_Explosion_New";

            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = explosionEffect;
            newEffectDef.prefabEffectComponent = explosionEffect.GetComponent<EffectComponent>();
            newEffectDef.prefabName = explosionEffect.name;
            newEffectDef.prefabVfxAttributes = explosionEffect.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = "Arsonist_Secondary_Flare_Explosion_New";

            projectileExplosion.impactEffect = explosionEffect;

            ProjectileSimple projectileSimple = flareChildPrefab.AddComponent<ProjectileSimple>();
            projectileSimple.lifetime = 10f;
            projectileSimple.desiredForwardSpeed = 30f;

            PrefabAPI.RegisterNetworkPrefab(flareChildPrefab);
        }


        private static void CreateStrongFlare()
        {
            strongFlare = CloneProjectilePrefab("LemurianBigFireball", "strongFlare");
            //strongFlare = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareShot");


            //ProjectileDamage strongFlareDamage = strongFlare.GetComponent<ProjectileDamage>();

            //if (!strongFlareDamage)
            //{
            //    strongFlareDamage = weakFlare.AddComponent<ProjectileDamage>();

            //}
            //strongFlareDamage.damage = 1f;
            //strongFlareDamage.damageType = DamageType.Generic;
            Rigidbody strongFlareRigidbody = strongFlare.GetComponent<Rigidbody>();
            if (!strongFlareRigidbody)
            {
                strongFlareRigidbody = strongFlare.AddComponent<Rigidbody>();
            }
            ProjectileImpactExplosion strongFlareexplosion = strongFlare.GetComponent<ProjectileImpactExplosion>();
            if (!strongFlareexplosion)
            {
                 strongFlareexplosion = strongFlare.AddComponent<ProjectileImpactExplosion>();
            }

            InitializeImpactExplosion(strongFlareexplosion);

            strongFlareexplosion.blastDamageCoefficient = 1f;
            strongFlareexplosion.blastProcCoefficient = 1f;
            strongFlareexplosion.blastRadius = StaticValues.flareBlastRadius;
            strongFlareexplosion.destroyOnEnemy = true;
            strongFlareexplosion.lifetime = 5f;
            strongFlareexplosion.explosionEffect = Assets.elderlemurianexplosionEffect;
            strongFlareexplosion.impactEffect = Assets.elderlemurianexplosionEffect;
            strongFlareexplosion.timerAfterImpact = true;
            strongFlareexplosion.lifetimeAfterImpact = 3f;

            NetworkSoundEventDef soundEvent = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            soundEvent.akId = 3061346618;
            soundEvent.eventName = "Arsonist_Secondary_Flare_Explosion";
            Modules.Content.AddNetworkSoundEventDef(soundEvent);

            strongFlareexplosion.lifetimeExpiredSound = soundEvent;

            GameObject explosionEffect = strongFlareexplosion.impactEffect;
            explosionEffect.name = "explosion_effect_flare_strong";
            EffectComponent effect = explosionEffect.GetComponent<EffectComponent>();
            effect.soundName = "Arsonist_Secondary_Flare_Explosion_New";

            //Make effectdef
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = explosionEffect;
            newEffectDef.prefabEffectComponent = explosionEffect.GetComponent<EffectComponent>();
            newEffectDef.prefabName = explosionEffect.name;
            newEffectDef.prefabVfxAttributes = explosionEffect.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = "Arsonist_Secondary_Flare_Explosion_New";

            Modules.Content.AddEffectDef(newEffectDef);

            strongFlareexplosion.impactEffect = explosionEffect;


            strongFlare.AddComponent<ZeroPointOnWorldHit>();


            //DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = strongFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            //damageTypeComponent.Add(Damage.arsonistStickyDamageType);

            ProjectileController strongFlareController = strongFlare.GetComponent<ProjectileController>();
            strongFlareController.rigidbody = strongFlareRigidbody;
            strongFlareController.rigidbody.useGravity = false;
            strongFlareController.procCoefficient = 1f;
            strongFlareController.flightSoundLoop = ScriptableObject.CreateInstance<LoopSoundDef>();
            strongFlareController.flightSoundLoop.startSoundName = "Arsonist_Secondary_Flare_Projectile _Travel";
            strongFlareController.flightSoundLoop.stopSoundName = "Arsonist_Secondary_Flare_Projectile_Travel_Stop";

            GameObject ghostPrefab = CreateGhostPrefab("flareShot", false);
            //ghostPrefab.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Materials.CreateHopooMaterial("emissionSphereMat", false, 10);
            //ghostPrefab.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = Materials.CreateHopooMaterial("emissionRingMat", false, 10);
            strongFlareController.ghostPrefab = ghostPrefab;

            strongFlareController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider)
            {
                collider.radius = 1.2f;
            }
        }


        private static void CreateWeakFlare()
        {
            weakFlare = CloneProjectilePrefab("LemurianBigFireball", "weakFlare");
            //weakFlare = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareShot");


            //ProjectileDamage weakFlareDamage = weakFlare.GetComponent<ProjectileDamage>();

            //if (!weakFlareDamage)
            //{
            //    weakFlareDamage = weakFlare.AddComponent<ProjectileDamage>();

            //}
            //weakFlareDamage.damage = 1f;
            //weakFlareDamage.damageType = DamageType.Generic;
            Rigidbody weakFlareRigidbody = weakFlare.GetComponent<Rigidbody>();
            if (!weakFlareRigidbody)
            {
                weakFlareRigidbody = weakFlare.AddComponent<Rigidbody>();
            }
            ProjectileImpactExplosion weakFlareexplosion = weakFlare.GetComponent<ProjectileImpactExplosion>();
            if (!weakFlareexplosion)
            {
                weakFlareexplosion = weakFlare.AddComponent<ProjectileImpactExplosion>();
            }

            InitializeImpactExplosion(weakFlareexplosion);

            weakFlareexplosion.blastDamageCoefficient = 1f;
            weakFlareexplosion.blastProcCoefficient = 1f;
            weakFlareexplosion.blastRadius = StaticValues.flareBlastRadius;
            weakFlareexplosion.destroyOnEnemy = true;
            weakFlareexplosion.lifetime = 5f;
            weakFlareexplosion.explosionEffect = Assets.elderlemurianexplosionEffect;
            weakFlareexplosion.impactEffect = Assets.elderlemurianexplosionEffect;
            weakFlareexplosion.timerAfterImpact = true;
            weakFlareexplosion.lifetimeAfterImpact = 3f;

            NetworkSoundEventDef soundEvent = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            soundEvent.akId = 3061346618;
            soundEvent.eventName = "Arsonist_Secondary_Flare_Explosion";
            Modules.Content.AddNetworkSoundEventDef(soundEvent);

            weakFlareexplosion.lifetimeExpiredSound = soundEvent;

            GameObject explosionEffect = weakFlareexplosion.impactEffect;
            explosionEffect.name = "explosion_effect_flare_strong";
            EffectComponent effect = explosionEffect.GetComponent<EffectComponent>();
            effect.soundName = "Arsonist_Secondary_Flare_Explosion_New";

            //Make effectdef
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = explosionEffect;
            newEffectDef.prefabEffectComponent = explosionEffect.GetComponent<EffectComponent>();
            newEffectDef.prefabName = explosionEffect.name;
            newEffectDef.prefabVfxAttributes = explosionEffect.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = "Arsonist_Secondary_Flare_Explosion_New";

            Modules.Content.AddEffectDef(newEffectDef);

            weakFlareexplosion.impactEffect = explosionEffect;


            weakFlare.AddComponent<ZeroPointOnWorldHit>();


            ProjectileController weakFlareController = weakFlare.GetComponent<ProjectileController>();
            weakFlareController.rigidbody = weakFlareRigidbody;
            weakFlareController.rigidbody.useGravity = false;
            weakFlareController.procCoefficient = 1f;
            weakFlareController.flightSoundLoop = ScriptableObject.CreateInstance<LoopSoundDef>();
            weakFlareController.flightSoundLoop.startSoundName = "Arsonist_Secondary_Flare_Projectile_Travel";
            weakFlareController.flightSoundLoop.stopSoundName = "Arsonist_Secondary_Flare_Projectile_Travel_Stop";

            GameObject ghostPrefab = CreateGhostPrefab("flareShot", false);
            //ghostPrefab.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Materials.CreateHopooMaterial("emissionSphereMat", false, 10);
            //ghostPrefab.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = Materials.CreateHopooMaterial("emissionRingMat", false, 10);
            weakFlareController.ghostPrefab = ghostPrefab;

            weakFlareController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider)
            {
                collider.radius = 1.2f;
            }

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = weakFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(Damage.arsonistWeakStickyDamageType);
        }

        private static void CreateZeroPointBomb()
        {
            zeropointBomb = CloneProjectilePrefab("CommandoGrenadeProjectile", "zeropointBomb");

            ProjectileSimple zeropointBombTrajectory = zeropointBomb.GetComponent<ProjectileSimple>();
            zeropointBombTrajectory.lifetime = 20f;

            Rigidbody zeropointBombRigidbody = zeropointBomb.GetComponent<Rigidbody>();
            if (!zeropointBombRigidbody)
            {
                zeropointBombRigidbody = zeropointBomb.AddComponent<Rigidbody>();
            }

            ProjectileImpactExplosion zeropointBombexplosion = zeropointBomb.GetComponent<ProjectileImpactExplosion>();


            if (!zeropointBombexplosion)
            {
                zeropointBombexplosion = zeropointBomb.AddComponent<ProjectileImpactExplosion>();

            }
            InitializeImpactExplosion(zeropointBombexplosion);

            zeropointBombexplosion.blastDamageCoefficient = 1f;
            zeropointBombexplosion.blastProcCoefficient = 1f;
            zeropointBombexplosion.blastRadius = StaticValues.zeropointBlastRadius;
            zeropointBombexplosion.destroyOnEnemy = true;
            zeropointBombexplosion.lifetime = 20f;
            zeropointBombexplosion.impactEffect = Assets.bombExplosionEffect;
            zeropointBombexplosion.timerAfterImpact = true;
            zeropointBombexplosion.lifetimeAfterImpact = 3f;

            zeropointBombexplosion.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            ProjectileController zeropointBombController = zeropointBomb.GetComponent<ProjectileController>();
            zeropointBombController.rigidbody = zeropointBombRigidbody;
            zeropointBombController.rigidbody.useGravity = true;
            zeropointBombController.rigidbody.mass = 1000f;
            zeropointBombController.procCoefficient = 1f;
            
            
            zeropointBombController.ghostPrefab = Assets.arsonistFlare;
            zeropointBombController.startSound = "";

            zeropointBomb.AddComponent<ZeroPointOnWorldHit>();
            PrefabAPI.RegisterNetworkPrefab(zeropointBomb);
        }

        private static void CreateLemurianFireBall()
        {
            lemurianFireBall = PrefabAPI.InstantiateClone(Modules.Assets.lemfireBall, "lemurianFireBall", true);
            Rigidbody lemurianFireballRigidbody = lemurianFireBall.GetComponent<Rigidbody>();
            if (!lemurianFireballRigidbody)
            {
                lemurianFireballRigidbody = lemurianFireBall.AddComponent<Rigidbody>();
            }

            ProjectileImpactExplosion lemurianFireBallexplosion = lemurianFireBall.GetComponent<ProjectileImpactExplosion>();

            if (!lemurianFireBallexplosion)
            {
                lemurianFireBallexplosion = lemurianFireBall.AddComponent<ProjectileImpactExplosion>();

            }
            InitializeImpactExplosion(lemurianFireBallexplosion);

            lemurianFireBallexplosion.blastDamageCoefficient = 1f;
            lemurianFireBallexplosion.blastProcCoefficient = 1f;
            lemurianFireBallexplosion.blastRadius = StaticValues.firesprayweakBlastRadius;
            lemurianFireBallexplosion.destroyOnEnemy = true;
            lemurianFireBallexplosion.lifetime = 6f;
            lemurianFireBallexplosion.impactEffect = EntityStates.LemurianMonster.FireFireball.effectPrefab;
            lemurianFireBallexplosion.timerAfterImpact = false;
            lemurianFireBallexplosion.lifetimeAfterImpact = 0f;
            lemurianFireBallexplosion.destroyOnWorld = true;

            ProjectileController lemurianFireBallController = lemurianFireBall.GetComponent<ProjectileController>();
            lemurianFireBallController.rigidbody = lemurianFireballRigidbody;
            lemurianFireBallController.rigidbody.useGravity = true;
            lemurianFireBallController.rigidbody.mass = 1f;
            lemurianFireBallController.procCoefficient = 1f;

            lemurianFireBallController.ghostPrefab = Modules.Assets.fireballWeakGhost;
            lemurianFireBallController.startSound = "";
        }
        private static void CreateArtificerFireBolt()
        {
            artificerFirebolt = CloneProjectilePrefab("MageFirebolt", "artificerFireBolt");

            StrongFiresprayOnHit artificerFireboltexplosion = artificerFirebolt.GetComponent<StrongFiresprayOnHit>();
            if (!artificerFireboltexplosion)
            {
                artificerFireboltexplosion = artificerFirebolt.AddComponent<StrongFiresprayOnHit>();
            }
            Rigidbody artificerFireboltRigidbody = artificerFirebolt.GetComponent<Rigidbody>();
            if (!artificerFireboltRigidbody)
            {
                artificerFireboltRigidbody = artificerFirebolt.AddComponent<Rigidbody>();
            }

            ProjectileController projectileDamage = artificerFirebolt.GetComponent<ProjectileController>();
            if (!projectileDamage) 
            {
                projectileDamage = artificerFirebolt.AddComponent<ProjectileController>();
            }
            projectileDamage.procCoefficient = 1f;

            InitializeStrongFireSprayOnHit(artificerFireboltexplosion);

            artificerFireboltexplosion.blastDamageCoefficient = 1f;
            artificerFireboltexplosion.blastProcCoefficient = 1f;
            artificerFireboltexplosion.blastRadius = StaticValues.firesprayBlastRadius;
            artificerFireboltexplosion.destroyOnEnemy = true;
            artificerFireboltexplosion.lifetime = 6f;
			
            artificerFireboltexplosion.impactEffect = Assets.arsonistFiresprayExplosion;


            //Setup impact effect
            ProjectileImpactExplosion projImpact = artificerFirebolt.GetComponent<ProjectileImpactExplosion>();
            //Effect we want to create from artificer bolt.
            GameObject explosionEffect = UnityEngine.Object.Instantiate(projImpact.impactEffect);
            explosionEffect.name = "explosion_effect";
            EffectComponent effect = explosionEffect.GetComponent<EffectComponent>();
            effect.soundName = "Arsonist_Primary_Fire_Explosion";

            //Make effectdef
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = explosionEffect;
            newEffectDef.prefabEffectComponent = explosionEffect.GetComponent<EffectComponent>();
            newEffectDef.prefabName = explosionEffect.name;
            newEffectDef.prefabVfxAttributes = explosionEffect.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = "Arsonist_Primary_Fire_Explosion";

            Modules.Content.AddEffectDef(newEffectDef);

            artificerFireboltexplosion.timerAfterImpact = false;
            artificerFireboltexplosion.lifetimeAfterImpact = 0f;
            artificerFireboltexplosion.destroyOnWorld = true;
            artificerFireboltexplosion.onWorldCollisionBlastRadius = 5f;
            artificerFireboltexplosion.onEnemyCollisionBlastRadius = 7f;

            artificerFireboltexplosion.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            ProjectileController artificerFireboltController = artificerFirebolt.GetComponent<ProjectileController>();
            artificerFireboltController.rigidbody = artificerFireboltRigidbody;
            artificerFireboltController.rigidbody.useGravity = true;
            artificerFireboltController.rigidbody.mass = 1f;
            artificerFireboltController.flightSoundLoop = ScriptableObject.CreateInstance<LoopSoundDef>();
            artificerFireboltController.flightSoundLoop.startSoundName = "Arsonist_Primary_Fire_Travel";
            artificerFireboltController.flightSoundLoop.stopSoundName = "Arsonist_Primary_Fire_Travel_Stop";
            artificerFireboltController.shouldPlaySounds = true;



            artificerFireboltController.ghostPrefab = Modules.Assets.fireballStrongGhost;
            artificerFireboltController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider) 
            {
                collider.radius = 1.0f;
            }

            //ProjectileImpactExplosion projImpact = artificerFirebolt.GetComponent<ProjectileImpactExplosion>();
            //GameObject explosionEffect = UnityEngine.Object.Instantiate(projImpact.impactEffect);
            //explosionEffect.name = "new_effect";
            //EffectComponent effect = explosionEffect.GetComponent<EffectComponent>();
            //effect.soundName = "Arsonist_Primary_Fire_Explosion";

            ////Make effectdef
            //EffectDef newEffectDef = new EffectDef();
            //newEffectDef.prefab = explosionEffect;
            //newEffectDef.prefabEffectComponent = explosionEffect.GetComponent<EffectComponent>();
            //newEffectDef.prefabName = explosionEffect.name;
            //newEffectDef.prefabVfxAttributes = explosionEffect.GetComponent<VFXAttributes>();
            //newEffectDef.spawnSoundEventName = "Arsonist_Primary_Fire_Explosion";

            //Modules.Content.AddEffectDef(newEffectDef);

            //projImpact.impactEffect = explosionEffect;

            //Debug.Log(projImpact.impactEffect);
            UnityEngine.Object.Destroy(projImpact);

            //Component[] components = artificerFirebolt.GetComponents<Component>();
            //foreach (Component component in components) 
            //{
            //    Debug.Log(component.GetType().ToString());
            //}

        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static void InitializeStrongFireSprayOnHit(StrongFiresprayOnHit projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName, bool hopoo)
        {

            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName) != null)
            {
                GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
                if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
                if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

                if (hopoo) Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

                return ghostPrefab;
            }
            return null;       


        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }


        //internal class StrongFlareOnWorldHit : MonoBehaviour, IProjectileImpactBehavior
        //{

        //    public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        //    {
        //        if (impactInfo.collider)
        //        {
        //            GameObject collidedObject = impactInfo.collider.gameObject;

        //            Vector3 pos = impactInfo.estimatedPointOfImpact;

        //            CharacterBody body = collidedObject.GetComponent<CharacterBody>();
        //            if (!body)
        //            {
        //                FlareEffectControllerStrongWorld flarecon = collidedObject.AddComponent<FlareEffectControllerStrongWorld>();
        //                flarecon.worldPos = pos;
        //                flarecon.arsonistBody = strongFlare.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
        //                Instantiate(collidedObject, pos, collidedObject.transform.rotation);

        //            }
        //        }
        //    }
        //}

        //internal class WeakFlareOnWorldHit : MonoBehaviour, IProjectileImpactBehavior
        //{

        //    public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        //    {
        //        if (impactInfo.collider)
        //        {
        //            GameObject collidedObject = impactInfo.collider.gameObject;

        //            Vector3 pos = impactInfo.estimatedPointOfImpact;

        //            CharacterBody body = collidedObject.GetComponent<CharacterBody>();
        //            if (!body)
        //            {
        //
        //                EffectControllerWeakWorld flarecon = collidedObject.AddComponent<FlareEffectControllerWeakWorld>();
        //                flarecon.worldPos = pos;
        //                flarecon.arsonistBody = strongFlare.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
        //                Instantiate(collidedObject, pos, collidedObject.transform.rotation);

        //            }
        //        }
        //    }
        //}

        internal class ZeroPointOnWorldHit : MonoBehaviour, IProjectileImpactBehavior
        {

            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider)
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;
                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();

                    if (!body)
                    {
                        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                        if (rigidbody)
                        {
                            rigidbody.useGravity = false;
                            rigidbody.velocity = Vector3.zero;
                            rigidbody.angularVelocity = Vector3.zero;
                        }
                    }
                }
            }
        }


        internal class WeakFlareOnHit : MonoBehaviour, IProjectileImpactBehavior
        {
            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider)
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;
                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();
                    if (body)
                    {
                        if (body.teamComponent)
                        {
                            if (body.teamComponent.teamIndex == TeamIndex.Neutral || body.teamComponent.teamIndex == TeamIndex.Monster
                                || body.teamComponent.teamIndex == TeamIndex.Lunar || body.teamComponent.teamIndex == TeamIndex.Void)
                            {
                                FlareEffectControllerStrong flareCon = body.gameObject.AddComponent<FlareEffectControllerStrong>();
                                flareCon.arsonistBody = gameObject.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
                            }
                        }
                    }
                    else
                    {
                        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                        if (rigidbody)
                        {
                            rigidbody.useGravity = false;
                            rigidbody.velocity = Vector3.zero;
                        }
                    }
                }
            }
        }

        internal class StrongFlareOnHit : MonoBehaviour, IProjectileImpactBehavior
        {
            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider)
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;
                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();
                    if (body)
                    {
                        if (body.teamComponent)
                        {
                            if (body.teamComponent.teamIndex == TeamIndex.Neutral || body.teamComponent.teamIndex == TeamIndex.Monster
                                || body.teamComponent.teamIndex == TeamIndex.Lunar || body.teamComponent.teamIndex == TeamIndex.Void)
                            {
                                Destroy(gameObject);
                                FlareEffectControllerStrong flareCon = body.gameObject.AddComponent<FlareEffectControllerStrong>();
                                flareCon.arsonistBody = gameObject.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
                            }
                        }
                    }
                    else
                    {
                        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                        if (rigidbody)
                        {
                            rigidbody.useGravity = false;
                            rigidbody.velocity = Vector3.zero;
                        }
                    }
                }
            }
        }
    }
}
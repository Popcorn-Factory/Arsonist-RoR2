using ArsonistMod.Content.Controllers;
using ArsonistMod.Modules.Networking;
using ArsonistMod.Modules.ProjectileControllers;
using ArsonistMod.SkillStates.Arsonist.Secondary;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
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



        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }





        private static void CreateStrongFlare()
        {
            strongFlare = CloneProjectilePrefab("magefirebolt", "strongFlare");


            ProjectileDamage strongFlareDamage = strongFlare.GetComponent<ProjectileDamage>();

            if (!strongFlareDamage)
            {
                strongFlareDamage = weakFlare.AddComponent<ProjectileDamage>();

            }
            strongFlareDamage.damage = 1f;
            strongFlareDamage.damageType = DamageType.Generic;

            //ProjectileImpactExplosion strongFlareexplosion = strongFlare.GetComponent<ProjectileImpactExplosion>();

            //InitializeImpactExplosion(strongFlareexplosion);

            //strongFlareexplosion.blastDamageCoefficient = 1f;
            //strongFlareexplosion.blastProcCoefficient = 1f;
            //strongFlareexplosion.blastRadius = StaticValues.flareBlastRadius;
            //strongFlareexplosion.destroyOnEnemy = true;
            //strongFlareexplosion.lifetime = 5f;
            //strongFlareexplosion.impactEffect = Assets.explosionPrefab;
            //strongFlareexplosion.timerAfterImpact = true;
            //strongFlareexplosion.lifetimeAfterImpact = 3f;


            //strongFlare.AddComponent<ZeroPointOnWorldHit>();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = strongFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(Damage.arsonistStickyDamageType);

            ProjectileController strongFlareFireboltController = strongFlare.GetComponent<ProjectileController>();

            strongFlareFireboltController.ghostPrefab = CreateGhostPrefab("flare");
            strongFlareFireboltController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider)
            {
                Debug.Log(collider.radius);
                collider.radius = 1.0f;
            }
            PrefabAPI.RegisterNetworkPrefab(strongFlare);
        }


        private static void CreateWeakFlare()
        {
            weakFlare = CloneProjectilePrefab("magefirebolt", "weakFlare");

            ProjectileImpactExplosion weakFlareexplosion = weakFlare.GetComponent<ProjectileImpactExplosion>();
            
            InitializeImpactExplosion(weakFlareexplosion);

            weakFlareexplosion.blastDamageCoefficient = 1f;
            weakFlareexplosion.blastProcCoefficient = 1f;
            weakFlareexplosion.blastRadius = StaticValues.flareBlastRadius;
            weakFlareexplosion.destroyOnEnemy = true;
            weakFlareexplosion.lifetime = 5f;
            weakFlareexplosion.impactEffect = Assets.explosionPrefab;
            weakFlareexplosion.timerAfterImpact = true;
            weakFlareexplosion.lifetimeAfterImpact = 3f;

            weakFlare.AddComponent<ZeroPointOnWorldHit>();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = weakFlare.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(Damage.arsonistWeakStickyDamageType);

            ProjectileController weakFlareFireboltController = weakFlare.GetComponent<ProjectileController>();

            weakFlareFireboltController.ghostPrefab = CreateGhostPrefab("flare");
            weakFlareFireboltController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider)
            {
                Debug.Log(collider.radius);
                collider.radius = 1.0f;
            }
            PrefabAPI.RegisterNetworkPrefab(weakFlare);
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
            
            
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareAttached") != null) zeropointBombController.ghostPrefab = Assets.arsonistFlare;
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

            if (Assets.lemfireBallGhost != null) lemurianFireBallController.ghostPrefab = Assets.lemfireBallGhost;
            lemurianFireBallController.startSound = "";
        }
        private static void CreateArtificerFireBolt()
        {
            artificerFirebolt = CloneProjectilePrefab("MageFirebolt", "artificerFireBolt");

            StrongFiresprayOnHit artificerFireboltexplosion = artificerFirebolt.GetComponent<StrongFiresprayOnHit>();
            if (!artificerFireboltexplosion)
            {
                if (artificerFirebolt.GetComponent<ProjectileImpactExplosion>()) 
                {
                    UnityEngine.Object.Destroy(artificerFirebolt.GetComponent<ProjectileImpactExplosion>());
                }
                artificerFireboltexplosion = artificerFirebolt.AddComponent<StrongFiresprayOnHit>();
            }
            Rigidbody artificerFireboltRigidbody = artificerFirebolt.GetComponent<Rigidbody>();
            if (!artificerFireboltRigidbody)
            {
                artificerFireboltRigidbody = artificerFirebolt.AddComponent<Rigidbody>();
            }

            InitializeStrongFireSprayOnHit(artificerFireboltexplosion);

            artificerFireboltexplosion.blastDamageCoefficient = 1f;
            artificerFireboltexplosion.blastProcCoefficient = 1f;
            artificerFireboltexplosion.blastRadius = StaticValues.firesprayBlastRadius;
            artificerFireboltexplosion.destroyOnEnemy = true;
            artificerFireboltexplosion.lifetime = 6f;
            artificerFireboltexplosion.impactEffect = Assets.explosionPrefab;
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

            if (Assets.artificerFireboltGhost != null) artificerFireboltController.ghostPrefab = Assets.artificerFireboltGhost;
            artificerFireboltController.startSound = "";

            SphereCollider collider = artificerFirebolt.GetComponent<SphereCollider>();

            if (collider) 
            {
                Debug.Log(collider.radius);
                collider.radius = 1.0f;
            }
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

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }


        internal class StrongFlareOnWorldHit : MonoBehaviour, IProjectileImpactBehavior
        {

            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider)
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;

                    Vector3 pos = impactInfo.estimatedPointOfImpact;

                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();
                    if (!body)
                    {
                        FlareEffectControllerStrongWorld flarecon = collidedObject.AddComponent<FlareEffectControllerStrongWorld>();
                        flarecon.worldPos = pos;
                        flarecon.arsonistBody = strongFlare.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
                        Instantiate(collidedObject, pos, collidedObject.transform.rotation);

                    }
                }
            }
        }

        internal class WeakFlareOnWorldHit : MonoBehaviour, IProjectileImpactBehavior
        {

            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                if (impactInfo.collider)
                {
                    GameObject collidedObject = impactInfo.collider.gameObject;

                    Vector3 pos = impactInfo.estimatedPointOfImpact;

                    CharacterBody body = collidedObject.GetComponent<CharacterBody>();
                    if (!body)
                    {
                        FlareEffectControllerWeakWorld flarecon = collidedObject.AddComponent<FlareEffectControllerWeakWorld>();
                        flarecon.worldPos = pos;
                        flarecon.arsonistBody = strongFlare.GetComponent<ProjectileController>().owner.GetComponent<CharacterBody>();
                        Instantiate(collidedObject, pos, collidedObject.transform.rotation);

                    }
                }
            }
        }

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
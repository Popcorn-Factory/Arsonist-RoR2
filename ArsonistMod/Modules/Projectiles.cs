using ArsonistMod.SkillStates.Arsonist.Secondary;
using R2API;
using R2API.Networking;
using RoR2;
using RoR2.Projectile;
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

        internal static void RegisterProjectiles()
        {
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

        private static void CreateWeakFlare()
        {
            weakFlare = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("WeakFlare");
            // Ensure that the child is set in the right position in Unity!!!!
            Modules.Prefabs.SetupHitbox(weakFlare, weakFlare.transform.GetChild(0), "FlareHitbox");
            weakFlare.AddComponent<NetworkIdentity>();
            ProjectileController flareCon = weakFlare.AddComponent<ProjectileController>();

            ProjectileDamage flareDamage = weakFlare.AddComponent<ProjectileDamage>();
            InitializeFlareDamage(flareDamage);

            ProjectileSimple flareTrajectory = weakFlare.AddComponent<ProjectileSimple>();
            InitializeFlareTrajectory(flareTrajectory, 50f);

            ProjectileOverlapAttack flareoverlapAttack = weakFlare.AddComponent<ProjectileOverlapAttack>();
            InitializeFlareOverlapAttack(flareoverlapAttack);
            weakFlare.AddComponent<WeakFlareOnHit>();

            PrefabAPI.RegisterNetworkPrefab(weakFlare);
        }

        private static void CreateStrongFlare()
        {
            strongFlare = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("StrongFlare");
            // Ensure that the child is set in the right position in Unity!!!!
            Modules.Prefabs.SetupHitbox(strongFlare, strongFlare.transform.GetChild(0), "FlareHitbox");
            strongFlare.AddComponent<NetworkIdentity>();
            ProjectileController flareCon = strongFlare.AddComponent<ProjectileController>();

            ProjectileDamage flareDamage = strongFlare.AddComponent<ProjectileDamage>();
            InitializeFlareDamage(flareDamage);

            ProjectileSimple flareTrajectory = strongFlare.AddComponent<ProjectileSimple>();
            InitializeFlareTrajectory(flareTrajectory, 50f);

            ProjectileOverlapAttack flareoverlapAttack = strongFlare.AddComponent<ProjectileOverlapAttack>();
            InitializeFlareOverlapAttack(flareoverlapAttack);
            strongFlare.AddComponent<WeakFlareOnHit>();

            PrefabAPI.RegisterNetworkPrefab(strongFlare);
        }

        internal static void InitializeFlareOverlapAttack(ProjectileOverlapAttack overlap)
        {
            overlap.overlapProcCoefficient = 1.0f;
            overlap.damageCoefficient = 1.0f;
        }

        internal static void InitializeFlareTrajectory(ProjectileSimple simple, float speed)
        {
            simple.lifetime = 5f;
            simple.desiredForwardSpeed = speed;

        }

        internal static void InitializeFlareDamage(ProjectileDamage damageComponent)
        {
            damageComponent.damage = 0f;
            damageComponent.crit = false;
            damageComponent.force = 0f;
            damageComponent.damageType = DamageType.Generic;
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
            lemurianFireBallexplosion.blastRadius = 3f;
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

            ProjectileImpactExplosion artificerFireboltexplosion = artificerFirebolt.GetComponent<ProjectileImpactExplosion>();
            if (!artificerFireboltexplosion)
            {
                artificerFireboltexplosion = artificerFirebolt.AddComponent<ProjectileImpactExplosion>();
            }

            InitializeImpactExplosion(artificerFireboltexplosion);

            artificerFireboltexplosion.blastDamageCoefficient = 1f;
            artificerFireboltexplosion.blastProcCoefficient = 1f;
            artificerFireboltexplosion.blastRadius = 6f;
            artificerFireboltexplosion.destroyOnEnemy = true;
            artificerFireboltexplosion.lifetime = 6f;
            artificerFireboltexplosion.impactEffect = Assets.explosionPrefab;
            artificerFireboltexplosion.timerAfterImpact = false;
            artificerFireboltexplosion.lifetimeAfterImpact = 0f;
            artificerFireboltexplosion.destroyOnWorld = true;

            artificerFireboltexplosion.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            ProjectileController artificerFireboltController = artificerFirebolt.GetComponent<ProjectileController>();

            if (Assets.artificerFireboltGhost != null) artificerFireboltController.ghostPrefab = Assets.artificerFireboltGhost;
            artificerFireboltController.startSound = "";
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
                                if(body.GetBuffCount(Buffs.flareStrongBuff) >= 1)
                                {
                                    body.SetBuffCount(Buffs.flareStrongBuff.buffIndex, 0);
                                }
                                body.SetBuffCount(Buffs.FlareWeakBuff.buffIndex, 5);
                            }
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
                                if (body.GetBuffCount(Buffs.flareStrongBuff) >= 1)
                                {
                                    body.SetBuffCount(Buffs.flareStrongBuff.buffIndex, 0);
                                }
                                body.SetBuffCount(Buffs.FlareWeakBuff.buffIndex, 5);

                                FlareEffectController controller = body.gameObject.GetComponent<FlareEffectController>();
                                if (!controller)
                                {
                                    controller = body.gameObject.AddComponent<FlareEffectController>();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
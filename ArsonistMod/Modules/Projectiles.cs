using R2API;
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

        internal static void RegisterProjectiles()
        {
            CreateBomb();

            AddProjectile(bombPrefab);

            CreateLemurianFireBall();
            AddProjectile(lemurianFireBall);

            CreateArtificerFireBolt();
            AddProjectile(artificerFirebolt);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "ArsonistBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("ArsonistBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ArsonistBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("ArsonistBombGhost");
            bombController.startSound = "";
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
            artificerFirebolt = CloneProjectilePrefab("MageFirebolt", "artificerFireBolt" );

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
    }
}
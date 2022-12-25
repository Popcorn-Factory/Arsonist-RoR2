using HG;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.Modules.ProjectileControllers
{

    //A literal clone of ProjectileImpactExplosion
    [RequireComponent(typeof(ProjectileController))]
    public class StrongFiresprayOnHit : ProjectileExplosion, IProjectileImpactBehavior
    {
        // Token: 0x060043A9 RID: 17321 RVA: 0x001191ED File Offset: 0x001173ED
        public override void Awake()
        {
            base.Awake();
            this.lifetime += UnityEngine.Random.Range(0f, this.lifetimeRandomOffset);
        }

        // Token: 0x060043AA RID: 17322 RVA: 0x00119214 File Offset: 0x00117414
        protected void FixedUpdate()
        {
            this.stopwatch += Time.fixedDeltaTime;
            if (NetworkServer.active || this.projectileController.isPrediction)
            {
                if (this.timerAfterImpact && this.hasImpact)
                {
                    this.stopwatchAfterImpact += Time.fixedDeltaTime;
                }
                bool flag = this.stopwatch >= this.lifetime;
                bool flag2 = this.timerAfterImpact && this.stopwatchAfterImpact > this.lifetimeAfterImpact;
                bool flag3 = this.projectileHealthComponent && !this.projectileHealthComponent.alive;
                if (flag || flag2 || flag3)
                {
                    this.alive = false;
                }
                if (this.alive && !this.hasPlayedLifetimeExpiredSound)
                {
                    bool flag4 = this.stopwatch > this.lifetime - this.offsetForLifetimeExpiredSound;
                    if (this.timerAfterImpact)
                    {
                        flag4 |= (this.stopwatchAfterImpact > this.lifetimeAfterImpact - this.offsetForLifetimeExpiredSound);
                    }
                    if (flag4)
                    {
                        this.hasPlayedLifetimeExpiredSound = true;
                        if (NetworkServer.active && this.lifetimeExpiredSound)
                        {
                            PointSoundManager.EmitSoundServer(this.lifetimeExpiredSound.index, base.transform.position);
                        }
                    }
                }
                if (!this.alive)
                {
                    this.explosionEffect = (this.impactEffect ?? this.explosionEffect);
                    CustomDetonate();
                }
            }
        }

        public void CustomDetonate()
        {
            if (NetworkServer.active)
            {
                if (isWorldCollided)
                {
                    base.blastRadius = onWorldCollisionBlastRadius;
                }
                else 
                {
                    base.blastRadius = onEnemyCollisionBlastRadius;
                }
                base.DetonateServer();
            }
            UnityEngine.Object.Destroy(base.gameObject);
        }

        // Token: 0x060043AB RID: 17323 RVA: 0x00119368 File Offset: 0x00117568
        public override Quaternion GetRandomDirectionForChild()
        {
            Quaternion randomChildRollPitch = base.GetRandomChildRollPitch();
            ProjectileImpactExplosion.TransformSpace transformSpace = this.transformSpace;
            if (transformSpace == ProjectileImpactExplosion.TransformSpace.Local)
            {
                return base.transform.rotation * randomChildRollPitch;
            }
            if (transformSpace != ProjectileImpactExplosion.TransformSpace.Normal)
            {
                return randomChildRollPitch;
            }
            return Quaternion.FromToRotation(Vector3.forward, this.impactNormal) * randomChildRollPitch;
        }

        // Token: 0x060043AC RID: 17324 RVA: 0x001193B8 File Offset: 0x001175B8
        public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        {
            if (!this.alive)
            {
                return;
            }
            Collider collider = impactInfo.collider;
            this.impactNormal = impactInfo.estimatedImpactNormal;
            if (collider)
            {
                DamageInfo damageInfo = new DamageInfo();
                if (this.projectileDamage)
                {
                    damageInfo.damage = this.projectileDamage.damage;
                    damageInfo.crit = this.projectileDamage.crit;
                    damageInfo.attacker = (this.projectileController.owner ? this.projectileController.owner.gameObject : null);
                    damageInfo.inflictor = base.gameObject;
                    damageInfo.position = impactInfo.estimatedPointOfImpact;
                    damageInfo.force = this.projectileDamage.force * base.transform.forward;
                    damageInfo.procChainMask = this.projectileController.procChainMask;
                    damageInfo.procCoefficient = this.projectileController.procCoefficient;
                }
                else
                {
                    Debug.Log("No projectile damage component!");
                }
                HurtBox component = collider.GetComponent<HurtBox>();
                if (component)
                {
                    if (this.destroyOnEnemy)
                    {
                        isWorldCollided = false;
                        HealthComponent healthComponent = component.healthComponent;
                        if (healthComponent)
                        {
                            if (healthComponent.gameObject == this.projectileController.owner)
                            {
                                return;
                            }
                            if (this.projectileHealthComponent && healthComponent == this.projectileHealthComponent)
                            {
                                return;
                            }
                            this.alive = false;
                        }
                    }
                }
                else if (this.destroyOnWorld)
                {
                    this.alive = false;
                    isWorldCollided = true;
                }
                this.hasImpact = (component || this.impactOnWorld);
                if (NetworkServer.active && this.hasImpact)
                {
                    GlobalEventManager.instance.OnHitAll(damageInfo, collider.gameObject);
                }
            }
        }

        // Token: 0x060043AD RID: 17325 RVA: 0x00119560 File Offset: 0x00117760
        public override void OnValidate()
        {
            if (Application.IsPlaying(this))
            {
                return;
            }
            base.OnValidate();
            if (!string.IsNullOrEmpty(this.lifetimeExpiredSoundString))
            {
                Debug.LogWarningFormat(base.gameObject, "{0} ProjectileImpactExplosion component supplies a value in the lifetimeExpiredSoundString field. This will not play correctly over the network. Please use lifetimeExpiredSound instead.", new object[]
                {
                    Util.GetGameObjectHierarchyName(base.gameObject)
                });
            }
        }

        // Token: 0x040041FF RID: 16895
        private Vector3 impactNormal = Vector3.up;

        // Token: 0x04004200 RID: 16896
        [Header("Impact & Lifetime Properties")]
        public GameObject impactEffect;

        // Token: 0x04004201 RID: 16897
        [Tooltip("This sound will not play over the network. Use lifetimeExpiredSound instead.")]
        [ShowFieldObsolete]
        [Obsolete("This sound will not play over the network. Use lifetimeExpiredSound instead.", false)]
        public string lifetimeExpiredSoundString;

        // Token: 0x04004202 RID: 16898
        public NetworkSoundEventDef lifetimeExpiredSound;

        // Token: 0x04004203 RID: 16899
        public float offsetForLifetimeExpiredSound;

        // Token: 0x04004204 RID: 16900
        public bool destroyOnEnemy = true;

        // Token: 0x04004205 RID: 16901
        public bool destroyOnWorld;

        // Token: 0x04004206 RID: 16902
        public bool impactOnWorld = true;

        // Token: 0x04004207 RID: 16903
        public bool timerAfterImpact;

        // Token: 0x04004208 RID: 16904
        public float lifetime;

        // Token: 0x04004209 RID: 16905
        public float lifetimeAfterImpact;

        // Token: 0x0400420A RID: 16906
        public float lifetimeRandomOffset;

        // Token: 0x0400420B RID: 16907
        private float stopwatch;

        // Token: 0x0400420C RID: 16908
        private float stopwatchAfterImpact;

        // Token: 0x0400420D RID: 16909
        private bool hasImpact;

        // Token: 0x0400420E RID: 16910
        private bool hasPlayedLifetimeExpiredSound;

        // Token: 0x0400420F RID: 16911
        public ProjectileImpactExplosion.TransformSpace transformSpace;

        // Token: 0x02000BA0 RID: 2976
        public enum TransformSpace
        {
            // Token: 0x04004211 RID: 16913
            World,
            // Token: 0x04004212 RID: 16914
            Local,
            // Token: 0x04004213 RID: 16915
            Normal
        }

        public bool isWorldCollided = false;

        public float onWorldCollisionBlastRadius;

        public float onEnemyCollisionBlastRadius;
    }
}

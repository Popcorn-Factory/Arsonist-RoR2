using ArsonistMod.Modules;
using ArsonistMod.Modules.Networking;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ArsonistMod.SkillStates.Arsonist.Secondary
{
    internal class FlareEffectControllerStrong : MonoBehaviour
    {
        public CharacterBody arsonistBody;
        public CharacterBody charbody;
        public GameObject effectObj;
        private int timesFired;
        private float timer;
        private bool hasFired;
        public uint burningSound;

        void Start()
        {
            timer = 0f;
            timesFired = 0;
            charbody = gameObject.GetComponent<CharacterBody>();

            //effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareAttached"), charbody.corePosition + randVec, Quaternion.LookRotation(charbody.characterDirection.forward));
            effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.arsonistFlareAttached, charbody.corePosition, Quaternion.LookRotation(Vector3.forward));


            //EffectManager.SpawnEffect(effectObj, new EffectData
            //{
            //    origin = charbody.corePosition + randVec,
            //    scale = 1f,
            //    rotation = Quaternion.LookRotation(charbody.characterDirection.forward),

            //}, true);


            effectObj.transform.parent = charbody.gameObject.transform;

            Util.PlaySound("Arsonist_Secondary_Flare_Projectile_Impact", gameObject);
            burningSound = Util.PlaySound("Arsonist_Secondary_Flare_Projectile_Burning", gameObject);
        }

        void FixedUpdate()
        {
            if (charbody) 
            {
                if (NetworkServer.active)
                {
                    if (timer > Modules.StaticValues.flareInterval)
                    {
                        if (timesFired < Modules.StaticValues.flareTickNum)
                        {
                            timesFired++;
                            timer = 0;
                            new TakeDamageNetworkRequest(charbody.masterObjectId, arsonistBody.masterObjectId, arsonistBody.damage * Modules.StaticValues.flareStrongDamageCoefficient / StaticValues.flareTickNum, true).Send(NetworkDestination.Clients);


                            new PlaySoundNetworkRequest(charbody.netId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);
                        }
                        else
                        {
                            hasFired = true;
                            FireExplosion();
                            EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                            {
                                origin = charbody.transform.position,
                                scale = StaticValues.flareBlastRadius,
                                rotation = new Quaternion(0, 0, 0, 0)
                            }, true);
                            Destroy(this);
                            Destroy(effectObj);
                        }
                    }
                    else
                    {
                        timer += Time.fixedDeltaTime;
                    }

                }
                else
                {
                    //Keep track of timing and despawn whenever it runs out.
                    if (timer > Modules.StaticValues.flareInterval * Modules.StaticValues.flareTickNum)
                    {
                        timer += Time.fixedDeltaTime;
                    }
                    else 
                    {
                        EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                        {
                            origin = charbody.transform.position,
                            scale = StaticValues.flareBlastRadius,
                            rotation = new Quaternion(0, 0, 0, 0)
                        }, true);
                        Destroy(this);
                        Destroy(effectObj);
                    }
                }
            }
            else
            {
                Destroy(effectObj);
            }

        }

        
        private void FireExplosion()
        {
            BlastAttack blastAttack;
            blastAttack = new BlastAttack();
            blastAttack.radius = StaticValues.flareBlastRadius;
            blastAttack.procCoefficient = 1f;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.position = charbody.transform.position;
            if (!arsonistBody)
            {
                blastAttack.attacker = null;
            }
            else 
            {
                blastAttack.attacker = arsonistBody.gameObject;
                blastAttack.crit = arsonistBody.RollCrit();
            }
            
            blastAttack.baseDamage = this.arsonistBody.damage * Modules.StaticValues.flareStrongDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.IgniteOnHit;
            blastAttack.Fire();

            //Play Sound
            AkSoundEngine.PostEvent("Arsonist_Secondary_Flare_Explosion_New", gameObject);


            FireSalvos();
        }

        private void FireSalvos() 
        {
            //On hit, we want to spawn x amount more bombs up to 10 based on attackspeed
            
            if (NetworkServer.active)
            {
                int childAmount = Modules.Config.flareSalvoAmount.Value;

                Vector3 pointOfImpact = charbody.transform.position;
                float increment = (Mathf.PI * 2f) / (float)childAmount;
                float currentInc = 0f;

                //Determine the angle at which it should shoot each child projectile.
                for (int i = 0; i < childAmount; i++)
                {
                    //we want to aim for a radius around the bomb.
                    // x = rcos(theta)
                    // y = rsin(theta)
                    float x = Modules.StaticValues.flareSalvoRadius * Mathf.Cos(currentInc);
                    float z = Modules.StaticValues.flareSalvoRadius * Mathf.Sin(currentInc);
                    float y = Modules.StaticValues.flareSalvoRadius;

                    Vector3 dir = new Vector3(x, y, z).normalized;
                    FireProjectileSalvo(dir, new Vector3(pointOfImpact.x + x / 9.0f, pointOfImpact.y, pointOfImpact.z + z / 9.0f));
                    currentInc += increment;
                }
            }
        }

        public void FireProjectileSalvo(Vector3 direction, Vector3 origin)
        {
            ProjectileManager.instance.FireProjectile(Modules.Projectiles.flareChildPrefab,
                new Vector3(origin.x, origin.y + 2f, origin.z),
                Util.QuaternionSafeLookRotation(direction),
                arsonistBody.gameObject,
                Modules.StaticValues.flareStrongChildDamageCoefficient * arsonistBody.damage,
                UnityEngine.Random.Range(1000f, 4000f),
                arsonistBody.RollCrit(),
                DamageColorIndex.Default,
                null,
                UnityEngine.Random.Range(15f, 25f));
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(burningSound);
            if (!hasFired && charbody.hasEffectiveAuthority) { FireExplosion(); }
            Destroy(effectObj);
        }
    }
}
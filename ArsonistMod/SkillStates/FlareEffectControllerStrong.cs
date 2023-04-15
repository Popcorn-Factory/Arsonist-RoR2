using ArsonistMod.Modules;
using ArsonistMod.Modules.Networking;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using UnityEngine;

namespace ArsonistMod.SkillStates.Arsonist.Secondary
{
    internal class FlareEffectControllerStrong : MonoBehaviour
    {
        public CharacterBody arsonistBody;
        public CharacterBody charbody;
        public GameObject effectObj;
        private int timesFired;
        private float timer;

        void Start()
        {
            timer = 0f;
            timesFired = 0;
            charbody = gameObject.GetComponent<CharacterBody>();

            //effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("flareAttached"), charbody.corePosition + randVec, Quaternion.LookRotation(charbody.characterDirection.forward));
            effectObj = UnityEngine.Object.Instantiate<GameObject>(Assets.arsonistFlareAttached, charbody.corePosition, Quaternion.LookRotation(Vector3.forward));


            //EffectManager.SpawnEffect(effectObj, new EffectData
            //{
            //    origin = charbody.corePosition + randVec,
            //    scale = 1f,
            //    rotation = Quaternion.LookRotation(charbody.characterDirection.forward),

            //}, true);


            effectObj.transform.parent = charbody.gameObject.transform;

        }

        void FixedUpdate()
        {
            if (charbody.hasEffectiveAuthority)
            {

                if (timer > Modules.StaticValues.flareInterval)
                {
                    if (timesFired < Modules.StaticValues.flareTickNum)
                    {
                        timesFired++;
                        timer = 0;
                        new TakeDamageNetworkRequest(charbody.masterObjectId, arsonistBody.masterObjectId, arsonistBody.damage * Modules.StaticValues.flareStrongDamageCoefficient / StaticValues.flareTickNum).Send(NetworkDestination.Clients);


                        new PlaySoundNetworkRequest(charbody.masterObjectId, 3747272580).Send(R2API.Networking.NetworkDestination.Clients);
                    }
                    else
                    {
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
                    timer += Time.deltaTime;
                }

            }
            else if (!charbody)
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
            blastAttack.attacker = arsonistBody.gameObject;
            blastAttack.crit = arsonistBody.RollCrit();
            
            blastAttack.baseDamage = this.arsonistBody.damage * Modules.StaticValues.flareStrongDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.Generic;
            blastAttack.AddModdedDamageType(Modules.Damage.arsonistChildExplosionDamageType);
            blastAttack.Fire();

            //Play Sound
            new PlaySoundNetworkRequest(charbody.netId, 3061346618).Send(NetworkDestination.Clients);
        }
        private void OnDestroy()
        {
            Destroy(effectObj);
        }
    }
}
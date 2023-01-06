using ArsonistMod.Modules;
using R2API.Networking;
using RoR2;
using UnityEngine;

namespace ArsonistMod.SkillStates.Arsonist.Secondary
{
    internal class FlareEffectController : MonoBehaviour
    {
        public CharacterBody arsonistBody;
        public CharacterBody charbody;
        public bool isWeak;
        private int timesFired;
        public float timer;

        void Start()
        {
            Debug.Log("start");
            timer = 0f;
            isWeak = false;
            charbody = this.gameObject.GetComponent<CharacterBody>();
        }

        void FixedUpdate()
        {
            if (charbody.hasEffectiveAuthority)
            {
                timer += Time.deltaTime;

                if (timer > 1f && !isWeak)
                {
                    if (timesFired == 5)
                    {
                        FireExplosion(false);
                        EffectManager.SpawnEffect(Modules.Assets.explosionPrefab, new EffectData
                        {
                            origin = charbody.transform.position,
                            scale = StaticValues.flareBlastRadius,
                            rotation = new Quaternion(0, 0, 0, 0)
                        }, true);
                        Destroy(this);
                    }
                    else
                    {
                        timesFired++;
                        FireFlareTick(false);
                    }
                    timer = 0;
                }

                if (timer > 1f && isWeak)
                {
                    if (timesFired == 5)
                    {
                        FireExplosion(true);
                        EffectManager.SpawnEffect(Modules.Assets.explosionPrefab, new EffectData
                        {
                            origin = charbody.transform.position,
                            scale = StaticValues.flareBlastRadius,
                            rotation = new Quaternion(0, 0, 0, 0)
                        }, true);
                        Destroy(this);
                    }
                    else
                    {
                        timesFired++;
                        FireFlareTick(true);
                    }
                    timer = 0;
                }
            }
           
        }

        private void FireFlareTick(bool isWeak)
        {
            BlastAttack blastAttack;
            blastAttack = new BlastAttack();
            blastAttack.radius = 1f;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.procCoefficient = 0f;
            blastAttack.position = charbody.transform.position;
            blastAttack.attacker = arsonistBody.gameObject;
            blastAttack.crit = Util.CheckRoll(0);
            if (isWeak)
            {
                blastAttack.baseDamage = this.arsonistBody.damage * Modules.StaticValues.flareWeakDamageCoefficient/5f;
            }
            else blastAttack.baseDamage = this.arsonistBody.damage * Modules.StaticValues.flareStrongDamageCoefficient/5f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.Generic;

            blastAttack.Fire();
        }        
        
        private void FireExplosion(bool isWeak)
        {
            BlastAttack blastAttack;
            blastAttack = new BlastAttack();
            blastAttack.radius = StaticValues.flareBlastRadius;
            blastAttack.procCoefficient = 0f;
            blastAttack.teamIndex = TeamIndex.Player;
            blastAttack.position = charbody.transform.position;
            blastAttack.attacker = arsonistBody.gameObject;
            blastAttack.crit = Util.CheckRoll(0);
            if(isWeak)
            {
                blastAttack.baseDamage = this.arsonistBody.baseDamage * Modules.StaticValues.flareWeakDamageCoefficient;
            }
            else blastAttack.baseDamage = this.arsonistBody.baseDamage * Modules.StaticValues.flareStrongDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.Generic;

            blastAttack.Fire();
        }

        public void SetWeakBool()
        {
            isWeak = true;
        }
    }
}
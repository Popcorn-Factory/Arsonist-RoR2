using ArsonistMod.Modules;
using R2API.Networking;
using RoR2;
using UnityEngine;

namespace ArsonistMod.SkillStates.Arsonist.Secondary
{
    internal class FlareEffectController : MonoBehaviour
    {
        public CharacterBody charbody;
        private GameObject effectObj;
        public float timer;

        void Start()
        {
            timer = 0f;
            charbody = this.gameObject.GetComponent<CharacterBody>();
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;

            if (timer > 1f && charbody.HasBuff(Modules.Buffs.flareStrongBuff))
            {
                if (charbody.GetBuffCount(Modules.Buffs.flareStrongBuff) == 1)
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
                    charbody.ApplyBuff(Modules.Buffs.flareStrongBuff.buffIndex, charbody.GetBuffCount(Modules.Buffs.flareStrongBuff) - 1);
                    FireFlareTick(false);
                }
                timer = 0;
            }
            if (timer > 1f && charbody.HasBuff(Modules.Buffs.FlareWeakBuff))
            {
                if (charbody.GetBuffCount(Modules.Buffs.FlareWeakBuff) == 1)
                {
                    FireExplosion(true);
                    EffectManager.SpawnEffect(Modules.Assets.explosionPrefab, new EffectData
                    {
                        origin = charbody.transform.position,
                        scale = StaticValues.flareBlastRadius,
                        rotation = new Quaternion(0,0,0,0)
                    }, true);
                    Destroy(this);
                }
                else
                {
                    charbody.ApplyBuff(Modules.Buffs.FlareWeakBuff.buffIndex, charbody.GetBuffCount(Modules.Buffs.FlareWeakBuff) - 1);
                    FireFlareTick(true);
                }
                timer = 0;
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
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(0);
            if (isWeak)
            {
                blastAttack.baseDamage = this.charbody.baseDamage * Modules.StaticValues.flareWeakDamageCoefficient/5f;
            }
            else blastAttack.baseDamage = this.charbody.baseDamage * Modules.StaticValues.flareStrongDamageCoefficient/5f;
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
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(0);
            if(isWeak)
            {
                blastAttack.baseDamage = this.charbody.baseDamage * Modules.StaticValues.flareWeakDamageCoefficient;
            }
            else blastAttack.baseDamage = this.charbody.baseDamage * Modules.StaticValues.flareStrongDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.Generic;

            blastAttack.Fire();
        }
    }
}
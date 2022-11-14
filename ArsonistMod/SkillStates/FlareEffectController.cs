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
                FireFlareTick(false);
            }            
            if (timer > 1f && charbody.HasBuff(Modules.Buffs.FlareWeakBuff))
            {
                FireFlareTick(true);
            }
        }

        private void FireFlareTick(bool isWeak)
        {
            BlastAttack blastAttack;
            blastAttack = new BlastAttack();
            blastAttack.radius = 0.1f;
            blastAttack.procCoefficient = 0f;
            blastAttack.position = charbody.transform.position;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(0);
            blastAttack.baseDamage = this.charbody.baseDamage * 1f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1f;
            blastAttack.damageType = DamageType.Generic;

            blastAttack.Fire();
        }
    }
}
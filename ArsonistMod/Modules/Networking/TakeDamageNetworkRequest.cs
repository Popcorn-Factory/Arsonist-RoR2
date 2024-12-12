using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    internal class TakeDamageNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        NetworkInstanceId attackernetID;
        float health;
        bool burn;
        bool selfMasoDmg;
        bool crit;

        //Don't network these.
        GameObject bodyObj;
        GameObject attackerbodyObj;

        public TakeDamageNetworkRequest()
        {

        }

        public TakeDamageNetworkRequest(NetworkInstanceId netID, NetworkInstanceId attackernetID, float health, bool burn)
        {
            this.netID = netID;
            this.attackernetID = attackernetID;
            this.health = health;
            this.burn = burn;
            this.selfMasoDmg = false;
            this.crit = true;
        }

        public TakeDamageNetworkRequest(NetworkInstanceId netID, NetworkInstanceId attackernetID, float health, bool burn, bool selfMasoDmg)
        {
            this.netID = netID;
            this.attackernetID = attackernetID;
            this.health = health;
            this.burn = burn;
            this.selfMasoDmg = selfMasoDmg;
            this.crit = true;
        }

        public TakeDamageNetworkRequest(NetworkInstanceId netID, NetworkInstanceId attackernetID, float health, bool burn, bool selfMasoDmg, bool crit)
        {
            this.netID = netID;
            this.attackernetID = attackernetID;
            this.health = health;
            this.burn = burn;
            this.selfMasoDmg = selfMasoDmg;
            this.crit = crit;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            attackernetID = reader.ReadNetworkId();
            health = reader.ReadSingle();
            burn = reader.ReadBoolean();
            selfMasoDmg = reader.ReadBoolean();
            crit = reader.ReadBoolean();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(attackernetID);
            writer.Write(health);
            writer.Write(burn);
            writer.Write(selfMasoDmg);
            writer.Write(crit);
        }

        public void OnReceived()
        {

            GameObject masterobject = Util.FindNetworkObject(netID);
            if (!masterobject) 
            {
                //No target to hit.
                return; 
            }
            CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
            CharacterBody charBody = charMaster.GetBody();
            bodyObj = charBody.gameObject;

            GameObject attackermasterobject = Util.FindNetworkObject(attackernetID);
            CharacterMaster attackercharMaster = attackermasterobject.GetComponent<CharacterMaster>();
            CharacterBody attackercharBody = attackercharMaster.GetBody();
            attackerbodyObj = attackercharBody.gameObject;

            DamageTypeCombo genericDamage = new DamageTypeCombo(DamageType.Generic, DamageTypeExtended.Generic, DamageSource.NoneSpecified);
            DamageTypeCombo igniteDamage = new DamageTypeCombo(DamageType.IgniteOnHit, DamageTypeExtended.Generic, DamageSource.NoneSpecified);
            DamageTypeCombo masoDamage = new DamageTypeCombo(DamageType.Generic | DamageType.AOE | DamageType.DoT, DamageTypeExtended.Generic, DamageSource.NoneSpecified);

            //deal health damage
            if (NetworkServer.active && charBody.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = health;
                damageInfo.position = charBody.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.WeakPoint;
                damageInfo.crit = crit ? attackercharBody.RollCrit() : false;
                damageInfo.attacker = attackercharBody ? attackercharBody.gameObject : null;
                damageInfo.inflictor = null;
                damageInfo.damageType = genericDamage;
                damageInfo.procCoefficient = 0.2f;
                damageInfo.procChainMask = default(ProcChainMask);

                if (burn) 
                {
                    damageInfo.damageType = igniteDamage;
                }
                if (selfMasoDmg) 
                {
                    damageInfo.damageType = masoDamage;
                }
                charBody.healthComponent.TakeDamage(damageInfo);
            }
        }
    }
}

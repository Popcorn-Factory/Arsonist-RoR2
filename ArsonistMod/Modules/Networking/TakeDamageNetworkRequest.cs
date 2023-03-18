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

        //Don't network these.
        GameObject bodyObj;
        GameObject attackerbodyObj;

        public TakeDamageNetworkRequest()
        {

        }

        public TakeDamageNetworkRequest(NetworkInstanceId netID, NetworkInstanceId attackernetID, float health)
        {
            this.netID = netID;
            this.attackernetID= attackernetID;
            this.health = health;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            attackernetID = reader.ReadNetworkId();
            health = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(attackernetID);
            writer.Write(health);
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



            //deal health damage
            if (NetworkServer.active && charBody.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = health;
                damageInfo.position = charBody.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.WeakPoint;
                damageInfo.crit = attackercharBody.RollCrit();
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = DamageType.Generic;
                damageInfo.procCoefficient = 0.2f;
                damageInfo.procChainMask = default(ProcChainMask);
                charBody.healthComponent.TakeDamage(damageInfo);
            }

            

        }
    }
}

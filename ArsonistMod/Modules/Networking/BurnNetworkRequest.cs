using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    internal class BurnNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId charnetID;
        NetworkInstanceId enemynetID;

        //Don't network these.
        GameObject bodyObj;
        GameObject enemybodyObj;

        public BurnNetworkRequest()
        {

        }

        public BurnNetworkRequest(NetworkInstanceId charnetID, NetworkInstanceId enemynetID)
        {
            this.charnetID = charnetID;
            this.enemynetID = enemynetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
            enemynetID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
            writer.Write(enemynetID);
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                if (NetworkServer.active)
                {
                    GameObject masterobject = Util.FindNetworkObject(charnetID);
                    CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                    CharacterBody charBody = charMaster.GetBody();
                    bodyObj = charBody.gameObject;

                    GameObject enemymasterobject = Util.FindNetworkObject(enemynetID);
                    CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                    CharacterBody enemycharBody = enemycharMaster.GetBody();
                    enemybodyObj = enemycharBody.gameObject;

                    InflictDotInfo info = new InflictDotInfo();
                    info.damageMultiplier = charBody.damage * Modules.StaticValues.cleanseDamageCoefficient;
                    info.attackerObject = bodyObj;
                    info.victimObject = enemybodyObj;
                    info.duration = Modules.StaticValues.cleanseDuration;
                    info.dotIndex = DotController.DotIndex.Burn;

                    DotController.InflictDot(ref info);
                }

            }
            

        }
    }
}

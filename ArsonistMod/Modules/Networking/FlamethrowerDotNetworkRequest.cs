using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    internal class FlamethrowerDotNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId charnetID;
        NetworkInstanceId enemynetID;
        float duration;

        //Don't network these.
        GameObject bodyObj;
        GameObject enemybodyObj;

        public FlamethrowerDotNetworkRequest()
        {

        }

        public FlamethrowerDotNetworkRequest(NetworkInstanceId charnetID, NetworkInstanceId enemynetID, float duration)
        {
            this.charnetID = charnetID;
            this.enemynetID = enemynetID;
            this.duration = duration;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
            enemynetID = reader.ReadNetworkId();
            duration = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
            writer.Write(enemynetID);
            writer.Write(duration);
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(charnetID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                GameObject enemymasterobject = Util.FindNetworkObject(enemynetID);
                if (!enemymasterobject) 
                {
                    //Nothing to burn
                    return;
                }
                CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                CharacterBody enemycharBody = enemycharMaster.GetBody();
                enemybodyObj = enemycharBody.gameObject;

                InflictDotInfo info = new InflictDotInfo();
                info.damageMultiplier = 1f;
                info.attackerObject = bodyObj;
                info.victimObject = enemybodyObj;
                info.duration = duration;
                info.dotIndex = DotController.DotIndex.Burn;

                RoR2.StrengthenBurnUtils.CheckDotForUpgrade(charMaster.inventory, ref info);

                DotController.InflictDot(ref info);
            }
        }
    }
}

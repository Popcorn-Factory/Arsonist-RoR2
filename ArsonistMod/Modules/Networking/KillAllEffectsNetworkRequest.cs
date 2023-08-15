using ArsonistMod.Content.Controllers;
using ArsonistMod.SkillStates;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    internal class KillAllEffectsNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId charnetID;

        public KillAllEffectsNetworkRequest()
        {

        }

        public KillAllEffectsNetworkRequest(NetworkInstanceId charnetID)
        {
            this.charnetID = charnetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
        }

        public void OnReceived()
        {
            GameObject bodyObj = Util.FindNetworkObject(charnetID);
            CharacterBody body = bodyObj.GetComponent<CharacterBody>();
            if (body) 
            {
                ArsonistController controller = body.GetComponent<ArsonistController>();
                if (controller) 
                {
                    controller.StopAllParticleEffects();
                }
            }
        }
    }
}

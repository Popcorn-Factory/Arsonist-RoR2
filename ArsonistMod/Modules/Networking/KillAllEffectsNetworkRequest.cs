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
        bool killMaso;

        public KillAllEffectsNetworkRequest()
        {

        }

        public KillAllEffectsNetworkRequest(NetworkInstanceId charnetID, bool killMaso)
        {
            this.charnetID = charnetID;
            this.killMaso = killMaso;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
            killMaso = reader.ReadBoolean();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
            writer.Write(killMaso);
        }

        public void OnReceived()
        {
            GameObject bodyObj = Util.FindNetworkObject(charnetID);
            if (!bodyObj) 
            {
                return; //null errors bruh
            }

            CharacterBody body = bodyObj.GetComponent<CharacterBody>();
            if (body) 
            {
                ArsonistController controller = body.GetComponent<ArsonistController>();
                if (controller) 
                {
                    controller.StopAllParticleEffects();
                }

                MasochismController masoCon = body.GetComponent<MasochismController>();
                if (masoCon && body.hasEffectiveAuthority && killMaso) 
                {
                    if (masoCon.masochismActive) 
                    {
                        masoCon.TriggerMasochismAndEXOverheat(false);
                    }
                }
            }
        }
    }
}

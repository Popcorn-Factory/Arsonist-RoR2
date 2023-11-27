using ArsonistMod.Content.Controllers;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    internal class PlayCleanseBlastNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId charnetID;
        bool strong;

        public PlayCleanseBlastNetworkRequest()
        {

        }

        public PlayCleanseBlastNetworkRequest(NetworkInstanceId charnetID, bool strong)
        {
            this.charnetID = charnetID;
            this.strong = strong;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
            strong = reader.ReadBoolean();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
            writer.Write(strong);
        }

        public void OnReceived()
        {
            GameObject bodyObj = Util.FindNetworkObject(charnetID);
            if (!bodyObj) 
            {
                return;
            }

            CharacterBody body = bodyObj.GetComponent<CharacterBody>();
            if (body) 
            {
                ArsonistController arsonistController = body.GetComponent<ArsonistController>();
                // Let off steam, play the blast effect, play trail effect, play ring effect
                // ring and trail will shut off after the buff expires.
                if (arsonistController) 
                {
                    if (strong)
                    {
                        arsonistController.cleanseBlast.Play();
                        arsonistController.steamParticle.Play();
                        arsonistController.ringFireActive = true;
                        arsonistController.trailFire.Play();
                        arsonistController.ringFire.Play();
                    }
                    else 
                    {
                        //We only play steam because the move quickly removes heat from overheat
                        arsonistController.steamParticle.Play();
                    }
                }
            }
        }
    }
}

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
    internal class ToggleMasochismEffectNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId charnetID;
        bool toggleOn;

        public ToggleMasochismEffectNetworkRequest()
        {

        }

        public ToggleMasochismEffectNetworkRequest(NetworkInstanceId charnetID, bool toggleOn)
        {
            this.charnetID = charnetID;
            this.toggleOn = toggleOn;
        }

        public void Deserialize(NetworkReader reader)
        {
            charnetID = reader.ReadNetworkId();
            toggleOn = reader.ReadBoolean();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(charnetID);
            writer.Write(toggleOn);
        }

        public void OnReceived()
        {
            GameObject bodyObj = Util.FindNetworkObject(charnetID);
            CharacterBody body = bodyObj.GetComponent<CharacterBody>();
            if (body) 
            {
                if (!body.hasEffectiveAuthority) 
                {
                    MasochismController masocon = body.GetComponent<MasochismController>();
                    if (masocon) 
                    {
                        masocon.ToggleMasochismRangeIndicator(toggleOn);
                    }
                }
            }
        }
    }
}

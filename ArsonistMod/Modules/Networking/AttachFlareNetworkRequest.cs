using ArsonistMod.SkillStates.Arsonist.Secondary;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace ArsonistMod.Modules.Networking
{
    public class AttachFlareNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId targetCharNetID;
        NetworkInstanceId arsonistCharNetID;

        //Don't network these.
        GameObject bodyObj;

        public enum FlareType : uint 
        {
            STRONG = 1,
            WEAK = 2,
            CHILD_STRONG = 3
        }

        FlareType flareType;

        public AttachFlareNetworkRequest()
        {

        }

        public AttachFlareNetworkRequest(NetworkInstanceId targetCharNetID, NetworkInstanceId arsonistCharNetID, FlareType flareType)
        {
            this.targetCharNetID = targetCharNetID;
            this.arsonistCharNetID = arsonistCharNetID;
            this.flareType = flareType;
        }

        public void Deserialize(NetworkReader reader)
        {
            targetCharNetID = reader.ReadNetworkId();
            arsonistCharNetID = reader.ReadNetworkId();
            flareType = (FlareType)reader.ReadUInt32();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(targetCharNetID);
            writer.Write(arsonistCharNetID);
            writer.Write((uint)flareType);
        }

        public void OnReceived()
        {
            GameObject bodyObj = Util.FindNetworkObject(targetCharNetID);
            GameObject arsonistBodyObj = Util.FindNetworkObject(arsonistCharNetID);
            if (bodyObj && arsonistBodyObj) 
            {
                if (arsonistBodyObj.GetComponent<CharacterBody>().hasEffectiveAuthority) 
                {
                    switch (flareType)
                    {
                        case FlareType.STRONG:
                            FlareEffectControllerStrong strong = bodyObj.AddComponent<FlareEffectControllerStrong>();
                            strong.arsonistBody = arsonistBodyObj.GetComponent<CharacterBody>();
                            strong.charbody = bodyObj.GetComponent<CharacterBody>();
                            break;
                        case FlareType.CHILD_STRONG:
                            FlareEffectControllerStrongChild child = bodyObj.AddComponent<FlareEffectControllerStrongChild>();
                            child.arsonistBody = arsonistBodyObj.GetComponent<CharacterBody>();
                            child.charbody = bodyObj.GetComponent<CharacterBody>();
                            break;
                        case FlareType.WEAK:
                            FlareEffectControllerWeak weak = bodyObj.AddComponent<FlareEffectControllerWeak>();
                            weak.arsonistBody = arsonistBodyObj?.GetComponent<CharacterBody>();
                            weak.charbody = bodyObj?.GetComponent<CharacterBody>();
                            break;
                        default:
                            FlareEffectControllerWeak weak2 = bodyObj.AddComponent<FlareEffectControllerWeak>();
                            weak2.arsonistBody = arsonistBodyObj?.GetComponent<CharacterBody>();
                            weak2.charbody = bodyObj?.GetComponent<CharacterBody>();
                            //Default to weak.
                            Debug.Log("If you're reading this, this means that the flare network request has failed, Contact Ethanol 10 on the RoR2 modding discord.");
                            break;
                    }
                }
            }
        }
    }
}

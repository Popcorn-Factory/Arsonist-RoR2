using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    internal class ScepterBurningSoundController : MonoBehaviour
    {
        //All this controller does is play the burning sound for however long they are burning if they are hit by the scepter flamethrower.

        public uint burningSFXID;
        public CharacterBody characterBody;

        public void Start() 
        {
            characterBody = GetComponent<CharacterBody>();

            if (!characterBody) 
            {
                //Immediately destroy. Can't monitor when this will finish burning.
                Destroy(this);
            }

            burningSFXID = AkSoundEngine.PostEvent(1864761202, this.gameObject);
        }

        public void Update() 
        {
            if (!(characterBody.HasBuff(RoR2Content.Buffs.OnFire) || characterBody.HasBuff(DLC1Content.Buffs.StrongerBurn)) )
            {
                Destroy(this);
            }
        }

        public void OnDestroy() 
        {
            AkSoundEngine.StopPlayingID(burningSFXID);
        }
    }
}

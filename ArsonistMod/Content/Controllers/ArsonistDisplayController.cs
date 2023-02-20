using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    internal class ArsonistDisplayController : MonoBehaviour
    {
        internal float stopwatch;
        internal float duration;
        internal static float igniteFraction = 0.38f;
        internal static float sizzlingFraction = 0.40f;
        internal bool ignited;
        internal bool sizzling;

        void Start() 
        {
            stopwatch = 0f;
            ignited = false;
            sizzling = false;
            duration = 1.7f;
        }

        void Update()
        {
            stopwatch += Time.deltaTime;
            //Update certain parameters here, the ignite animation is playing at the start so time effects and such 
            //To this animation at 1x speed.
            if(stopwatch >= duration * igniteFraction && !ignited) 
            {
                Util.PlaySound("Arsonist_Menu_Match_Strike", this.gameObject);
                ignited = true;
            }

            if (stopwatch >= duration * sizzlingFraction && !sizzling) 
            {
                Util.PlaySound("Arsonist_Menu_Match_Sizzle", this.gameObject);
                sizzling = true;
            }
        }
    }
}

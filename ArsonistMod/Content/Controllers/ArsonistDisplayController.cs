using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    internal class ArsonistDisplayController : MonoBehaviour
    {
        internal float stopwatch;
        internal static float igniteFraction;

        void Start() 
        {
            stopwatch = 0f;
        }

        void Update()
        {
            //Update certain parameters here, the ignite animation is playing at the start so time effects and such 
            //To this animation at 1x speed.


        }
    }
}

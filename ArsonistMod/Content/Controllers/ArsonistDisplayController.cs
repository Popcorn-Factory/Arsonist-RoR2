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
        internal static float flameParticleTrigger = 0.55f;
        internal bool ignited;
        internal bool sizzling;
        public ParticleSystem fireParticle; 
        public ParticleSystem sparkParticle;

        void Start() 
        {
            stopwatch = 0f;
            ignited = false;
            sizzling = false;
            duration = 1.7f;
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if (childLocator != null)
            {
                fireParticle = childLocator.FindChild("FireParticle").gameObject.GetComponent<ParticleSystem>();
                sparkParticle = childLocator.FindChild("SparkParticle").gameObject.GetComponent<ParticleSystem>();
            }
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
                // Play particle effects
                fireParticle.Play();
                sparkParticle.Play();
            }
        }
    }
}

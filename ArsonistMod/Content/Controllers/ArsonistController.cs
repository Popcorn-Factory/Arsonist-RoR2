using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistController : MonoBehaviour
    {
        Animator anim;

        public ParticleSystem steamParticle;
        public ParticleSystem fireParticle;

        // Use this for initialization
        void Start()
        {
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if(childLocator != null)
            {
                steamParticle = childLocator.FindChild("SteamGroup").GetComponent<ParticleSystem>();
                fireParticle = childLocator.FindChild("FireParticle").GetComponent<ParticleSystem>();
                fireParticle.Stop();
            }
            //What's your existance for.
            //To update an overlay.
            //Oh god.
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
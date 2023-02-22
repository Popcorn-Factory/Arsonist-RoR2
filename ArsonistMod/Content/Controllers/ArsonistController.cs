using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistController : MonoBehaviour
    {
        Animator anim;

        public ParticleSystem steamParticle;

        // Use this for initialization
        void Start()
        {
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if(childLocator != null)
            {
                steamParticle = childLocator.FindChild("SteamGroup").GetComponent<ParticleSystem>();
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
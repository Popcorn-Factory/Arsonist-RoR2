using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class MenuController : MonoBehaviour
    {
        Animator anim;

        private float timer;
        private bool hasPlayed;

        public ParticleSystem fireParticle;
        public ParticleSystem sparkParticle;

        // Use this for initialization
        void Start()
        {
            hasPlayed = false;
            timer = 0;
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if(childLocator != null)
            {
                fireParticle = childLocator.FindChild("FireParticle").gameObject.GetComponent<ParticleSystem>();
                sparkParticle = childLocator.FindChild("SparkParticle").gameObject.GetComponent<ParticleSystem>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer > 0.55f && hasPlayed == false)
            {
                hasPlayed = true;
                fireParticle.Play();
                sparkParticle.Play();
            }
        }
    }
}
using ArsonistMod.SkillStates.EmoteStates;
using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistController : MonoBehaviour
    {
        Animator anim;
        CharacterBody charBody;
        EntityStateMachine[] entityStateMachines;
        EntityStateMachine weaponStateMachine;
        EntityStateMachine bodyStateMachine;

        public ParticleSystem steamParticle;
        public ParticleSystem steamDownParticle;

        // Use this for initialization
        void Start()
        {
            ChildLocator childLocator = GetComponentInChildren<ChildLocator>();
            if(childLocator != null)
            {
                steamParticle = childLocator.FindChild("SteamGroup").GetComponent<ParticleSystem>();
                steamDownParticle = childLocator.FindChild("SteamFireDown").GetComponent<ParticleSystem>()
            }
            //What's your existance for.
            //To update an overlay.
            //Oh god.

            charBody = gameObject.GetComponent<CharacterBody>();
            entityStateMachines = gameObject.GetComponents<EntityStateMachine>();
            foreach (EntityStateMachine entityStateMachine in entityStateMachines) 
            {
                if (entityStateMachine.customName == "Weapon") 
                {
                    weaponStateMachine = entityStateMachine;
                }
                if (entityStateMachine.customName == "Body") 
                {
                    bodyStateMachine = entityStateMachine;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Check here for authority and also executing emote states
            if (charBody.hasEffectiveAuthority)
            {
                //We can execute states now.

                //Check for input down.
                if (Modules.Config.emoteSitKey.Value.IsPressed())
                {
                    //Trigger the emote sit key
                    bodyStateMachine.SetNextState(new EmoteSit());
                }
                else if (Modules.Config.emoteStrutKey.Value.IsPressed())
                {
                    //Trigger the emote strut key
                    weaponStateMachine.SetNextState(new EmoteStrut());
                }
            }
        }
    }
}
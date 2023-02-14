using RoR2;
using System.Collections;
using UnityEngine;

namespace ArsonistMod.Content.Controllers
{
    public class ArsonistController : MonoBehaviour
    {
        Animator anim;
        CharacterBody charBody;

        // Use this for initialization
        void Start()
        {
            //What's your existance for.
            //To update an overlay.
            //Oh god.

            charBody = gameObject.GetComponent<CharacterBody>();
        }

        // Update is called once per frame
        void Update()
        {
            //Check here for authority and also executing emote states
            if (charBody.hasEffectiveAuthority)
            {
                //We can execute states now.

                //Check for input down.
                if (Modules.Config.emoteSitKey.Value.IsDown())
                {
                    //Trigger the emote sit key
                }
                else if (Modules.Config.emoteStrutKey.Value.IsDown())
                {
                    //Trigger the emote strut key
                    
                }
            }
        }
    }
}
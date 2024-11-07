using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    Animator anim;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("active", !anim.GetBool("active"));
            if(ps.isPlaying)
            {
                ps.Stop();
            }
            else{
                ps.Play();
            }
        }

        if(!ps.isPlaying && anim.GetBool("active")){
            ps.Play();
        }
    }
}

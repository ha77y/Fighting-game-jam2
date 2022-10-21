using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HorseshoeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clip;
    public Animator anim;

    void Start()
    {
        AnimationEvent evnt;
        evnt = new AnimationEvent();
        evnt.time = 0.1f;
        evnt.functionName = "Shoot";
        anim = gameObject.GetComponent(typeof(Animator)) as Animator;

        clip = anim.runtimeAnimatorController.animationClips[1];
        clip.AddEvent(evnt);
        evnt.time = 0.3f;
        clip.AddEvent(evnt);
        evnt.time = 0.5f;
        clip.AddEvent(evnt);
        evnt.time = 0.7f;
        clip.AddEvent(evnt);
        evnt.time = 0.9f;
        clip.AddEvent(evnt);
        evnt.time = 0.11f;
        clip.AddEvent(evnt);
    }

    // Update is called once per frame
    void Update()
    {

        if (this.transform.parent.GetComponent<Enemy>().isshooting)
        {
            anim.Play("HorseshoeShoot");
        }
        else
        {
            anim.Play("HorseshoeIdle");
        }

    }
    public void Shoot()
    {
        print("HorseshoeShoot!!");
    }
}

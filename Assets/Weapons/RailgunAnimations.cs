using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clip;
    public Animator anim;
    
    void Start()
    {
        AnimationEvent evnt;
        evnt = new AnimationEvent();

        evnt.time = 0.5f;
        evnt.functionName = "Shoot";
        anim = gameObject.GetComponent(typeof(Animator)) as Animator;

        clip = anim.runtimeAnimatorController.animationClips[0];
        clip = anim.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evnt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot()
    {
        print("Shoot!!");
    }
}

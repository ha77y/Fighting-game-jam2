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

        evnt.intParameter = 12345;
        evnt.time = 1.3f;
        evnt.functionName = "Shoot";
        anim = GetComponent<Animator>();

        //clip = anim.runtimeAnimatorController.animationClips[0];
        clip = anim.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evnt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(int i)
    {
        print("Shoot!!");
    }
}

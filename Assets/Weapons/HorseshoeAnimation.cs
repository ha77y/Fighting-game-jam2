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
    public Rigidbody2D bullet;

    void Start()
    {
        //anim = gameObject.GetComponent(typeof(Animator)) as Animator;
        this.transform.parent.GetComponent<Enemy>().ammo = 5;
        AnimationEvent evnt;

        //Shoot animation event
        evnt = new AnimationEvent();
        //clip.events[0] == evnt;
        evnt.time = 0.1f;
        evnt.functionName = "Shoot";
        anim = gameObject.GetComponent(typeof(Animator)) as Animator;
        clip = anim.runtimeAnimatorController.animationClips[2];
        if (!(clip.events[0] == evnt))
        {
            clip.AddEvent(evnt);
            evnt.time = 0.3f;
            clip.AddEvent(evnt);
            evnt.time = 0.5f;
            clip.AddEvent(evnt);
            evnt.time = 0.7f;
            clip.AddEvent(evnt);
            evnt.time = 0.9f;
            clip.AddEvent(evnt);
            evnt.time = 1.1f;
            clip.AddEvent(evnt);
        }
        

        //Reload animation event
        
        
        clip = anim.runtimeAnimatorController.animationClips[1];
        evnt.time = 1.0f;
        evnt.functionName = "Reload";
        if (!(clip.events[clip.events.Length] == evnt))
        {
            clip.AddEvent(evnt);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (this.transform.parent.GetComponent<Enemy>().isshooting)
        {
            anim.Play("HorseshoeShoot");
        }
        else if (this.transform.parent.GetComponent<Enemy>().isreloading)    
        {
            anim.Play("HorseshoeReload");
        } else
        {
            anim.Play("HorseshoeIdle");
        }

    }
    public void Shoot()
    {
        print("HorseshoeShoot!!");
        Rigidbody2D b = Instantiate(bullet, new Vector3(this.gameObject.transform.GetChild(0).position.x, this.gameObject.transform.GetChild(0).position.y, this.gameObject.transform.GetChild(0).position.z), Quaternion.identity);
        b.velocity = transform.right * -10.0f;
        if (b.velocity.x < 0)
        {
            b.transform.Rotate(0f, 0f, 90f);
        } else
        {
            b.transform.Rotate(0f, 0f, -90f);
        }
           
        if (this.transform.parent.GetComponent<Enemy>().ammo != 0) 
        { 
            this.transform.parent.GetComponent<Enemy>().ammo -= 1; 
        }
    }
    public void Reload()
    {
        print("HorseshoeReloaded!");
        this.transform.parent.GetComponent<Enemy>().ammo = 5;
        this.transform.parent.GetComponent<Enemy>().isreloading = false;
    }
}

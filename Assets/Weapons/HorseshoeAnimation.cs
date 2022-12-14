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
    public int damage = 5;
    static public Boolean horseshoeAddEvents = true;
    public AudioSource fire;

    void Start()
    {
        //anim = gameObject.GetComponent(typeof(Animator)) as Animator;
        this.transform.parent.parent.parent.GetComponent<Enemy>().ammo = 5;
     
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (this.transform.parent.parent.parent.GetComponent<Enemy>().isshooting)
        {
            anim.Play("HorseshoeShoot");
        }
        else if (this.transform.parent.parent.parent.GetComponent<Enemy>().isreloading)    
        {
            anim.Play("HorseshoeReload");
        } else
        {
            anim.Play("HorseshoeIdle");
        }

    }
    public void Shoot()
    {
        fire.Play();
        Rigidbody2D b = Instantiate(bullet, new Vector2(this.gameObject.transform.parent.GetChild(1).position.x, this.gameObject.transform.parent.GetChild(1).position.y), Quaternion.identity);
        b.velocity = transform.right * -10.0f;
        b.GetComponent<Bullet>().damage = damage;
        if (b.velocity.x < 0)
        {
            b.transform.Rotate(0f, 0f, 90f);
        } else
        {
            b.transform.Rotate(0f, 0f, -90f);
        }
           
        if (this.transform.parent.parent.parent.GetComponent<Enemy>().ammo != 0) 
        { 
            this.transform.parent.parent.parent.GetComponent<Enemy>().ammo -= 1;
        }
    }
    public void Reload()
    {
        this.transform.parent.parent.parent.GetComponent<Enemy>().ammo = 5;
        this.transform.parent.parent.parent.GetComponent<Enemy>().isreloading = false;
    }
}

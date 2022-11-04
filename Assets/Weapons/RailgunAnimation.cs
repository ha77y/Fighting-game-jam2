using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class RailgunAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clip;
    public Animator anim;
    static public Boolean railgunAddEvents = true;
    public LineRenderer lineRenderer;
    public float laserWidth = 0.1f;

    void Start()
    {
        if (railgunAddEvents)
        {
            AnimationEvent evnt;
            evnt = new AnimationEvent();
            evnt.time = 0.5f;
            evnt.functionName = "Shoot";
            anim = gameObject.GetComponent(typeof(Animator)) as Animator;
            clip = anim.runtimeAnimatorController.animationClips[1];
            clip.AddEvent(evnt);
            evnt.time = 0.7f;
            evnt.functionName = "HideLaser";
            clip.AddEvent(evnt);
            railgunAddEvents = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.transform.parent.GetComponent<Enemy>().isshooting)
        {
            anim.Play("RailgunShoot");
        } else {
            anim.Play("RailgunIdle");
        }

    }
    public void Shoot()
    {
        //ContactFilter2D contactFilter;
        //contactFilter.layerMask = LayerMask.GetMask("SolidTiles");
        //contactFilter.useLayerMask = true;
        Vector2 firingPoint = new Vector2(this.gameObject.transform.GetChild(0).position.x, this.gameObject.transform.GetChild(0).position.y);
        RaycastHit2D hit = Physics2D.Raycast(firingPoint, Vector2.left, Mathf.Infinity, LayerMask.GetMask("SolidTiles"));
        RaycastHit2D hit2 = Physics2D.Raycast(firingPoint, Vector2.left, Mathf.Infinity, LayerMask.GetMask("Player"));

        if (hit.distance > hit2.distance) // If hit player before wall
        {
            //player takes a big bomb
        }
        lineRenderer.SetPosition(0, firingPoint);
        lineRenderer.SetPosition(1, hit.point);
        lineRenderer.enabled = true;
    }
    public void HideLaser()
    {
        lineRenderer.enabled = false;
    }
}

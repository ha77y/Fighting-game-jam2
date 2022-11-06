using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RailgunAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clip;
    public Animator anim;
    static public Boolean railgunAddEvents = true;
    public LineRenderer lineRenderer;
    public float laserWidth = 0.1f;
    public int damage = 10;

    void Start()
    {
        if (railgunAddEvents)
        {
            AnimationEvent evnt;
            evnt = new AnimationEvent();
            evnt.time = 1.4f;
            evnt.functionName = "Shoot";
            //anim = gameObject.GetComponent(typeof(Animator)) as Animator;
            clip = anim.runtimeAnimatorController.animationClips[1];
            clip.AddEvent(evnt);
            //evnt.time = 0.7f;
            //evnt.functionName = "HideLaser";
            //clip.AddEvent(evnt);
            railgunAddEvents = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.transform.parent.parent.parent.GetComponent<Enemy>().isshooting)
        {
            anim.Play("RailgunShoot");
        } else {
            anim.Play("RailgunIdle");
        }
        

    }
    public IEnumerator Shoot()
    {        
        Vector2 firingPoint = new Vector2(this.gameObject.transform.parent.GetChild(1).position.x, this.gameObject.transform.parent.GetChild(1).position.y);
        Vector2 playerPos = this.transform.parent.parent.parent.GetChild(0).GetComponent<Sensor>().Player.position;
        RaycastHit2D hit = Physics2D.Raycast(firingPoint, playerPos - firingPoint, Mathf.Infinity, LayerMask.GetMask("SolidTiles"));
        RaycastHit2D hit2 = Physics2D.Raycast(firingPoint, playerPos - firingPoint, Mathf.Infinity, LayerMask.GetMask("Player"));

        if ((hit.distance > hit2.distance & hit2.distance != 0) | (hit.distance == 0 & hit2.distance > 0)) // If hit player before wall
        {
            PlayerStats player = hit2.collider.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
            }
        }
        if (hit.distance != 0)
        {
            lineRenderer.SetPosition(0, firingPoint);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.SetPosition(0, firingPoint);
            lineRenderer.SetPosition(1, (playerPos * 10) - (firingPoint * 9));
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
        }
    }
    public void HideLaser()
    {
        lineRenderer.enabled = false;
    }
}

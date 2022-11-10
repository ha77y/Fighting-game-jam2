using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RailgunAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clip;
    public Animator anim;
    static public Boolean railgunAddEvents = true;
    public LineRenderer lineRenderer;
    public float laserWidth = 0.1f;
    public int damage = 10;
    public LineRenderer targetingLaser;

    void Start()
    {
        targetingLaser.startColor = Color.green;
        targetingLaser.endColor = new Color(0.08f, 0.84f, 0.15f, 1);
        if (railgunAddEvents)
        {
            AnimationEvent evnt;
            evnt = new AnimationEvent();
            evnt.time = 2.3f;
            evnt.functionName = "Shoot";
            clip = anim.runtimeAnimatorController.animationClips[1];
            clip.AddEvent(evnt);
            evnt.time = 1.8f;
            evnt.functionName = "TargetingLaserColor";
            clip.AddEvent(evnt);
            railgunAddEvents = false;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.parent.parent.parent.GetComponent<Enemy>().playerInLOS)
        {
            Vector2 firingPoint = new Vector2(this.gameObject.transform.parent.GetChild(1).position.x, this.gameObject.transform.parent.GetChild(1).position.y);
            Vector2 playerPos = this.transform.parent.parent.parent.GetChild(0).GetComponent<Sensor>().Player.position;
            targetingLaser.SetPosition(0, firingPoint);
            targetingLaser.SetPosition(1, playerPos);
            targetingLaser.enabled = true;
        } else
        {
            targetingLaser.enabled = false;
        }


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
        RaycastHit2D deflect = Physics2D.Raycast(firingPoint, playerPos - firingPoint, Mathf.Infinity, LayerMask.GetMask("Deflect"));
        targetingLaser.startColor = Color.green;
        targetingLaser.endColor = new Color(0.08f, 0.84f, 0.15f, 1);

        if (deflect.distance != 0) //if the player deflects the ray
        {
            Vector2 mousePos = Input.mousePosition;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            lineRenderer.SetPosition(0, firingPoint); //visuals of original laser
            lineRenderer.SetPosition(1, deflect.point);
            lineRenderer.enabled = true;

            Vector2 mousePoint = new Vector2(mouseRay.direction.x + mouseRay.origin.x, mouseRay.direction.y + mouseRay.origin.y);
            mousePoint = (mousePoint * 1000) - (deflect.point * 999);
            LineRenderer deflectedLineRenderer = deflect.transform.GetComponent<LineRenderer>();
            RaycastHit2D deflectHit = Physics2D.Raycast(deflect.point, mousePoint, Mathf.Infinity, LayerMask.GetMask("SolidTiles"));
            RaycastHit2D[] deflectHit2 = Physics2D.RaycastAll(deflect.point, mousePoint, Mathf.Infinity, LayerMask.GetMask("Enemy"));

            if (deflectHit.distance != 0) { //visuals of deflected laser
                deflectedLineRenderer.SetPosition(0, deflect.point);
                deflectedLineRenderer.SetPosition(1, deflectHit.point);
                deflectedLineRenderer.enabled = true;
            } else {
                deflectedLineRenderer.SetPosition(0, deflect.point);
                deflectedLineRenderer.SetPosition(1, mousePoint);
                deflectedLineRenderer.enabled = true;
            }

            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
            deflectedLineRenderer.enabled = false;

            foreach (RaycastHit2D result in deflectHit2) //damage each enemy hit if they are hit before a solid wall
            {
                if ((deflectHit.distance > result.distance & result.distance != 0) | (deflectHit.distance == 0 & result.distance > 0)) // If hit enemy before wall
                {
                    Enemy enemy = result.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Damaged(damage);
                    }
                }
            }
        }
        else
        {

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
    }
    public void HideLaser()
    {
        lineRenderer.enabled = false;
    }
    public void TargetingLaserColor()
    {
        targetingLaser.startColor = Color.yellow;
        targetingLaser.endColor = new Color(0.84f, 0.81f, 0.04f, 1);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Boolean isshooting = true;
    public Boolean isreloading = false;
    public Boolean ispatroling = false;
    public Boolean playerInRange = false;
    public Boolean playerInLOS = false;
    public Boolean willDrop = false;
    public Boolean highgroundSeeking = false;
    public Boolean wander = false;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform left;
    public Transform right;


    public int ammo = -1;
    public int health = 30;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = transform.GetChild(0).GetComponent<Sensor>().Player;

        Debug.DrawRay(leftFoot.transform.position, -Vector2.up);
        Debug.DrawRay(rightFoot.transform.position, -Vector2.up);
        Debug.DrawRay(leftFoot.transform.position, Vector2.up);
        Debug.DrawRay(rightFoot.transform.position, Vector2.up);

        Debug.DrawRay(transform.position, Vector2.left);
        Debug.DrawRay(transform.position, Vector2.right);

        Debug.DrawRay(left.transform.position, Vector2.up);
        Debug.DrawRay(right.transform.position, Vector2.up);

        Debug.DrawRay(left.transform.position, Vector2.down);
        Debug.DrawRay(right.transform.position, Vector2.down);

        RaycastHit2D groundAboveLeft = Physics2D.Raycast(leftFoot.transform.position, -Vector2.up, Mathf.Infinity);
        RaycastHit2D groundAboveright = Physics2D.Raycast(rightFoot.transform.position, Vector2.up, Mathf.Infinity);
        RaycastHit2D groundBelowLeft = Physics2D.Raycast(leftFoot.transform.position, -Vector2.up, Mathf.Infinity);
        RaycastHit2D groundBelowright = Physics2D.Raycast(rightFoot.transform.position, Vector2.up, Mathf.Infinity);

        RaycastHit2D wallLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity);
        RaycastHit2D wallRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity);

        RaycastHit2D highgroundLeft = Physics2D.Raycast(left.transform.position, Vector2.up, Mathf.Infinity);
        RaycastHit2D highgroundRight = Physics2D.Raycast(right.transform.position, Vector2.up, Mathf.Infinity);

        RaycastHit2D dropLeft = Physics2D.Raycast(left.transform.position, Vector2.down, Mathf.Infinity);
        RaycastHit2D dropRight = Physics2D.Raycast(right.transform.position, Vector2.down, Mathf.Infinity);


        if (playerInLOS & playerInRange)
        {
            if ((transform.position.x - player.position.x) > 5 & wallLeft.distance == 0)
            {
                Walk("left");
            } else if ((transform.position.x - player.position.x) < 5 & wallRight.distance == 0)
            {
                Walk("right");
            }
        }

        if (playerInLOS & playerInRange)
        {
            Vector2 localPlayerPos;

            Vector2 playerPos = this.transform.GetChild(0).GetComponent<Sensor>().Player.position;
            if (playerPos != null)
            {
                localPlayerPos.x = this.transform.position.x - playerPos.x;
                localPlayerPos.y = this.transform.position.y - playerPos.y;
                float angle = Mathf.Atan2(localPlayerPos.y, localPlayerPos.x) * Mathf.Rad2Deg;
                Transform gun = this.transform.GetChild(1);
                gun.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        if (ammo == 0 & !isreloading)
        {
            isshooting = false;
            isreloading = true;
        }
        else if (!isreloading & playerInLOS)
        {
            isshooting = true;
            isreloading = false;
        }
        else if (ispatroling & ammo < 3 & !isreloading & ammo != -1)
        {
            isreloading = true;
            isshooting = false;
        } else if (!playerInLOS & !isreloading)
        {
            ispatroling = true;
            isshooting = false;
        }
        

        
    }

    public void Walk(string direction)
    {
        if (direction == "left")
        {
            transform.position = new Vector3(transform.position.x + (Vector2.left.x * speed * Time.deltaTime), transform.position.y);
        } else if (direction == "right")
        {
            transform.position = new Vector3(transform.position.x + (-Vector2.left.x * speed * Time.deltaTime), transform.position.y);
        }


    }

    public void Damaged(int amount)
    {
        health -= amount;
        StartCoroutine(spriteFlash(0.6f, 0.15f));
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator spriteFlash(float duration, float delta)
    {
        for (float i = 0; i < duration; i += delta)
        {
            if (transform.localScale == Vector3.one)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(delta);
        }
        transform.localScale = Vector3.one;
    }
}

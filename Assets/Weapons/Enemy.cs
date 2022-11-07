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
    public int kiteDistance;

    private bool tooClose;

    private RaycastHit2D groundBelowLeft;
    private RaycastHit2D groundBelowRight;
    private RaycastHit2D groundAboveLeft;
    private RaycastHit2D groundAboveRight;
    private RaycastHit2D highgroundLeft;
    private RaycastHit2D highgroundRight;
    private RaycastHit2D dropLeft;
    public RaycastHit2D dropRight;

    public int ammo = -1;
    public int health = 30;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = transform.GetChild(0).GetComponent<Sensor>().Player;

        //Debug.DrawRay(leftFoot.transform.position, -Vector2.up);
        //Debug.DrawRay(rightFoot.transform.position, -Vector2.up);
        //Debug.DrawRay(leftFoot.transform.position, Vector2.up);
        //Debug.DrawRay(rightFoot.transform.position, Vector2.up);

        //Debug.DrawRay(transform.position, Vector2.left*2);
        //Debug.DrawRay(transform.position, Vector2.right*2);

        //Debug.DrawRay(left.transform.position, Vector2.up);
        //Debug.DrawRay(right.transform.position, Vector2.up);

        Debug.DrawRay(leftFoot.transform.position, Vector2.down, Color.red, 1f);
        Debug.DrawRay(rightFoot.transform.position, Vector2.down, Color.red, 1f);

        

        RaycastHit2D groundAboveLeft = Physics2D.Raycast(leftFoot.transform.position, Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D groundAboveright = Physics2D.Raycast(rightFoot.transform.position, Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D groundBelowLeft = Physics2D.Raycast(leftFoot.transform.position, -Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D groundBelowRight = Physics2D.Raycast(rightFoot.transform.position, -Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        RaycastHit2D wallLeft = Physics2D.Raycast(transform.position, Vector2.left*2, Vector2.Distance(transform.position, Vector2.left/10), (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D wallRight = Physics2D.Raycast(transform.position, Vector2.right*2, Vector2.Distance(transform.position, Vector2.right/10), (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        RaycastHit2D highgroundLeft = Physics2D.Raycast(left.transform.position, Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D highgroundRight = Physics2D.Raycast(right.transform.position, Vector2.up, Mathf.Infinity, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        RaycastHit2D dropLeft = Physics2D.Raycast(left.transform.position, Vector2.down, Vector2.Distance(left.transform.position, Vector2.down), (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        RaycastHit2D dropRight = Physics2D.Raycast(right.transform.position, Vector2.down, Vector2.Distance(right.transform.position, Vector2.down), (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        Debug.DrawRay(groundBelowLeft.point, new Vector2 (leftFoot.transform.position.x, leftFoot.transform.position.y) - groundBelowLeft.point, Color.blue, 1f);
        Debug.DrawRay(groundBelowRight.point, new Vector2(rightFoot.transform.position.x, rightFoot.transform.position.y) - groundBelowRight.point, Color.blue, 1f);

        
        if (playerInLOS & playerInRange)
        {

            if ((kiteDistance / 4) > (Vector2.Distance(player.position, transform.position)))
                {
                tooClose = true;
                }
            print((transform.position.x - player.position.x));
            print(transform.position.x);
            print(player.position.x);
            print((transform.position.x - player.position.x) > kiteDistance & (wallLeft.distance == 0 | wallLeft.distance > 1));

            if (
                ((Math.Abs(transform.position.x - player.position.x) > kiteDistance) & (player.position.x < transform.position.x) | (Math.Abs(transform.position.x - player.position.x) < kiteDistance) & (player.position.x > transform.position.x))
                & (wallLeft.distance == 0 | wallLeft.distance > 1)) // if no wall left
            {
                if ((groundBelowLeft.distance < 1)|((willDrop | tooClose) & (groundBelowLeft.distance < 50))) // if will not fall off platform unless willDrop == true or player is within 1/4 kite distance
                {
                    Walk("left");
                }
                
            } else if (
                ((Math.Abs(transform.position.x - player.position.x) > kiteDistance) & (player.position.x > transform.position.x) | (Math.Abs(transform.position.x - player.position.x) < kiteDistance) & (player.position.x < transform.position.x))
                & (wallRight.distance == 0 | wallRight.distance > 1))
            {
                if ((groundBelowRight.distance < 1 )|((willDrop | tooClose) & (groundBelowRight.distance < 50))) // if will not fall off platform unless willDrop == true
                {
                    Walk("right");
                }
            }
            tooClose = false;
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

        }
        else if (direction == "right")
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;
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
    public Boolean jumping = false;
    public Boolean canJump = false;
    public Boolean invincible = false;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform left;
    public Transform right;
    public int kiteDistance = 5;
    public int leaveLOSRange = 18;
    public int enterLOSRange = 12;
    public float jumpForce = 10f;

    private bool tooClose;
    private int counter;
    private string jumpDirection;
    bool flipped = true;
    bool flipped2 = false;

    private RaycastHit2D groundBelowLeft;
    private RaycastHit2D groundBelowRight;
    private RaycastHit2D groundAboveLeft;
    private RaycastHit2D groundAboveRight;
    private RaycastHit2D highgroundLeft;
    private RaycastHit2D highgroundRight;
    private RaycastHit2D wallLeft;
    private RaycastHit2D wallRight;

    public int ammo = -1;
    public int maxAmmo = -1;
    public int health = 30;
    void Start()
    {
        canJump = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform player = transform.GetChild(0).GetComponent<Sensor>().Player;

        groundAboveLeft = Physics2D.Raycast(leftFoot.transform.position, Vector2.up, 10, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        groundAboveRight = Physics2D.Raycast(rightFoot.transform.position, Vector2.up, 10, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        groundBelowLeft = Physics2D.Raycast(leftFoot.transform.position, -Vector2.up, 2, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        groundBelowRight = Physics2D.Raycast(rightFoot.transform.position, -Vector2.up, 2, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        highgroundLeft = Physics2D.Raycast(left.transform.position, Vector2.up, 15, LayerMask.GetMask("Water"));
        highgroundRight = Physics2D.Raycast(right.transform.position, Vector2.up, 15, LayerMask.GetMask("Water"));

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
                Transform sprite = transform.GetChild(2);
                gun.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                if (localPlayerPos.x >= 0) {
                    //gameObject.transform.localScale = new Vector3(1, 1, 1);
                    sprite.transform.localScale = new Vector3(1, 1, 1);
                    gun.transform.localScale = new Vector3(1, 1, 1);
                    if (!flipped)
                    {
                        gun.transform.position = new Vector3(gun.transform.position.x -1.1f, gun.transform.position.y, gun.transform.position.z);
                        flipped = true;
                        flipped2 = false;
                    }
                } else
                {
                    //gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    gun.transform.localScale = new Vector3(1, -1, 1);
                    sprite.transform.localScale = new Vector3(-1, 1, 1);
                    if (!flipped2)
                    {
                        gun.transform.position = new Vector3(gun.transform.position.x+1.1f, gun.transform.position.y, gun.transform.position.z);
                        flipped2 = true;
                        flipped = false;
                    }
                }
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
        }
        else if (!playerInLOS & !isreloading)
        {
            ispatroling = true;
            isshooting = false;
        }
    }

    void Update() {
        wallLeft = Physics2D.Raycast(leftFoot.transform.position, Vector2.left * 2, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles") | LayerMask.GetMask("Enemy")));
        wallRight = Physics2D.Raycast(rightFoot.transform.position, Vector2.right * 2, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles") | LayerMask.GetMask("Enemy")));
        Transform player = transform.GetChild(0).GetComponent<Sensor>().Player;
        if (playerInLOS & playerInRange & !jumping)
        {
            
            if (groundBelowLeft.distance < 0.5 | groundBelowRight.distance < 0.5)
            {
                canJump = true;
            }

            if ((kiteDistance / 2) > (Vector2.Distance(player.position, transform.position)))
                {
                tooClose = true;
                }

            if (
                ((Math.Abs(transform.position.x - player.position.x) > kiteDistance+1) & (player.position.x < transform.position.x) | (Math.Abs(transform.position.x - player.position.x) < kiteDistance-1) & (player.position.x > transform.position.x))
                & (wallLeft.distance == 0 | wallLeft.distance > 1)) // if no wall left
            {
                if ((groundBelowLeft.distance < 1)|((willDrop | tooClose) & (groundBelowLeft.distance < 50))) // if will not fall off platform unless willDrop == true or player is within 1/4 kite distance
                {
                    Walk("left");
                    if (highgroundLeft.distance > 1 & highgroundLeft.distance < 6 & (groundAboveLeft.distance == 0 | groundAboveLeft.distance > 8) & canJump)
                    {
                        jumpDirection = "left";
                        StartCoroutine("Jump");
                    }
                }
                
            } else if (
                ((Math.Abs(transform.position.x - player.position.x) > kiteDistance+1) & (player.position.x > transform.position.x) | (Math.Abs(transform.position.x - player.position.x) < kiteDistance-1) & (player.position.x < transform.position.x))
                & (wallRight.distance == 0 | wallRight.distance > 1))
            {
                if ((groundBelowRight.distance < 1 )|((willDrop | tooClose) & (groundBelowRight.distance < 50))) // if will not fall off platform unless willDrop == true
                {
                    Walk("right");
                    if (highgroundRight.distance > 1 & highgroundRight.distance < 6 & (groundAboveRight.distance == 0 | groundAboveRight.distance > 8) & canJump)
                    {
                        jumpDirection = "right";
                        StartCoroutine("Jump");
                    }
                }
            }
            tooClose = false;
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

    public IEnumerator Jump()
    {
        if (canJump)
        {
            counter = 100;
            transform.GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            jumping = true;
            while (counter-- > 0)
            {
                if (jumpDirection == "left")
                {
                    transform.position = new Vector3(transform.position.x + (Vector2.left.x * (speed*2) * Time.deltaTime), transform.position.y);

                }
                else if (jumpDirection == "right")
                {
                    transform.position = new Vector3(transform.position.x + (-Vector2.left.x * (speed*2) * Time.deltaTime), transform.position.y);
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
            jumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dash")
        {
            Damaged(collision.transform.parent.parent.GetComponent<PlayerStats>().dashDamage);
        } else if (collision.gameObject.tag == "Attack")
        {
            Damaged(collision.transform.parent.parent.GetComponent<PlayerStats>().attackDamage);
        }
    }

    public void Damaged(int amount)
    {
        if (invincible) return;
        health -= amount;
        invincible = true;
        StartCoroutine(spriteFlash(0.6f, 0.15f));
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator spriteFlash(float duration, float delta)
    {
        Transform sprite = transform.GetChild(2);
        for (float i = 0; i < duration; i += delta)
        {
            if (sprite.transform.localScale == Vector3.one)
            {
                sprite.transform.localScale = Vector3.zero;
            }
            else
            {
                sprite.transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(delta);
            invincible = false;
        }
       
        sprite.transform.localScale = Vector3.one;
    }
}

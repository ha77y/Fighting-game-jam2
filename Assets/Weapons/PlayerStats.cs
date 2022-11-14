using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int healing = 45;
    public bool invincible = false;
    public GameObject sprite;
    public float invincibilityDuration = 1.35f;
    public float invincibilityDelta = 0.15f;

    public healthbar hp;
    public CollectableCounter counter;

    public Boolean isAttacking = false;
    public Boolean isDeflecting = false;
    public Boolean isDashing = false;
    public Boolean deflectCooldown = false;
    public Boolean dashCooldown = false;
    public Boolean canWalk = true;
    public Boolean facingByMovement = true;
    public Boolean isJumping = false;

    public int jumps = 2;
    public int jumpForce = 0;
    public int firstJumpForce = 20;
    public int secondJumpForce = 15;
    public int deflectCooldownLength = 10;
    public int dashCooldownLength = 3;
    public int deflectDuration = 5;
    public int dashDamage = 15;
    public float attackDuration = 0.5f;
    public int attackDamage = 5;
    public float dashDuration = 0.1f;

    public Collider2D deflectCollision;
    public Collider2D attackCollision;
    public Collider2D dashCollision;
    public Collider2D dashDamageCollision;
    public Rigidbody2D rb;
    public Transform foot;
    public Boolean isWallLeft;
    public Boolean isWallRight;
    public Transform canvas;
    public Animator animator;

    RaycastHit2D wallLeft, wallRight, wallLeftFoot, wallRightFoot;

    private void deflect()
    {
        isDeflecting = true;
        facingByMovement = false;                                                               // character no longer faces direction of motion
        animator.Play("PlayerDeflect");
        StartCoroutine(Cooldown("isDeflecting", deflectDuration));                              // after deflect length, player faces direction of motion again
        canvas.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = true; // enables the black box infront of the hud icon
        deflectCooldown = true;
        StartCoroutine(Cooldown("deflectCooldown", deflectCooldownLength));                     //deactivates the black box infront of hud after deflect cooldown 
    }

    private void playerDash()
    {
        isDashing = true;   //sets bools 
        invincible = true;
        canWalk = false;
        animator.Play("PlayerDash");
        StartCoroutine(Cooldown("isDashing", dashDuration * 2f));                               // starts 2 second timer for isDashing boolean to swithc back  
        canvas.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true; //enables the black box infront of the hud icon
        dashCooldown = true;
        StartCoroutine(Cooldown("dashCooldown", dashCooldownLength));                           //deactivates the black box infront of hud after dash cooldown 
        StartCoroutine(Dash(dashDuration));                                                     // initaites the actual dash
    }

    private void attack()
    {
        isAttacking = true;
        animator.Play("PlayerAttack");
        StartCoroutine(Cooldown("isAttacking", attackDuration));
    }




    // Start is called before the first frame update
    void Start()
    {
        canvas.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        canvas.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        counter = GameObject.FindWithTag("CollectableCounter").GetComponent<CollectableCounter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // casting raycasts to stop movement into walls 
        wallLeft = Physics2D.Raycast(transform.position, Vector2.left, 5, (LayerMask.GetMask("Default", "SolidTiles")));
        wallRight = Physics2D.Raycast(transform.position, Vector2.right, 5, (LayerMask.GetMask("Default", "SolidTiles")));
        wallLeftFoot = Physics2D.Raycast(foot.transform.position, Vector2.left, 5, (LayerMask.GetMask("Default", "SolidTiles")));
        wallRightFoot = Physics2D.Raycast(foot.transform.position, Vector2.right, 5, (LayerMask.GetMask("Default", "SolidTiles")));

        //sets booleans for neccessary colliders
        if (isDeflecting)
        {
            deflectCollision.gameObject.SetActive(true); // can deflect
            attackCollision.gameObject.SetActive(false); // can't attack
            dashCollision.gameObject.SetActive(false); // can't dash
            dashDamageCollision.gameObject.SetActive(false); // ^^
        }
        else if (isAttacking)
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(true);
            dashCollision.gameObject.SetActive(false);
            dashDamageCollision.gameObject.SetActive(false);
        }
        else if (isDashing)
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(true);
            dashDamageCollision.gameObject.SetActive(true);
        }
        else  // when not attacking, deflecting or dashing 
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(false);
            dashDamageCollision.gameObject.SetActive(false);
        }

        if ((wallLeft.distance < 1.5f & wallLeft.distance != 0) | (wallLeftFoot.distance < 1.5f & wallLeftFoot.distance != 0))
        {
            isWallLeft = true;
        }
        else isWallLeft = false;

        if ((wallRight.distance < 1.5f & wallRight.distance != 0) | (wallRightFoot.distance < 1.5f & wallRightFoot.distance != 0))
        {
            isWallRight = true;
        }
        else isWallRight = false;

    }
    private void Update()
    {
        // on press E or right click
        if ((Input.GetKeyDown(KeyCode.E) | Input.GetKeyDown(KeyCode.Mouse1)) & !deflectCooldown & !isAttacking & !isDashing)
        {
            deflect();
        }

        // on press left shift or middle mouse in 
        else if ((Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKeyDown(KeyCode.Mouse2)) & !dashCooldown & !isAttacking & !isDeflecting)
        {
            playerDash();
        }

        // on press q or left click
        else if ((Input.GetKeyDown(KeyCode.Q) | Input.GetKeyDown(KeyCode.Mouse0)) & !isAttacking & !isDeflecting & !isDashing)
        {
            attack();
        }


        // jump reset on floor collison
        if (Physics2D.OverlapCircle(new Vector3(foot.position.x, foot.position.y), 0.1f, LayerMask.GetMask("SolidTiles", "Default")))
        {
            jumps = 2;
            jumpForce = firstJumpForce;
            isJumping = false;
        }

        // jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumps != 0)
        {
            StartCoroutine(Jump());
        }


        if (!isAttacking & !isDeflecting & !isDashing & !isJumping)
        {
            // play run animation when moving without actions or being in air
            if (Input.GetAxis("Horizontal") != 0 & Physics2D.OverlapCircle(new Vector3(foot.position.x, foot.position.y), 0.1f, LayerMask.GetMask("SolidTiles", "Default")))
            {
                animator.Play("PlayerRun");
            } else // play idle when idle
            {
                animator.Play("PlayerIdle");
            }
        }
    }



    public IEnumerator Cooldown(string boolean, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (boolean == "isDeflecting")
        {
            isDeflecting = !isDeflecting;
            facingByMovement = true;
        }
        else if (boolean == "deflectCooldown")
        {
            deflectCooldown = !deflectCooldown;
            canvas.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (boolean == "isDashing")
        {
            isDashing = !isDashing;
        }
        else if (boolean == "dashCooldown")
        {
            canvas.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            dashCooldown = !dashCooldown;
        }
        else if (boolean == "isAttacking") isAttacking = !isAttacking;



    }
    public IEnumerator Jump()
    {
        isJumping = true;
        if (jumpForce == firstJumpForce & !isDeflecting)
        {
            animator.Play("PlayerJump");
        } else if (!isDeflecting)
        {
            animator.Play("PlayerSecondJump");
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        isJumping = true;
        if (jumpForce == firstJumpForce & !isDeflecting)

        {
            animator.Play("PlayerJump");
        }
        else if (!isDeflecting)
        {
            animator.Play("PlayerSecondJump");
        }
        jumps -= 1;
        jumpForce = secondJumpForce;
    }

    public IEnumerator Dash(float duration)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        for (float i = 0; i < duration; i += Time.deltaTime/2)
        {
            if (gameObject.transform.localScale == new Vector3(-1, 1, 1) & !isWallLeft)
            {
                transform.position = new Vector3(transform.position.x + (Vector2.left.x * 30 * Time.deltaTime), transform.position.y);
            } else if (gameObject.transform.localScale == new Vector3(1, 1, 1) & !isWallRight)
            {
                transform.position = new Vector3(transform.position.x + (-Vector2.left.x * 30 * Time.deltaTime), transform.position.y);
            }
            yield return new WaitForSeconds(Time.deltaTime/2);
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(duration);
        invincible = false;
        canWalk = true;

    }

    public void Damaged(int amount)
    {
        if (invincible) return;
        health -= amount;
        Data.DamageTaken += amount;
        hp.SetHealth(health);
        if (health <= 0)
        {     
            GameOver();
        }
        StartCoroutine(IFrames(invincibilityDuration, invincibilityDelta));
      
        
    }
    public IEnumerator IFrames(float duration, float delta)
    {
        invincible = true;
        for (float i = 0; i < duration; i += delta)
        {
            if (sprite.transform.localScale == Vector3.one)
            {
                sprite.transform.localScale = Vector3.zero;
            } else
            {
                sprite.transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(delta);

        }
        sprite.transform.localScale = Vector3.one;
        invincible = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Healthpack")
        {

            if (health + healing > 100)
            {
                healing = 100 - health;
            }

            health += healing;
            Data.amountHealed += healing;
            healing = 45;
            hp.SetHealth(health);
            Destroy(collision.gameObject);

        }
        if(collision.gameObject.tag == "Collectable")
        {
            Data.collectables++;
            counter.UpdateCounter();
            Destroy(collision.gameObject);
        }
    }


    public void GameOver()
    {
        print("Game Over");

        SceneManager.LoadScene(3);

    }
}


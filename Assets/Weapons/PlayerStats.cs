using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int healing = 45;
    public bool invincible = false;
    public GameObject sprite;
    public float invincibilityDuration = 1.35f;
    public float invincibilityDelta = 0.15f;

    public healthbar hp;

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
  

    // Start is called before the first frame update
    void Start()
    {
        canvas.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        canvas.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wallLeft = Physics2D.Raycast(transform.position, Vector2.left, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        wallRight = Physics2D.Raycast(transform.position, Vector2.right, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        wallLeftFoot = Physics2D.Raycast(foot.transform.position, Vector2.left, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));
        wallRightFoot = Physics2D.Raycast(foot.transform.position, Vector2.right, 5, (LayerMask.GetMask("Default") | LayerMask.GetMask("SolidTiles")));

        if (isDeflecting)
        {
            deflectCollision.gameObject.SetActive(true);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(false);
            dashDamageCollision.gameObject.SetActive(false);
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
        else
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(false);
            dashDamageCollision.gameObject.SetActive(false);
        }

        if ((wallLeft.distance < 0.8f & wallLeft.distance != 0) | (wallLeftFoot.distance < 0.8f & wallLeftFoot.distance != 0))
        {
            isWallLeft = true;
        }
        else isWallLeft = false;

        if ((wallRight.distance < 0.8f & wallRight.distance != 0) | (wallRightFoot.distance < 0.8f & wallRightFoot.distance != 0))
        {
            isWallRight = true;
        }
        else isWallRight = false;

    }
    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.E) | Input.GetKeyDown(KeyCode.Mouse1)) & !deflectCooldown & !isAttacking & !isDashing)
        {
            isDeflecting = true;
            facingByMovement = false;
            animator.Play("PlayerDeflect");
            StartCoroutine(Cooldown("isDeflecting", deflectDuration));
            canvas.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            deflectCooldown = true;
            StartCoroutine(Cooldown("deflectCooldown", deflectCooldownLength));
        } 
        else if ((Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKeyDown(KeyCode.Mouse2)) & !dashCooldown & !isAttacking & !isDeflecting)
        {
            isDashing = true;
            invincible = true;
            canWalk = false;
            animator.Play("PlayerDash");
            StartCoroutine(Cooldown("isDashing", dashDuration*2f));
            canvas.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            dashCooldown = true;
            StartCoroutine(Cooldown("dashCooldown", dashCooldownLength));
            StartCoroutine(Dash(dashDuration));
        }
        else if ((Input.GetKeyDown(KeyCode.Q) | Input.GetKeyDown(KeyCode.Mouse0)) & !isAttacking & !isDeflecting & !isDashing) {
            isAttacking = true;
            animator.Play("PlayerAttack");
            StartCoroutine(Cooldown("isAttacking", attackDuration));
        }
        if (Physics2D.OverlapCircle(new Vector3(foot.position.x, foot.position.y), 0.1f, LayerMask.GetMask("SolidTiles", "Default")))
        {
            jumps = 1;
            jumpForce = firstJumpForce;
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps !=0)
        {
            isJumping = true;
            animator.Play("PlayerJump");
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            jumps -= 1;
            jumpForce = secondJumpForce;
        }
        if (!isAttacking & !isDeflecting & !isDashing & !isJumping)
        {
            if (Input.GetAxis("Horizontal") != 0 & Physics2D.OverlapCircle(new Vector3(foot.position.x, foot.position.y), 0.1f, LayerMask.GetMask("SolidTiles", "Default")))
            {
                animator.Play("PlayerRun");
            } else
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
            healing = 45;
            hp.SetHealth(health);
            Destroy(collision.gameObject);
        }
    }


    public void GameOver()
    {
        print("Game Over");
    }
}


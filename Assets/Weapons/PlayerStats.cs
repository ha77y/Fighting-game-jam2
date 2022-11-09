using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int healing = 45;
    public bool invincible = false;
    public GameObject sprite;
    public float invincibilityDuration = 1.35f;
    public float invincibilityDelta = 0.15f;

    public Boolean isAttacking = false;
    public Boolean isDeflecting = false;
    public Boolean isDashing = false;
    public Boolean deflectCooldown = false;
    public Boolean dashCooldown = false;
    public Boolean canWalk = true;

    public int deflectCooldownLength = 10;
    public int dashCooldownLength = 3;
    public int deflectDuration = 5;
    public int dashDamage = 15;
    public float attackDuration = 0.1f;
    public int attackDamage = 5;
    public float dashDuration = 0.1f;

    public Collider2D deflectCollision;
    public Collider2D attackCollision;
    public Collider2D dashCollision;
    public Collider2D dashDamageCollision;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
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
        } else if (isDashing)
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(true);
            dashDamageCollision.gameObject.SetActive(true);
        } else
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(false);
            dashCollision.gameObject.SetActive(false);
            dashDamageCollision.gameObject.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.E) | Input.GetKeyDown(KeyCode.Mouse1)) & !deflectCooldown & !isAttacking & !isDashing)
        {
            isDeflecting = true;
            StartCoroutine(Cooldown("isDeflecting", deflectDuration));
            deflectCooldown = true;
            StartCoroutine(Cooldown("deflectCooldown", deflectCooldownLength));
        } 
        if ((Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKeyDown(KeyCode.Mouse2)) & !dashCooldown & !isAttacking & !isDeflecting)
        {
            isDashing = true;
            invincible = true;
            canWalk = false;
            StartCoroutine(Cooldown("isDashing", dashDuration*2f));
            dashCooldown = true;
            StartCoroutine(Cooldown("dashCooldown", dashCooldownLength));
            StartCoroutine(Dash(dashDuration));
        }
        if ((Input.GetKeyDown(KeyCode.Q) | Input.GetKeyDown(KeyCode.Mouse0)) & !isAttacking & !isDeflecting & !isDashing) {
            isAttacking = true;
            StartCoroutine(Cooldown("isAttacking", attackDuration));
        }
    }

    public IEnumerator Cooldown(string boolean, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (boolean == "isDeflecting") isDeflecting = !isDeflecting;
        else if (boolean == "deflectCooldown") deflectCooldown = !deflectCooldown;
        else if (boolean == "isDashing")
        {
            isDashing = !isDashing;
            invincible = false;
            canWalk = true;
        }
        else if (boolean == "dashCooldown") dashCooldown = !dashCooldown;
        else if (boolean == "isAttacking") isAttacking = !isAttacking;
        
            
        
    }
    public IEnumerator Dash(float duration)
    {
        print("Dash");
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        for (float i = 0; i < duration; i += Time.deltaTime/2)
        {
            if (gameObject.transform.localScale == new Vector3(-1, 1, 1))
            {
                transform.position = new Vector3(transform.position.x + (Vector2.left.x * 30 * Time.deltaTime), transform.position.y);
            } else if (gameObject.transform.localScale == new Vector3(1, 1, 1))
            {
                transform.position = new Vector3(transform.position.x + (-Vector2.left.x * 30 * Time.deltaTime), transform.position.y);
            }
            yield return new WaitForSeconds(Time.deltaTime/2);
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    public void Damaged(int amount)
    {
        if (invincible) return;
        print("Player Damaged");
        health -= amount;
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
            Destroy(collision.gameObject);
        }
    }
    public void GameOver()
    {
        print("Game Over");
    }
}


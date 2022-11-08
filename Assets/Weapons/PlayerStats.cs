using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public bool invincible = false;
    public GameObject sprite;
    public float invincibilityDuration = 1.35f;
    public float invincibilityDelta = 0.15f;
    public Boolean isAttacking = false;
    public Boolean isDeflecting = false;
    public Boolean isDashing = false;
    public Collider2D deflectCollision;
    public Collider2D attackCollision;
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
        }
        else if (isAttacking)
        {
            deflectCollision.gameObject.SetActive(false);
            attackCollision.gameObject.SetActive(true);
        }
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
        invincible = false;
    }
    public void GameOver()
    {
        print("Game Over");
    }
}


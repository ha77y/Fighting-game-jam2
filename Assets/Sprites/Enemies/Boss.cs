using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    public Enemy enemyScript;
    public Boolean facingLeft = true;
    public float updateSpeed = 3f;
    public Rigidbody2D rb;
    public Boolean charging = false;
    public Boolean flipping = false;
    public Boolean invincible = false;
    public float slowdownSpeed = 10f;
    public float maxSpeed = 10f;
    public float speedUp = 0.5f;
    public float direction = 1f;
    public float health = 300f;
    public Transform[] firePoints;
    public Rigidbody2D rocket;
    public Transform parentObject;
    public Transform leftCollision;
    public Transform rightCollision;
    void Start()
    {
        enemyScript = transform.GetChild(1).GetComponent<Enemy>();
        StartCoroutine(CheckFlip());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (flipping == false && charging == false && transform.GetChild(2).GetComponent<Sensor>().playerInLOS)
        {
            charging = true;
        }
        if (charging && !flipping &&rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity += direction * speedUp * Time.deltaTime * Vector2.left;
        }
    }

    private void Update()
    {
        Vector2 localPlayerPos;
        try
        {
            localPlayerPos.x = this.transform.position.x - enemyScript.player.position.x;
            localPlayerPos.y = this.transform.position.y - enemyScript.player.position.y;
        }
        catch
        {
            return;
        }
        localPlayerPos.x = this.transform.position.x - enemyScript.player.position.x; 
        localPlayerPos.y = this.transform.position.y - enemyScript.player.position.y;
        float angle = Mathf.Atan2(localPlayerPos.y, localPlayerPos.x) * Mathf.Rad2Deg;
        Transform gun = transform.GetChild(1).transform.GetChild(1);
        if (facingLeft && ((angle < 0 && angle > -90) || (angle > 0 && angle < 90)))
        {
            gun.GetChild(0).GetChild(0).GetComponent<RailgunAnimation>().canShoot = true;
            gun.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (!facingLeft && ((angle < 180 && angle > 90) || (angle > -180 && angle < -90)))
        {
            gun.GetChild(0).GetChild(0).GetComponent<RailgunAnimation>().canShoot = true;
            gun.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        } else
        {
            gun.GetChild(0).GetChild(0).GetComponent<RailgunAnimation>().canShoot = false;
        }
    }
    public IEnumerator CheckFlip()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            print("Hello");
            Vector2 localPlayerPos;
            try
            {
                localPlayerPos.x = this.transform.position.x - enemyScript.player.position.x;
                localPlayerPos.y = this.transform.position.y - enemyScript.player.position.y;
            } catch
            {
                continue;
            }
            if (localPlayerPos.x > 0 && !facingLeft)
            {
                flipping = true;
                charging = false;
                
                while (rb.velocity.magnitude > 2)
                {
                    rb.velocity -= Vector2.left * speedUp * Time.deltaTime * direction;
                    yield return new WaitForSeconds(1 / slowdownSpeed);
                }
                transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;
                direction = 1f;
                flipping = false;
                leftCollision.GetComponent<Collider2D>().enabled = true;
                rightCollision.GetComponent<Collider2D>().enabled = false;
                transform.GetChild(1).GetComponent<Enemy>().Flip();
            }
            else if (localPlayerPos.x < 0 && facingLeft)
            {
                flipping = true;
                charging = false;
                
                while (rb.velocity.magnitude > 2)
                {
                    rb.velocity -= Vector2.left * speedUp * Time.deltaTime * direction;
                    yield return new WaitForSeconds(1 / slowdownSpeed);
                }
                transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;
                direction = -1f;
                flipping = false;
                leftCollision.GetComponent<Collider2D>().enabled = false;
                rightCollision.GetComponent<Collider2D>().enabled = true;
                transform.GetChild(1).GetComponent<Enemy>().Flip();
            }
            yield return new WaitForSeconds(updateSpeed);
        }
    }

    public IEnumerator Charge()
    {
        charging = true;
        float direction;
        if (facingLeft) direction = 1f;
        else direction = -1f;
        while (rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity += direction * speedUp * Vector2.left;
            yield return new WaitForSeconds(1 / slowdownSpeed);
        }
    }
    public void Damaged(int amount)
    {
        if (invincible) return;
        health -= amount;
        Data.DamageDealt += amount;
        invincible = true;
        StartCoroutine(spriteFlash(0.6f, 0.15f));
        if (health <= 0)
        {
            Data.EnemiesKilled++;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dash")
        {
            Damaged(collision.transform.parent.parent.GetComponent<PlayerStats>().dashDamage);
        }
        else if (collision.gameObject.tag == "Attack")
        {
            Damaged(collision.transform.parent.parent.GetComponent<PlayerStats>().attackDamage);
        }
    }
    public IEnumerator spriteFlash(float duration, float delta)
        {
            Transform sprite = transform.GetChild(0);
            for (float i = 0; i < duration; i += delta)
            {
                if (sprite.GetComponent<SpriteRenderer>().enabled)
                {
                    sprite.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    sprite.GetComponent<SpriteRenderer>().enabled = true;
                }
                yield return new WaitForSeconds(delta);
                invincible = false;
            }
            sprite.GetComponent<SpriteRenderer>().enabled = true;
        }

        public IEnumerator FireRockets()
    {
        foreach (Transform point in firePoints)
        {
            Rigidbody2D b = Instantiate(rocket, point.position, Quaternion.identity, parentObject);
            b.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

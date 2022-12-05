using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Rocket : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public Enemy enemyScript;
    public float angleChangingSpeed;
    public float movementSpeed;
    public int damage = 15;
    public Boolean launching = true;
    public Boolean deflected = false;
    public Boolean resetParent = true;
    public float size = 1f;
    public Boolean explodeAtMouse = true;
    public float lifeSpan = 8f;
    // Start is called before the first frame update
    void Start()
    {
        enemyScript = transform.GetComponentInParent<Enemy>();
        anim.Play("RocketFly");
        if (resetParent) transform.SetParent(transform.parent.parent, true);
        StartCoroutine(Launch());
        StartCoroutine(LifeSpan(lifeSpan, false));
    }
    void FixedUpdate()
    {
        if (!launching & !deflected)
        {
            Vector2 direction = (Vector2)enemyScript.player.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            //print(rotateAmount);
            //print(-angleChangingSpeed * rotateAmount);
            rb.angularVelocity = -angleChangingSpeed * rotateAmount;
            //print(rb.angularVelocity);
            rb.velocity = transform.up * movementSpeed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Deflect")
        {

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 mousePoint = new Vector2(mouseRay.direction.x + mouseRay.origin.x - transform.position.x, mouseRay.direction.y + mouseRay.origin.y - transform.position.y);
            gameObject.layer = 11;
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            deflected = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePoint.y, mousePoint.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            rb.velocity = transform.up * 25f;
            if (explodeAtMouse) {
                StartCoroutine(ExplodeAtMouse(mousePos));
            } else
            {
                StartCoroutine(LifeSpan(lifeSpan, true));
            }
            
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
            }
            ExpireCollision();
        }
        else if (collision.gameObject.tag == "Enemy" & gameObject.layer == 11)
        {
            collision.transform.GetComponent<Enemy>().Damaged(damage);
            ExpireCollision();
        } else if (collision.gameObject.tag == "Boss" & gameObject.layer == 11)
        {
            collision.transform.GetComponent<Boss>().Damaged(damage);
        }
        else
        {
            ExpireCollision();
        }
    }

    public void ExpireCollision()
    {
        StartCoroutine(Dissipate());
    }   
    public void Expire()
    {
        if (gameObject.layer != 11) {
            StartCoroutine(Dissipate());
        }
        
    }

    public IEnumerator Dissipate()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        transform.GetComponent<Animator>().Play("RocketExpire");
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }

    public IEnumerator ExplodeAtMouse(Vector3 mousePos)
    {
        yield return new WaitForSeconds(mousePos.magnitude/600f);
        StartCoroutine(Dissipate());
    }

    public IEnumerator Launch()
    {
        anim.Play("RocketLaunch");
        launching = true;
        bool pointingAtPlayer = false;
        rb.velocity = transform.up * movementSpeed;
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
        while (!pointingAtPlayer)
        {
            anim.Play("RocketRotate");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 50, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, transform.up);
            if (hit.distance > 0)
            {
                pointingAtPlayer = true;
            }
            Vector2 direction = (Vector2)enemyScript.player.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            if (-200 * rotateAmount > 0) {
                transform.localScale = new Vector3(size, size, 1);
                rb.angularVelocity = -200 * rotateAmount;
            } else
            {
                transform.localScale = new Vector3(-size, size, 1);
                rb.angularVelocity = -200 * rotateAmount;
            }
            yield return new WaitForSeconds(0.1f);
        }
        anim.Play("RocketFly");
        transform.localScale = new Vector3(size, size, 1);
        launching = false;
    }
    public IEnumerator LifeSpan(float length, bool deflected)
    {
        yield return new WaitForSeconds(length);
        if (deflected)
        {
            ExpireCollision();
        } else
        {
            Expire();
        }
        
    }
}

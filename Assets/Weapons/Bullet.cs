using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public AnimationClip flyAnimation;
    public AudioSource expire;
    void Start()
    {
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
            Vector2 mousePoint = new Vector2(mouseRay.direction.x + mouseRay.origin.x, mouseRay.direction.y + mouseRay.origin.y);
            //mousePoint = mousePoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
            gameObject.layer = 11;
            Rigidbody2D b = transform.GetComponent<Rigidbody2D>();
            Vector2 toMouse = (mousePoint - new Vector2(transform.position.x, transform.position.y)).normalized;
            b.velocity = Vector2.zero;
            b.velocity = toMouse.normalized * 10f;
            //b.velocity = t.right * 10f;

            } else if (collision.gameObject.tag == "Player")
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
            }
            ExpireCollision();
        } else if (collision.gameObject.tag == "Enemy" & gameObject.layer == 11)
        {
            collision.transform.GetComponent<Enemy>().Damaged(damage);
            ExpireCollision();
        } else if (collision.gameObject.tag == "Boss" & gameObject.layer == 11)
        {
            collision.transform.GetComponent<Boss>().Damaged(damage);
            ExpireCollision();
        }
        else
        {
            ExpireCollision();
        }
        
    }
    public void Expire()
    {
        if (this.gameObject.layer == 11) return;
        StartCoroutine("Dissapate");
    }
    public void ExpireCollision()
    {
        StartCoroutine("Dissapate");
    }

    public IEnumerator Dissapate()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        transform.GetComponent<Animator>().Play("BulletExpire");
        expire.Play();
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
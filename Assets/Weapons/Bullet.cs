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
            Rigidbody2D b = this.transform.GetComponent<Rigidbody2D>();
            Transform t = collision.transform.parent.parent.GetChild(3).GetChild(0).GetChild(0);
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
        } else
        {
            ExpireCollision();
        }
        
    }
    public void Expire()
    {
        if (this.gameObject.layer == 11) return;
        Destroy(this.gameObject);
    }
    public void ExpireCollision()
    {
        Destroy(this.gameObject);
    }
}

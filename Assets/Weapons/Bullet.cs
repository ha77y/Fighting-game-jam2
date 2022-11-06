using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
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
            print("Bullet Deflected!");
            gameObject.layer = LayerMask.GetMask("Friendly");
            Rigidbody2D b = this.transform.GetComponent<Rigidbody2D>();
            Transform t = collision.transform.parent.GetChild(2);
            b.velocity = Vector2.zero;
            b.velocity = t.right * 10f;

        } else if (collision.gameObject.tag == "Player")
        {
            print("Damage Player");
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
            }
            Expire();
        } else if (collision.gameObject.tag == "Enemy" & gameObject.layer == LayerMask.GetMask("Friendly"))
        {
            print("Damage Enemy");
            Expire();
        } else
        {
            Expire();
        }
        
    }
    public void Expire()
    {
        print("Bullet Expired");
        Destroy(this.gameObject);
    }
}

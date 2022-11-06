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
        if (collision.gameObject.tag == "Player")
        {
            print("Damage Player");
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
            }
            
        }
        Expire();
    }
    public void Expire()
    {
        print("Bullet Expired");
        Destroy(this.gameObject);
    }
}

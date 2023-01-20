using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlailCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collision");
        if (collision.gameObject.tag == "Player")
        {
            print("collisionplayer");
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damaged(damage);
                player.rb.velocity += new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y) * 10f;
            }
        }
    }
}

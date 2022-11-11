using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    public bool PlayerEnter;
    public Collider2D door1;
    public Collider2D door2;
    private ContactFilter2D filter;

    private void Start()
    {
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Enemy");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "player")
        {
            PlayerEnter = true;
            door1.enabled = true;
            door2.enabled = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collider)
    {

        if(collider.tag == "Enemy")
        {
            Collider2D[] results = new Collider2D[10];
            int enemyCount = transform.GetComponent < Collider2D>().OverlapCollider(filter,results);
            print(enemyCount);
            if (enemyCount == 0)
            {
                door1.enabled = false;
                door2.enabled = false;
            }
        }
        
    }

}

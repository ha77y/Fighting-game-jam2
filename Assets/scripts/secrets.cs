using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class secrets : MonoBehaviour
{
    Boolean isEntered = false;
    float opacity = 1f;

    private void FixedUpdate()
    {
        Tilemap tilemap = transform.GetComponent<Tilemap>();
        if (isEntered)
        {
            
            tilemap.color = new Color(1, 1, 1, opacity);
            if (opacity > 0) 
            {
                opacity -= 0.05f;
            }
        } else
        {
            tilemap.color = new Color(1, 1, 1, opacity);
            if (opacity < 1f)
            {
                opacity += 0.05f;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = false;
        }
    }

}

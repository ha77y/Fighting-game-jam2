using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class secrets : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hello");
        transform.GetComponent<TilemapRenderer>().enabled = false;
        //transform.GetComponent<TilemapCollider2D>().enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        secretPath.GetComponent<TilemapRenderer>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.GetComponent<TilemapRenderer>().enabled = true;
        //transform.GetComponent<TilemapCollider2D>().enabled = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class secrets : MonoBehaviour
{
    public Tilemap secretPath;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        secretPath.GetComponent<TilemapRenderer>().enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        secretPath.GetComponent<TilemapRenderer>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        secretPath.GetComponent<TilemapRenderer>().enabled = true;
    }

}

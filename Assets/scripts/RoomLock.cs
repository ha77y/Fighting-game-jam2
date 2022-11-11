using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLock : MonoBehaviour
{
    public bool PlayerEnter;
    public Tilemap door1;
    public Tilemap door2;
    private ContactFilter2D filter;

    private void Start()
    {
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Enemy");
        door1.GetComponent<TilemapRenderer>().enabled = false;
        door1.GetComponent<TilemapCollider2D>().enabled = false;
        door2.GetComponent<TilemapRenderer>().enabled = false;
        door2.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void FixedUpdate()
    {
        Collider2D[] results = new Collider2D[10];
        int enemyCount = transform.GetComponent<Collider2D>().OverlapCollider(filter, results);
        if (enemyCount == 0)
        {
            door1.GetComponent<TilemapRenderer>().enabled = false;
            door1.GetComponent<TilemapCollider2D>().enabled = false;
            door2.GetComponent<TilemapRenderer>().enabled = false;
            door2.GetComponent<TilemapCollider2D>().enabled = false;
            PlayerEnter = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] results = new Collider2D[10];
        int enemyCount = transform.GetComponent<Collider2D>().OverlapCollider(filter, results);
        if (collision.name == "player" & enemyCount != 0)
        {
            PlayerEnter = true;
            door1.GetComponent<TilemapRenderer>().enabled = true;
            door1.GetComponent<TilemapCollider2D>().enabled = true;
            door2.GetComponent<TilemapRenderer>().enabled = true;
            door2.GetComponent<TilemapCollider2D>().enabled = true;
        }

    }
}
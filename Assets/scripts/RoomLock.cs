using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLock : MonoBehaviour
{
    public MusicBetweenScenes Music;
    public bool PlayerEnter;
    public Tilemap door1;
    public Tilemap door2;
    private ContactFilter2D filter;
    public Boolean faded = false;

    private void Start()
    {
       

        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Enemy");
        door1.GetComponent<TilemapRenderer>().enabled = false;
        door1.GetComponent<TilemapCollider2D>().enabled = false;
        door2.GetComponent<TilemapRenderer>().enabled = false;
        door2.GetComponent<TilemapCollider2D>().enabled = false;
        Music = GameObject.FindWithTag("GameMusic").GetComponent<MusicBetweenScenes>();
        print("fadedown?");
        Music.fadedown();
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
            if (!faded)
            {
                print("fadedown?");
                Music.fadedown();
                faded = true;
            }
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
            print("fadeup?");
            Music.fadeup();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ambush : MonoBehaviour
{
    public GameObject[] enemies;
    public Tilemap door;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
            door.GetComponent<TilemapRenderer>().enabled = true;
            door.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}

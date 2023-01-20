using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class fakeVictoryCrate : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap floor;
    public Transform cam;
    public Transform player;
    public Boolean centerCamY;
    public Transform boss;

    private void Start()
    {
        floor.gameObject.SetActive(true);
        floor.GetComponent<TilemapRenderer>().enabled = true;
        floor.GetComponent<TilemapCollider2D>().enabled = true;
    }

    private void Update()
    {
        if (centerCamY)
        {
            cam.position = new Vector3(cam.position.x, player.position.y, cam.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(FallSequence());
        }
    }
    public IEnumerator FallSequence()
    {
        player.GetComponent<PlayerStats>().canWalk = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        int i = 10;
        while (i != 0)
        {
            cam.position = new Vector3(cam.position.x, cam.position.y + 1, cam.position.z);
            yield return new WaitForSeconds(0.2f);
            cam.position = new Vector3(cam.position.x, cam.position.y - 1, cam.position.z);
            yield return new WaitForSeconds(0.2f);
            if (i % 2 != 0)
            {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }
            i--;
        }
        centerCamY = true;
        floor.gameObject.SetActive(false);
        floor.GetComponent<TilemapRenderer>().enabled = false;
        floor.GetComponent<TilemapCollider2D>().enabled = false;
        player.GetComponent<PlayerStats>().canWalk = true;
        yield return new WaitForSeconds(3f);
        centerCamY = false;
        cam.position = new Vector3(cam.position.x, -112, cam.position.z);
        player.position = new Vector3(player.position.x, player.position.y - 10, player.position.z);
        yield return new WaitForSeconds(2f);
        boss.GetChild(0).GetComponent<tankAnimator>().anim.Play("TankStart");
    }
}

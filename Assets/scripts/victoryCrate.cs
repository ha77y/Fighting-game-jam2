using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class victoryCrate : MonoBehaviour
{
    private GameObject music;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            music = GameObject.FindWithTag("GameMusic");
            music.GetComponent<AudioSource>().enabled = false;
            SceneManager.LoadScene(2);
        }
    }
}

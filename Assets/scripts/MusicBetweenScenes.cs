using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBetweenScenes : MonoBehaviour
{
    private AudioSource Music;
    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
    }
    private void Awake()
    {
        
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

   
    public void fadeup()
    {
        while (Music.volume < 1f)
        {
            Music.volume += 0.1f * Time.deltaTime;
        }
        Music.volume = 1f;
    }
    public void fadedown()
    {
        while (Music.volume > 0.6f)
        {
            Music.volume -= 0.1f * Time.deltaTime;
        }
        Music.volume = 0.6f;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBetweenScenes : MonoBehaviour
{
    private AudioSource Music;
    public Boolean upfade;
    public Boolean downfade;
    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
    }
    private void Awake()
    {
        Music = gameObject.GetComponent<AudioSource>();
        Music.volume = 0.8f;
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
            print("music destroyed");
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (upfade)
        {
            if (Music.volume < 0.8f)
            {
                Music.volume += 0.01f;
            } else
            {
                upfade = false;
            }
        }
        else if (downfade)
        {
            if (Music.volume > 0.6f)
            {
                Music.volume -= 0.01f;
            }
            else
            {
                downfade = false;
            }
        }
        print(Music.volume);
        print(upfade);
        print(downfade);
    }


    public void fadeup()
    {
        upfade = true;
        downfade = false;
    }
    public void fadedown()
    {
        upfade = false;
        downfade = true;
    }

}

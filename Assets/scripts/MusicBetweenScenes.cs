using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBetweenScenes : MonoBehaviour
{
    private AudioSource Music;
    public Boolean upfade;
    public Boolean downfade;
    public Boolean Speaking;
    private void Start()
    {
        Music = gameObject.GetComponent<AudioSource>();
    }
    private void Awake()
    {
        Music = gameObject.GetComponent<AudioSource>();
        Music.volume = 0.3f;
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
            if (Music.volume < 0.3f)
            {
                Music.volume += 0.01f;
            } else
            {
                upfade = false;
            }
        }
        else if (downfade)
        {
            if (Music.volume > 0.2f)
            {
                Music.volume -= 0.01f;
            }
            else
            {
                downfade = false;
            }
        }
        if (Speaking)
        {
            if (Music.volume > 0.1f)
            {
                Music.volume -= 0.01f;
            }
        }
        if (!Speaking)
        {
            if (!upfade && !downfade)
            {
                if (Music.volume < 0.2f)
                {
                    Music.volume += 0.01f;
                }
            }
        }
        
        print(Music.volume);
        print(upfade);
        print(downfade);
        print(Speaking);
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
    public void SpeakingStart()
    {
        Speaking = true;
        upfade = false;
        downfade = false;
    }
    public void SpeakingEnd()
    {
        Speaking = false;
    }

}

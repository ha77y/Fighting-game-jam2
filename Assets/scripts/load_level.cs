using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level : MonoBehaviour
{
    private MusicBetweenScenes Music;
    public int sceneNum;
    public Animator anim;
    public Boolean hovering = false;
    private void Start()
    {
        Music = GameObject.FindWithTag("GameMusic").GetComponent<MusicBetweenScenes>();
    }

    private void FixedUpdate()
    {
        if (anim != null)
        {
            if (hovering)
            {
                anim.Play("tryAgain");
            }
            else
            {
                anim.Play("idle");
            }
        }
    }


    public void GoLevel1()
    {
        Music.fadedown();
        if (sceneNum == 1|| sceneNum == 0)
        {
            Data.collectables = 0;
            Data.DamageDealt = 0;
            Data.DamageTaken = 0;
            Data.amountHealed = 0;
            Data.EnemiesKilled = 0;
            if (Music.enabled != true)
            {
                Music.enabled = true;
            }
        }
        if (sceneNum == 3)
        {
            Music.enabled = false;
        }
        SceneManager.LoadScene(sceneNum);
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public void CursorHover()
    {
        hovering = true;
    }

    public void CursorStopHover()
    {
        hovering = false;
    }
}

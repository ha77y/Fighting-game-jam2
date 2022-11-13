using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level : MonoBehaviour
{
    private MusicBetweenScenes Music;
    public int sceneNum;
    private void Start()
    {
        Music = GameObject.FindWithTag("GameMusic").GetComponent<MusicBetweenScenes>();
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
}

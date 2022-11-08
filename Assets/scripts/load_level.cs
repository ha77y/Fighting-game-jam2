using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level : MonoBehaviour
{
    public int sceneNum;
    public void GoLevel1()
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}

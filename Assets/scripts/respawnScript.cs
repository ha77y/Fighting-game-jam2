using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnScript : MonoBehaviour
{
    void OnTriggerEnter2D()
    {
        Data.SpawnPoint = this.transform;
        if (Data.TutorialPlayed == false)
        {
            Data.TutorialPlayed = true;
        }
    }
}

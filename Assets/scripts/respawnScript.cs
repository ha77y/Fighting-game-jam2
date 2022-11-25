using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnScript : MonoBehaviour
{ 
    void OnTriggerEnter2D()
    {
        print(Data.SpawnPoint);
        Data.SpawnPoint = gameObject.transform.position;
        if (Data.TutorialPlayed == false)
        {
            Data.TutorialPlayed = true;
        }
    }
}

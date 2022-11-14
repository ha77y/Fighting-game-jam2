using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{

    void Start()
    {
        int i = 0;
        while (transform.GetChild(i) != null)
        {
            Data.MaxEnemies++;
            i++;
        } 
    }

    
}

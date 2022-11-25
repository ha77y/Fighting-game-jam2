using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (Data.SpawnPoint != null)
        {
            this.transform.position = Data.SpawnPoint.position;
        }
    }


}

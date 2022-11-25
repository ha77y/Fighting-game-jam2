using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject listOfDoors;

    void Start()
    {


        if (Data.SpawnPoint != new Vector3(0,0,0))
        {
            gameObject.transform.position = Data.SpawnPoint;
        }
    }
    


}

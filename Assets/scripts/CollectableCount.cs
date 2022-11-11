using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCount : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while (transform.GetChild(i)!= null)
        {
            Data.MaxCollectables++;
            i++;
            print(Data.MaxCollectables);
        }
    }

}

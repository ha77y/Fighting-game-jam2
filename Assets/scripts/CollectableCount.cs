using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCount : MonoBehaviour
{
    private CollectableCounter counter;
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        counter = GameObject.FindWithTag("CollectableCounter").GetComponent<CollectableCounter>();
        while (transform.GetChild(i)!= null)
        {
            Data.MaxCollectables++;
            counter.UpdateCounter();
            i++;
            print(Data.MaxCollectables);
        }
        
        
    }

}

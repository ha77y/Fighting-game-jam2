using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showButtons : MonoBehaviour
{
  

    public void ShowButtons()
    {
        gameObject.transform.parent.GetChild(0).transform.position = new Vector3(0, -1, 0);
        gameObject.transform.parent.GetChild(1).transform.position = new Vector3(0, -3, 0);
        
    }

}

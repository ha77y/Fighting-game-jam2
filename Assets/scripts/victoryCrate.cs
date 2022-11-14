using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryCrate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //swap to victory scene
        }
    }
}

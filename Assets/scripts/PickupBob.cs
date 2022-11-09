using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBob : MonoBehaviour
{
    private float height;
    private float initialHeight;
    private bool up;


    private void Start()
    {
        height = gameObject.transform.position.y;
        initialHeight = height;
        up = true;
    }

    void Update()
    {
        if (height >= initialHeight +0.2)
        {
            up = false;
        }
        else if (height <= initialHeight -0.2)
        {
            up = true;
        }

        if (up)
        {
            height += 0.001f;
        }
        else
        {
            height -= 0.001f;
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, height,gameObject.transform.position.z);
    }
}

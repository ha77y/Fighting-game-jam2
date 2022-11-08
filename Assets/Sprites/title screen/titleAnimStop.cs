using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleAnimStop : MonoBehaviour
{
    public static Animation myAnimation;

    public new Animation animation;
    // Start is called before the first frame update
    public static void AnimStop()
    {
        myAnimation.gameObject.GetComponent<Animator>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    public Animator anim;
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        print("Hello");
        StartCoroutine("Delay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        print("Play");
        anim.Play("Background");
    }
}

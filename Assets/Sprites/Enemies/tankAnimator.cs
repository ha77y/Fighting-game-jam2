using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        anim.Play("TankIdle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Started()
    {
        transform.parent.GetComponent<Boss>().startFlip = true;
        transform.parent.GetComponent<Boss>().started = true;
        anim.Play("TankAttack");
    }

    public void Fire()
    {
        StartCoroutine(transform.parent.GetComponent<Boss>().FireRockets());
    }
}

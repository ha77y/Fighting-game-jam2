using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        anim.Play("TankAttack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        StartCoroutine(transform.parent.GetComponent<Boss>().FireRockets());
    }
}

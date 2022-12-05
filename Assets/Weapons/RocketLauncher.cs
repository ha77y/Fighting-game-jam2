using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RocketLauncher : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public Vector3 oldPos = new Vector3(0, 0, 0);
    public Boolean firing = false;
    public Boolean reloading = false;
    public Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = transform.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyScript.isLanding & !enemyScript.jumping & !firing & !reloading)
        {
            if (rb.velocity.magnitude != 0 | oldPos.x != transform.position.x)
            {
                anim.Play("rocketmanWalk");
            } else if (enemyScript.ammo <= 0 && Math.Abs(enemyScript.player.transform.position.x - transform.position.x) > enemyScript.kiteDistance - 1)
            {
                anim.Play("rocketmanReady");
                reloading = true;
                enemyScript.canWalk = false;
            }
            else if ((enemyScript.playerInLOS || enemyScript.isshooting) && !firing && Math.Abs(enemyScript.player.transform.position.x - transform.position.x) > enemyScript.kiteDistance - 1)
            {
                anim.Play("rocketmanFire");
                firing = true;
                enemyScript.canWalk = false;
            }
            else
            {
                anim.Play("rocketmanIdle");
            }
        }
        oldPos = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fatguyAnimator : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer gunSprite;
    public Rigidbody2D rb;
    public Vector3 oldPos = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        gunSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude != 0 | oldPos.x != transform.position.x)
        {
            anim.Play("fatguyWalk");
            gunSprite.enabled = true;
        }
        else if (transform.GetComponent<Enemy>().playerInLOS | transform.GetComponent<Enemy>().isshooting)
        {
            anim.Play("fatguyIdle2");
            gunSprite.enabled = true;
        }
        else
        {
            anim.Play("fatguyIdle");
            gunSprite.enabled = false;
        }
        oldPos = transform.position;
    }
}
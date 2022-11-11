using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class smgAnimator : MonoBehaviour
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
        if (!transform.GetComponent<Enemy>().isLanding & !transform.GetComponent<Enemy>().jumping)
        {
            if (rb.velocity.magnitude != 0 | oldPos.x != transform.position.x)
            {
                anim.Play("smgWalk");
                gunSprite.enabled = true;
            }
            else if (transform.GetComponent<Enemy>().playerInLOS | transform.GetComponent<Enemy>().isshooting)
            {
                anim.Play("smgIdle2");
                gunSprite.enabled = true;
            }
            else
            {
                anim.Play("smgIdle");
                gunSprite.enabled = false;
            }
        }
        oldPos = transform.position;
        

    }
    public void Jump()
    {
        anim.Play("smgJump");
    }

    public IEnumerator Land()
    {
        transform.GetComponent<Enemy>().isLanding = true;
        anim.Play("smgLand");
        yield return new WaitForSeconds(1f);
        transform.GetComponent<Enemy>().isLanding = false;
    }

}

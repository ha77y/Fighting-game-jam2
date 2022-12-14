using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Player_Script : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D Rigidbody2D;
    [SerializeField] private Transform Pointer;
    public Boolean isNSFW;
    public Boolean canWalk = true;
    public Animator animator;
    private Vector3 mousepos;
    private float mouserotX;
    private float mouserotY;
    private Quaternion RotQuaternion;



    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<PlayerStats>().canWalk)
        {
            
            if (Input.GetAxis("Horizontal") > 0 & !transform.GetComponent<PlayerStats>().isWallRight)
            {
                transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime), transform.position.y);
                if (transform.GetComponent<PlayerStats>().facingByMovement)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else if (Input.GetAxis("Horizontal") < 0 & !transform.GetComponent<PlayerStats>().isWallLeft)
            {
                transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime), transform.position.y);
                if (transform.GetComponent<PlayerStats>().facingByMovement)
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        /*if (jumps > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D.velocity = Vector3.zero;
                Rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                jumps -= 1;
                jumpForce = secondJumpForce;

            }
        }
        if (Physics2D.OverlapCircle(new Vector3(GroundCheckTransform.position.x, GroundCheckTransform.position.y), 0.1f, LayerMask.GetMask("SolidTiles")) | Physics2D.OverlapCircle(new Vector3(GroundCheckTransform.position.x, GroundCheckTransform.position.y), 0.1f, LayerMask.GetMask("Default")))
        {
            jumps = 1;
            jumpForce = firstJumpForce;
        }
        /*mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        mouserotX = Mathf.Atan2(mousepos.x,gameObject.transform.position.x);
        mouserotY = Mathf.Atan2(mousepos.y, gameObject.transform.position.y);
        Pointer.rotation = new Quaternion(mouserotX,mouserotY,0,0);
        Vector3 relativePos = mousepos - gameObject.transform.position;
        Pointer.rotation = Quaternion.LookRotation(relativePos, new Vector3(0, 0, 0));
        */
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 gunPos = Camera.main.WorldToScreenPoint(Pointer.transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (transform.localScale.x==1)
        {
            if (isNSFW)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            } else
            {
                Pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        } else if (transform.localScale.x==-1)
        {
            if (isNSFW)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
            }
            else
            {
                Pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
            }
        }

        if (mousePos.x > 30 & !transform.GetComponent<PlayerStats>().facingByMovement) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        } 
        else if (mousePos.x < -10 & !transform.GetComponent<PlayerStats>().facingByMovement)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }


    }
    
}




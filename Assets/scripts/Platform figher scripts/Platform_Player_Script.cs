using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Player_Script : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float firstJumpForce = 10f;
    [SerializeField] private float secondJumpForce = 8f;
    [SerializeField] private Rigidbody2D Rigidbody2D;
    [SerializeField] private Transform GroundCheckTransform = null;
    [SerializeField] private Transform Pointer;
    float jumpForce;
    private Vector3 mousepos;
    private float mouserotX;
    private float mouserotY;
    private Quaternion RotQuaternion;

    private int jumps = 1;
    // Start is called before the first frame update
    void Start()
    {
        jumpForce = firstJumpForce;
    }

    // Update is called once per frame
    void Update()
    { 
        transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal")* speed * Time.deltaTime ), transform.position.y);

        if (jumps > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D.velocity = Vector3.zero;
                Rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                jumps -= 1;
                jumpForce = secondJumpForce;
                
            }
        }
        if (Physics2D.OverlapCircle(new Vector3(GroundCheckTransform.position.x, GroundCheckTransform.position.y), 0.1f, LayerMask.GetMask("SolidTiles")))
        {
            jumps = 1;
            jumpForce = firstJumpForce;
        }
        mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        /*
        mouserotX = Mathf.Atan2(mousepos.x,gameObject.transform.position.x);
        mouserotY = Mathf.Atan2(mousepos.y, gameObject.transform.position.y);
        Pointer.rotation = new Quaternion(mouserotX,mouserotY,0,0);
        */
        Vector3 relativePos = mousepos - gameObject.transform.position;
        Pointer.rotation = Quaternion.LookRotation(relativePos, new Vector3(0,0,1)) ;
        
    }
}

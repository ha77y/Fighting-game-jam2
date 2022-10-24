using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Player_Script : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private Rigidbody2D Rigidbody2D;
    [SerializeField] private Transform GroundCheckTransform = null;

    private int jumps = 1;
    // Start is called before the first frame update
    void Start()
    {
        
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
                Rigidbody2D.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
                jumps -= 1;
                JumpForce = 5f;
                
            }
        }
        if (Physics2D.OverlapCircle(new Vector3(GroundCheckTransform.position.x, GroundCheckTransform.position.y), 0.1f, layerMask:3))
        {
            jumps = 1;
            JumpForce = 10f;
        }
    }
}

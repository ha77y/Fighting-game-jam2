using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Player_Script : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private Rigidbody2D Rigidbody2D;
    [SerializeField] private Transform GroundCheckTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal")* speed * Time.deltaTime ), transform.position.y);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D.AddForce (Vector3.up * JumpForce , ForceMode2D.Impulse);
        }
        if (Physics2D.OverlapCircle(new Vector3(GroundCheckTransform.position.x,GroundCheckTransform.position.y), 0.1f))
        {

        }
    }
}

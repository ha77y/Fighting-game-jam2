using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_script : MonoBehaviour
{
    [SerializeField] float speed; 
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0f)
        {
            transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime), transform.position.y);
        }
        if (Input.GetAxis("Vertical") != 0f)
        {
            transform.position = new Vector3(transform.position.x , transform.position.y + (Input.GetAxis("Vertical") * speed * Time.deltaTime));

        }

    }
}

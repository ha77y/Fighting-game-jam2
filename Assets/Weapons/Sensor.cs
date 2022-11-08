using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Sensor : MonoBehaviour 
{
    // Start is called before the first frame update
    public CircleCollider2D sensorBox;
    public Transform Player = null;
    public bool showGizmos = true;
    public LineRenderer lineRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.GetComponent<Enemy>().playerInRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.position - transform.position, Vector2.Distance(transform.position, Player.position), LayerMask.GetMask("SolidTiles"));
            if (hit.distance != 0)
            {
                transform.parent.GetComponent<Enemy>().playerInLOS = false;
            } else
            {
                transform.parent.GetComponent<Enemy>().playerInLOS = true;
            }
        } else
        {
            transform.parent.GetComponent<Enemy>().playerInLOS = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player Entered");
            sensorBox.radius = transform.parent.GetComponent<Enemy>().leaveLOSRange;
            transform.parent.GetComponent<Enemy>().playerInRange = true;
            Player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player Exited");
            sensorBox.radius = transform.parent.GetComponent<Enemy>().enterLOSRange;
            transform.parent.GetComponent<Enemy>().playerInRange = false;
            Player = null;
        }
    }
}

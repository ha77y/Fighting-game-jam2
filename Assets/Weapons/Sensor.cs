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
    public bool seenPlayer = false;
    public bool playerInLOS = false;
    public bool playerInRange = false;
    public bool attatchedToEnemy = true;
    public Transform head;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((!attatchedToEnemy && !Player.transform.GetComponent<PlayerStats>().frozen) ||  (this.transform.parent.GetComponent<Enemy>().playerInRange && !Player.transform.GetComponent<PlayerStats>().frozen))
        {
            RaycastHit2D hit = Physics2D.Raycast(head.transform.position, Player.position - head.transform.position, Vector2.Distance(head.transform.position, Player.position), LayerMask.GetMask("SolidTiles"));
            if (hit.distance != 0)
            {
                playerInLOS = false;
                transform.parent.GetComponent<Enemy>().playerInLOS = false;
                
            } else
            {
                playerInLOS = true;
                transform.parent.GetComponent<Enemy>().playerInLOS = true;
                
            }
        } else
        {
            playerInLOS = false;
            transform.parent.GetComponent<Enemy>().playerInLOS = false;
            
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            Player = collision.gameObject.transform;
            seenPlayer = true;
            sensorBox.radius = transform.parent.GetComponent<Enemy>().leaveLOSRange;
            transform.parent.GetComponent<Enemy>().playerInRange = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            sensorBox.radius = transform.parent.GetComponent<Enemy>().enterLOSRange;
            transform.parent.GetComponent<Enemy>().playerInRange = false;
            
            //Player = null;
        }
    }
}

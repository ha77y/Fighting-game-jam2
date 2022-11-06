using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Boolean isshooting = true;
    public Boolean isreloading = false;
    public Boolean ispatroling = false;
    public Boolean playerInRange = false;
    public Boolean playerInLOS = false;

    public int ammo = -1;
    public float health = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerInLOS)
        {
            Vector2 target = this.transform.GetChild(0).GetComponent<Sensor>().Player.position;
            this.transform.GetChild(1).transform.rotation = Quaternion.LookRotation(target);
        }*/




        if (ammo == 0 & !isreloading)
        {
            isshooting = false;
            isreloading = true;
        }
        else if (!isreloading & playerInLOS)
        {
            isshooting = true;
            isreloading = false;
        }
        else if (ispatroling & ammo < 3 & !isreloading & ammo != -1)
        {
            isreloading = true;
            isshooting = false;
        } else if (!playerInLOS & !isreloading)
        {
            ispatroling = true;
            isshooting = false;
        }

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        
    }
}

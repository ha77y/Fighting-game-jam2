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
    public int ammo = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo == 0 & !isreloading & !ispatroling)
        {
            isshooting = false;
            isreloading = true;
        }
        else if (!ispatroling & !isreloading)
        {
            isshooting = true;
            isreloading = false;
        }
        else if (ispatroling & ammo < 3 & !isreloading)
        {
            isreloading = true;
            isshooting = false;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private bool canAttack;
    private float cooldown;

    // Update is called once per frame
    void Update()
    {
        if (cooldown == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

            }
        } 
                
    }

}

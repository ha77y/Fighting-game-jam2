using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    public bool PlayerEnter;
    public bool locked;
    private Enemy enemy;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Enemy" && PlayerEnter == true)
        {
            locked = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Enemy" && PlayerEnter == true)
        {
            locked = false;
        }
    }
}

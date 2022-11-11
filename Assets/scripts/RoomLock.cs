using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    public bool PlayerEnter;
    public bool locked;
    private Enemy enemy;
    public Collider2D door1;
    public Collider2D door2;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Enemy" && PlayerEnter == true)
        {
            locked = true;
            door1.enabled = true;
            door2.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Enemy" && PlayerEnter == true)
        {
            locked = false;
            door1.enabled = false;
            door2.enabled = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        if (playerInLOS & playerInRange)
        {
            Vector2 localPlayerPos;
            Vector2 playerPos = this.transform.GetChild(0).GetComponent<Sensor>().Player.position;
            if (playerPos != null)
            {
                localPlayerPos.x = this.transform.position.x - playerPos.x;
                localPlayerPos.y = this.transform.position.y - playerPos.y;
                float angle = Mathf.Atan2(localPlayerPos.y, localPlayerPos.x) * Mathf.Rad2Deg;
                Transform gun = this.transform.GetChild(1);
                gun.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

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
    public void Damaged(int amount)
    {

    }

    /*public IEnumerator spriteFlash(float duration, float delta)
    {
        invincible = true;
        for (float i = 0; i < duration; i += delta)
        {
            if (sprite.transform.localScale == Vector3.one)
            {
                sprite.transform.localScale = Vector3.zero;
            }
            else
            {
                sprite.transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(delta);

        }
        invincible = false;
    }*/
}

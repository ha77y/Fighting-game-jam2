using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Damaged(int amount)
    {
        print("Player Damaged");
        health -= amount;
        if (health <= 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        print("Game Over");
    }
}


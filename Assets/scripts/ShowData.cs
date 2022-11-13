using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ShowData : MonoBehaviour
{

    public TextMeshProUGUI StatsBox;


    void Start()
    {
        

        string stats = "Collectables : " + Data.collectables + " / " + Data.MaxCollectables + "\n \n" +
                       "Enemies killed : " + Data.EnemiesKilled + " / " + Data.MaxEnemies + "\n \n" + 
                       "Damage Dealt : " + Data.DamageDealt + "\n \n"  +
                       "Damage Taken : " + Data.DamageTaken +"\n \n" + 
                       "Amount Healed : " + Data.amountHealed ;

        StatsBox.SetText(stats);
    }

    
}

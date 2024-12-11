using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Isaac Soto
 * 11/9/24
 * This is the SlotMachine Script 
 */
public class Test_Slots : MonoBehaviour
{
    public int Money = 50000;
    public bool isPowerUpActive = false; 

    void OnTriggerEnter(Collider other)
    {
        // Referrencing the player controller
        var ply = other.GetComponent <carController>();
        if (ply == null) return;

        // Money Bet = Public int money 
        var Bet = Money;

        //When the Doublepoints is activated
        if (isPowerUpActive)
        {
            Bet *= 2;
        }

        // Negative the bet when inserted from the player
        ply.AddPoints(-Bet);

       // random range of losing 
        var chance = Random.Range(0, 100);
        if (chance < 75)
        {
            Debug.Log("You stink, loser.");
            return;
        }

       //Random range of winning 
        var RewardScale = Random.Range(1, 100); 
        ply.AddPoints(Bet * RewardScale); 
        Debug.Log("You are winner, hahaha!");
        Destroy(gameObject);
    }
   
}

















































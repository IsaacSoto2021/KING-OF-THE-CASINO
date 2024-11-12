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
    public int Money = 1000;
    public bool isPowerUpActive = false; 

    void OnTriggerEnter(Collider other)
    {
        var ply = other.GetComponent<Test_PlayerController>();
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
        var chance = Random.Range(0, 10);
        if (chance < 7)
        {
            Debug.Log("You stink, loser.");
            return;
        }

       //Random range of winning 
        var RewardScale = Random.Range(1, 10); 
        ply.AddPoints(Bet * RewardScale); 
        Debug.Log("You are winner, hahaha!");
    }









}

















































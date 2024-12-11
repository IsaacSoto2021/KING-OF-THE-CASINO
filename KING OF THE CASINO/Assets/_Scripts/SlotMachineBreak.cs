using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineBreak : MonoBehaviour
{
    public int Money = 0;
    public bool isPowerUpActive = false;

    void OnTriggerEnter(Collider other)
    {
        var ply = other.GetComponent<carController>();
        if (ply == null) return;

        // Money Bet = Public int money 
        var Bet = Money;

        // When the Doublepoints is activated, only apply to the reward
        var finalBet = Bet;
        if (isPowerUpActive)
        {
            finalBet *= 2;
        }

        // Negative the bet when inserted from the player
        ply.AddPoints(-Bet);

        Destroy(gameObject);

        // Random range of losing
        var chance = Random.Range(0, 100);
        if (chance < 75)
        {
            Debug.Log("You stink, loser.");
            return;
        }

        // Random range of winning
        var RewardScale = Random.Range(1, 100);
        ply.AddPoints(finalBet * RewardScale);
        Debug.Log("You are winner, hahaha!");


    }












}

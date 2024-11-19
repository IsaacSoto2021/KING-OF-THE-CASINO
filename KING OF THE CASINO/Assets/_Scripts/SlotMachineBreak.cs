using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineBreak : MonoBehaviour
{
    public int Money = 1000;
    public bool isPowerUpActive = false;

    public void OnTriggerEnter(Collider other)
    {
        var ply = other.GetComponent<carController>();
        if (ply == null) return;

        var Bet = Money;

        if (isPowerUpActive)
        {
            Bet *= 2;
        }

        ply.AddPoints(-Bet);

        var chance = Random.Range(0, 5);
        if (chance <= 0)
        {
            Debug.Log("You stink, loser.");
            return;
        }


        var RewardScale = Random.Range(1, 10);
        ply.AddPoints(Bet * RewardScale);
        Debug.Log("You are winner, hahaha!");


        GetComponent<Renderer>().material.color = Color.gray;
    }












 }

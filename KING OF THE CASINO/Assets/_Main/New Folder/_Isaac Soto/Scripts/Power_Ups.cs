using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;
/*
 * Isaac Soto 
 * 10/24/24
 * [ Power-up script that randomizes power-ups given to the player on pick-up]
 */

public class Power_Ups : MonoBehaviour
{
    public Freeze freezeScript;

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int Value = Random.Range(0, 3);
            if (Value == 0)
            {
                SpeedBoost();
            }
            else if (Value == 1)
            {
                Freeze();
            }
            else if (Value == 2)
            {
                DoublePoints();
            }

            Destroy(gameObject);
        }

      
    }

   public void Freeze()
   {

        if (freezeScript != null) // Check if testScript is assigned
        {
            freezeScript.Freezing();
            Debug.Log("All agents frozen for 5 seconds.");
        }
        Debug.Log("ALLOW ME TO BREAK THE ICE");


   }

    public void DoublePoints()
    {


        
        Debug.Log("GET RICH AND CRASH");

    }


    public void SpeedBoost()
    {

       
        Debug.Log("NEED FOR MF SPEED");

    }
}
   
    








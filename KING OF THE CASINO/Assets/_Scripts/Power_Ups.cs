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
    public carController carScript;
    public DoublePoints TimesScript;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int Value = Random.Range(0, 3);

            if (Value == 0)
            {
                SpeedBoost(); //Activate speedboost effect.

            }
            else if (Value == 1 )
            {

                Freeze(); // Activate freeze effect.

            }
            else if (Value == 2)
            {
                DoubleUp();  // Activate double points effect.

            }
            Destroy(gameObject);
        }
    }

    public void Freeze()
    {
        if (freezeScript != null) // Check if the Freeze script is assigned.
        {
            freezeScript.Freezing(); // Call the freezing method in the Freeze script.
            Debug.Log("All agents frozen for 5 seconds.");
        }
        else
        {
            Debug.Log("Freeze script is not assigned.");
        }
    }

    public void DoubleUp()
    {
        if (TimesScript != null)
        {
            TimesScript.ActivateDoublePointsPowerUp(); // Call the method in DoublePoints script.
            Debug.Log("GET RICH AND CRASH");
        }
        else
        {
            Debug.Log("DoublePoints script is not assigned.");
        }
    }

    public void SpeedBoost()
    {
        if (carScript != null)
        {
            carScript.speedBoost();
            Debug.Log("I AM SPEED!");

        }
        else 
        {

            Debug.Log("YOUR NOT VIN DIESEL");
        }



    }

 




}
   
    








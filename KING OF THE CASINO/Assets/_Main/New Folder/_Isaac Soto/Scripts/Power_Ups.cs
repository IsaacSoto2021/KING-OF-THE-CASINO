using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Isaac Soto 
 * 10/24/24
 * [ Power-up script that randomizes power-ups given to the player on pick-up]
 */

public class Power_Ups : MonoBehaviour
{

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
                
                Debug.Log("PowerUp1");
            }
            else if (Value == 1)
            {
                Debug.Log("PowerUp2");


            }
            else if (Value == 2)
            {
                Debug.Log("PowerUp3");
            }
        }

    }

}

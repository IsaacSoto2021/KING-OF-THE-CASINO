using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public int points = 10000;
   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          int Value = Random.Range(0, 5);
          if (Value == 0)
          {   
            points -= 1000;
            Debug.Log("You lose 1000 points");
          }
          else if (Value == 1)
          {
            points -= 2000;
            Debug.Log("You lose 2000 points");
          }
          else if (Value == 2)
          {
            points -= 3000;
            Debug.Log("You lose 3000 points");

          }
          else if (Value == 3)
          {
            points -= 4000;
            Debug.Log("You lose 4000 points");
          }
          else if (Value == 4)
          { 
            points -= 5000;
            Debug.Log("You lose 5000 points");
          }
          else if (Value == 5)
          {
            points -= 6000;
            Debug.Log("You lose 6000 points");
          }

          Destroy(other.gameObject);




        }


        




    }      

            
}
          


         


      
       
           
      
    


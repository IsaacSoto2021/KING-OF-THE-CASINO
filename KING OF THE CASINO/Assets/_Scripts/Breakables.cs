using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public int PlayerMoney = 10000;
   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          int Value = Random.Range(0, 5);
          if (Value == 0)
          {   
                PlayerMoney -= 1000;
            Debug.Log("You lose 1000 points");
          }
          else if (Value == 1)
          {
                PlayerMoney -= 2000;
            Debug.Log("You lose 2000 points");
          }
          else if (Value == 2)
          {
                PlayerMoney -= 3000;
            Debug.Log("You lose 3000 points");

          }
          else if (Value == 3)
          {
                PlayerMoney -= 4000;
            Debug.Log("You lose 4000 points");
          }
          else if (Value == 4)
          {
                PlayerMoney -= 5000;
            Debug.Log("You lose 5000 points");
          }
          else if (Value == 5)
          {
                PlayerMoney -= 6000;
            Debug.Log("You lose 6000 points");
          }

          Destroy(other.gameObject);




        }


        




    }      

            
}
          


         


      
       
           
      
    


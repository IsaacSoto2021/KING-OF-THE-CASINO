using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Ups : MonoBehaviour
{
    public float Coin = 10f;

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
            Debug.Log("You have collected this power-up");
            Destroy(gameObject);
        }

    }

}

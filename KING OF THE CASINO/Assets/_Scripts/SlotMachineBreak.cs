using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineBreak : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }
}

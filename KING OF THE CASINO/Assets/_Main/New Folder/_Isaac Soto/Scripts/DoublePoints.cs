using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePoints : MonoBehaviour
{
    public string slotMachineTag = "SlotMachine"; 
    public float multiplier = 2f; 
    public float effectDuration = 10f; 
    public GameObject Image; 
    private bool isPowerUpActive = false;

    public void ActivateDoublePointsPowerUp()
    {
        if (isPowerUpActive) return; 

        ShowImage();

        
        GameObject[] slotMachines = GameObject.FindGameObjectsWithTag(slotMachineTag);
        foreach (GameObject slotMachineObj in slotMachines)
        {
            Test_Slots slotMachine = slotMachineObj.GetComponent<Test_Slots>();
            if (slotMachine != null)
            {
                slotMachine.isPowerUpActive = true; 
            }
        }

        isPowerUpActive = true;
        StartCoroutine(ResetPowerUpAfterDuration());
    }

    private IEnumerator ResetPowerUpAfterDuration()
    {
        yield return new WaitForSeconds(effectDuration);

       
        GameObject[] slotMachines = GameObject.FindGameObjectsWithTag(slotMachineTag);
        foreach (GameObject slotMachineObj in slotMachines)
        {
            Test_Slots slotMachine = slotMachineObj.GetComponent<Test_Slots>();
            if (slotMachine != null)
            {
                slotMachine.isPowerUpActive = false; 
            }
        }

        isPowerUpActive = false;
    }

    private void ShowImage()
    {
        if (Image != null)
        {
            Image.SetActive(true);
            StartCoroutine(HideImageAfterDelay(10f));
        }
    }

    private IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Image != null)
        {
            Image.SetActive(false);
        }
    }
}

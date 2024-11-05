using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Isaac Soto
 * 11/4/25
 * [ When the player activated a Power_up and gets the freeze all agents in the game will no longer move until the timer is run out]
 */

public class Freeze : MonoBehaviour
{
    public string targettag = "Enemy";
    public float Duration = 5f;
    private bool isFrozen = false;


    // Set the image so when the image pop-ups up when the freeze is activated
    public GameObject Image;


    // Start is called before the first frame update
    private void Start()
    {
        if (Image == null)
        {
            Image.SetActive(false);
        }

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isFrozen)
        {

            Freezing();
        }
    }

    public void Freezing()
    {
        NavAgentsTargeted();
        ShowImage();
        StartCoroutine(NavAgentsUnTargeted());


    }

    private void NavAgentsTargeted()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targettag);
        foreach (GameObject obj in targetObjects)
        {
            UnityEngine.AI.NavMeshAgent agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
            }

        }
        isFrozen = true;
    }

    private IEnumerator NavAgentsUnTargeted()
    {
        yield return new WaitForSeconds(Duration);

        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targettag);

        foreach (GameObject obj in targetObjects)
        {
            UnityEngine.AI.NavMeshAgent agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = false; 
            }
        }
        isFrozen = false;
    }
   
    private void ShowImage()
    {
        if(Image != null)
        {
            Image.SetActive(true);
            StartCoroutine(HiddenAfterImage(5f));
        }
    }

    private IEnumerator HiddenAfterImage( float delay)
    {
        yield return new WaitForSeconds(delay);
        if(Image != null)
        {
            Image.SetActive(false);
        }
    }
}
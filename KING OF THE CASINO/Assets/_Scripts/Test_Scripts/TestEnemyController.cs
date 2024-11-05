using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;


public class TestEnemyController : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
      
    }

    void Update()
    {
        agent.destination = player.position;

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            RetryGame();
        }

    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
   
}





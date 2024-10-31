using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class TestEnemyController : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent <NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Player.position;
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





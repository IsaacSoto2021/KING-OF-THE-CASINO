using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for spawning enemies on platforms at start of game
//10/22/2024
//Austin, Robert

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab for unity inspector
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {

        // spawns enemy at set coordinates (Quaternion = no rotation on spawn)
        // this = parent object of script
        Instantiate(Enemy,(this.transform.position), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

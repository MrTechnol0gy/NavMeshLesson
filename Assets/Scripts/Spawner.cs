using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnedEnemy;                       // enemy prefab to be spawned
    [SerializeField] GameObject enemyContainer;                     // empty parent for all spawned enemies
    [SerializeField] Transform spawnpoint;                          // where to spawn
    [SerializeField] int amtToSpawn;                                // amount of enemies to spawn    
    private List<GameObject> allSpawns = new List<GameObject>();    
        
    
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < amtToSpawn; i++)
        {       
            GameObject newEnemy = Instantiate(spawnedEnemy, spawnpoint.position, Quaternion.identity);    // new enemy spawn
            newEnemy.transform.localScale = new Vector3(1,1,1);
            allSpawns.Add(newEnemy);                                                                // adds the newly spawned enemy to the list of spawns            
            newEnemy.transform.parent = enemyContainer.transform; 
            yield return new WaitForSeconds(1.0f);                                                  // time to wait between spawns
        }
    }   
}

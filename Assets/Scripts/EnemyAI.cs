using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent enemy;    
    private GameObject playerAgent;
    private Vector3 playerPOS;
    private Vector3 oldPlayerPOS;
    private Vector3 enemyPOS;
    private float enemyToPlayerDistance; 
    [SerializeField] GameObject patrolPointOne;
    [SerializeField] GameObject patrolPointTwo;
    [SerializeField] GameObject patrolPointThree;
    [SerializeField] GameObject patrolPointFour;
    [SerializeField] GameObject enemyHomeBase;
    [SerializeField] Material patrolMaterial;
    [SerializeField] Material attackMaterial;
    [SerializeField] Material chaseMaterial;
    [SerializeField] Material retreatMaterial;
    [SerializeField] Material searchMaterial;
    
    enum EnemyState
    {
        patrolling,     // patrolling = 0
        chasing,        // chasing = 1
        searching,      // searching = 2
        attacking,      // attacking = 3
        retreating      // retreating = 4
    }

    enum PatrolRounds
    {
        pointOne,       // patrolPointOne = 0
        pointTwo,       // patrolPointTwo = 1
        pointThree,     // patrolPointThree = 2
        pointFour       // patrolPointFour = 3
    }

    static EnemyState enemystate = EnemyState.patrolling;       //sets the starting enemy state
    static PatrolRounds patrolRounds = PatrolRounds.pointOne;   //sets the starting enemy patrol location

    void Start()
    {
        playerAgent = GameObject.FindGameObjectWithTag("Player"); //gets the player gameobject        
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        EnemyUpdate();
        Debug.Log("Distance to player is: " + enemyToPlayerDistance);
    }

    public void EnemyUpdate()
    {
        StateUpdate();                  //updates the enemy state        
        StateProceed();                 //enemy actions proceed based on state and speed
    }
    
    public void StateUpdate()           // compares enemy distance to player and sets the state    
    {
        GetEnemyToPlayerDistance();     // gets the distance between the enemy and the player
        switch (enemyToPlayerDistance)
        {
            case > 25:
            enemystate = EnemyState.patrolling;    //sets the enemy state to patrolling
            enemy.GetComponent<MeshRenderer>().material = patrolMaterial;   //sets the enemy material to the patrol material
            break;
            case > 20:
            enemystate = EnemyState.searching;
            enemy.GetComponent<MeshRenderer>().material = searchMaterial;
            break;
            case > 15:
            enemystate = EnemyState.chasing;
            enemy.GetComponent<MeshRenderer>().material = chaseMaterial;
            break;
            case >= 10:
            enemystate = EnemyState.attacking;
            enemy.GetComponent<MeshRenderer>().material = attackMaterial;
            break;
            case < 10:
            enemystate = EnemyState.retreating;
            enemy.GetComponent<MeshRenderer>().material = retreatMaterial;
            break;
            default:
            break;
        }
    }
    private void StateProceed()         // enemy ai will proceed based on current state
    {
        switch (enemystate)             // utilizes the distance between enemy and player to determine state
        {
            case EnemyState.patrolling:
            enemy.isStopped = false;
                switch (patrolRounds)
                {
                    case PatrolRounds.pointOne:
                    enemy.destination = patrolPointOne.transform.position;
                    if (Vector3.Distance(enemyPOS, patrolPointOne.transform.position) < 5)
                    {
                        patrolRounds = PatrolRounds.pointTwo;
                    }
                    break;
                    case PatrolRounds.pointTwo:
                    enemy.destination = patrolPointTwo.transform.position;
                    if (Vector3.Distance(enemyPOS, patrolPointTwo.transform.position) < 5)
                    {
                        patrolRounds = PatrolRounds.pointThree;
                    }
                    break;
                    case PatrolRounds.pointThree:
                    enemy.destination = patrolPointThree.transform.position;
                    if (Vector3.Distance(enemyPOS, patrolPointThree.transform.position) < 5)
                    {
                        patrolRounds = PatrolRounds.pointFour;
                    }
                    break;
                    case PatrolRounds.pointFour:
                    enemy.destination = patrolPointFour.transform.position;
                    if (Vector3.Distance(enemyPOS, patrolPointFour.transform.position) < 5)
                    {
                        patrolRounds = PatrolRounds.pointOne;
                    }
                    break;
                    default:
                    break;
                }
            break;
            case EnemyState.chasing:
            enemy.destination = playerPOS;
            oldPlayerPOS = playerPOS;
            enemy.isStopped = false;
            break;
            case EnemyState.searching:
            enemy.destination = oldPlayerPOS;
            enemy.isStopped = false;
            break;
            case EnemyState.attacking:
            enemy.isStopped = true;
            break;
            case EnemyState.retreating:
            enemy.destination = enemyHomeBase.transform.position;
            enemy.isStopped = false;
            if (Vector3.Distance(enemyPOS, enemyHomeBase.transform.position) < 5)
            {                
                enemystate = EnemyState.patrolling;
            }
            break;
            default:
            break;
        }
    }

    public void GetEnemyToPlayerDistance()
    {
        playerPOS = playerAgent.transform.position;                     //gets player's pos as a vector 3
        enemyPOS = this.transform.position;                             //gets the enemy's pos as a vector 3
        enemyToPlayerDistance = Vector3.Distance(enemyPOS, playerPOS);  //compares the difference between the enemy position and the player position
    }
}

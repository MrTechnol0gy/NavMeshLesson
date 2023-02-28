using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent enemy;    
    private GameObject playerAgent;
    private Vector3 playerPOS;
    private Vector3 enemyPOS;
    private float enemyToPlayerDistance;

    [SerializeField] GameObject patrolPointOne;
    [SerializeField] GameObject patrolPointTwo;
    [SerializeField] GameObject patrolPointThree;
    [SerializeField] GameObject patrolPointFour;
    
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
        StateUpdate();
        StateProceed();             
    }

    public void StateUpdate()           // compares enemy distance to player and sets the state    
    {
        GetEnemyToPlayerDistance();     // gets the distance between the enemy and the player
    }
    private void StateProceed()         // enemy ai will proceed based on current state
    {

        switch (enemystate)             // utilizes the distance between enemy and player to determine state
        {
            case EnemyState.patrolling:
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
            break;
            case EnemyState.searching:
            break;
            case EnemyState.attacking:
            break;
            case EnemyState.retreating:
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

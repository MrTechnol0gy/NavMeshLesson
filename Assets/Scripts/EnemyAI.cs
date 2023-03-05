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
    private float closeProximity = 5;
    private float distantProximity = 25;
    private bool playerVisible = false;
    private bool wallHit = false;
    public LayerMask wallLayer;
    [SerializeField] Collider playerCollider;
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
    
    void FixedUpdate()
    {   
        GetEnemyToPlayerDistance();     // gets the distance between the enemy and the player
        EnemyUpdate();
        //Debug.Log("Distance to player is: " + enemyToPlayerDistance);
    }

    public void EnemyUpdate()
    {
        StateUpdate();                  //updates the enemy state        
        StateProceed();                 //enemy actions proceed based on state
    }
    
    public void StateUpdate()           // checks for conditions that would cause the enemy state to change
    {        
        switch (enemystate)
        {
            case EnemyState.patrolling:
                enemy.GetComponent<MeshRenderer>().material = patrolMaterial;   // sets the enemy material to the patrol material
                TargetVisibleCheck();                                           // checks to see if the player is currently visible
                if (playerVisible)                                              // if the player is within range and visible the enemy will move to chasing state
                {
                    enemystate = EnemyState.chasing;
                }
                else
                {
                    enemystate = EnemyState.patrolling;
                }
                break;
            case EnemyState.searching:            
                enemy.GetComponent<MeshRenderer>().material = searchMaterial;
                TargetVisibleCheck();                                           // checks to see if it can see the player
                if (playerVisible)
                {
                    enemystate = EnemyState.chasing;
                }
                else if ((Vector3.Distance(enemyPOS, oldPlayerPOS) < closeProximity))// once the enemy agent arrives at the last seen position
                {
                    TargetVisibleCheck();
                    if (playerVisible)
                    {   
                        enemystate = EnemyState.chasing;                        // switches to chasing if the player is visible
                    }
                    else 
                    {
                        enemystate = EnemyState.patrolling;                     // switches to patrolling if the player isn't visible
                    }
                }
                break;
            case EnemyState.chasing:            
                enemy.GetComponent<MeshRenderer>().material = chaseMaterial;
                TargetVisibleCheck();
                if (playerVisible && enemyToPlayerDistance <= 10) 
                {
                    enemystate = EnemyState.attacking;                    
                }         
                else if (!playerVisible)
                {
                    enemystate = EnemyState.searching;                    
                }                       
                break;
            case EnemyState.attacking:            
                enemy.GetComponent<MeshRenderer>().material = attackMaterial;
                TargetVisibleCheck();
                if (enemyToPlayerDistance < closeProximity)
                {
                    enemystate = EnemyState.retreating;
                }
                else if (!playerVisible)
                {
                    enemystate = EnemyState.searching;
                }
                else if (playerVisible == true && enemyToPlayerDistance > 10)
                {
                    enemystate = EnemyState.chasing;
                }
                else
                {
                    enemystate = EnemyState.attacking;
                }
                break;
            case EnemyState.retreating:            
                enemy.GetComponent<MeshRenderer>().material = retreatMaterial;
                if ((Vector3.Distance(enemyPOS, enemyHomeBase.transform.position) < closeProximity))
                {
                    enemystate = EnemyState.patrolling;
                }                
                break;
            default:
            break;
        }
    }
    private void StateProceed()         // enemy ai will proceed based on current state
    {
        switch (enemystate)             // actions based on the state determined in StateUpdate
        {
            case EnemyState.patrolling:
            enemy.isStopped = false;
                switch (patrolRounds)
                {
                    case PatrolRounds.pointOne:
                        enemy.destination = patrolPointOne.transform.position;
                        if (Vector3.Distance(enemyPOS, patrolPointOne.transform.position) < closeProximity)
                        {
                            patrolRounds = PatrolRounds.pointTwo;
                        }
                        break;
                    case PatrolRounds.pointTwo:
                        enemy.destination = patrolPointTwo.transform.position;
                        if (Vector3.Distance(enemyPOS, patrolPointTwo.transform.position) < closeProximity)
                        {
                            patrolRounds = PatrolRounds.pointThree;
                        }
                        break;
                    case PatrolRounds.pointThree:
                        enemy.destination = patrolPointThree.transform.position;
                        if (Vector3.Distance(enemyPOS, patrolPointThree.transform.position) < closeProximity)
                        {
                            patrolRounds = PatrolRounds.pointFour;
                        }
                        break;
                    case PatrolRounds.pointFour:
                        enemy.destination = patrolPointFour.transform.position;
                        if (Vector3.Distance(enemyPOS, patrolPointFour.transform.position) < closeProximity)
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

    public void TargetVisibleCheck()
    {        
        Ray ray = new Ray (enemyPOS, playerPOS - enemyPOS);                         //casts a ray from the enemy agent towards the player's position 
        Debug.DrawRay(enemyPOS, (playerPOS - enemyPOS) * 10);                       // visualizes the raycast for debugging
        RaycastHit hitData;
        wallHit = false;
        playerVisible = false;
        if (Physics.Raycast(ray, out hitData, enemyToPlayerDistance, wallLayer))    //checks for walls between the player and the enemy
        {
            wallHit = true;
            Debug.Log("Wall has been hit.");
        }        
        else
        {
            wallHit = false;
            Debug.Log("wall has not been hit.");

            if (playerCollider.Raycast(ray, out hitData, distantProximity))         // checks for the player's visibility within "sight" range
            {
                playerVisible = true;
                Debug.Log("Player is visible.");
            }
            else
            {
                playerVisible = false;
                Debug.Log("Player is not visible.");
            }
        }
    }
}

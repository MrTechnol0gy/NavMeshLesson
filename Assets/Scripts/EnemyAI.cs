using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent enemy;
    
    enum EnemyState
    {
        patrolling,     // patrolling = 0
        chasing,        // chasing = 1
        searching,      // searching = 2
        attacking,      // attacking = 3
        retreating      // retreating = 4
    }

    static EnemyState enemystate = EnemyState.patrolling;

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
    }

    public void EnemyUpdate()
    {
        switch (enemystate)
        {
            case EnemyState.patrolling:
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
}

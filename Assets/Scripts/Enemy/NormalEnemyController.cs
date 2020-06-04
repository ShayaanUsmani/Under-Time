using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine;

public class NormalEnemyController : MonoBehaviour
{
    public Transform playerTransform;

    private Animator animator;

    public Transform gun;
    public Transform bulletPrefab;

    private Vector3 startPos;
    private Quaternion startRot;
    private float distFromPlayer;

    /*
    private string botLayerName = "Bot";
    private bool isBot;
    */

    private float lookAtSmoothness = 3f;
    private float aggroRange = 5.4f;
    private float stopRange = 3f;
    private float fieldOfViewAngle = 55f;
    public bool isAggrod;
    private bool wasAggrod;

    public Image aggroIndicatorImage;

    private NavMeshAgent agent;
    public LayerMask environmentLayer;
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        aggroIndicatorImage.enabled = false;
        agent = gameObject.GetComponent<NavMeshAgent>();
        startPos = transform.position;
        startRot = transform.rotation;
        agent.stoppingDistance = stopRange;
        animator.SetBool("isIdle", true);
        /*
        if ((gameObject.layer == LayerMask.NameToLayer(botLayerName)))
        {
            isBot = true;
        }
        */
    }
    
    // check distance from player
    // check if should be aggrod,
    // if so then provide conditions for the proper animations
    // if the player is too far from the enemy, make the enemy follow the player
    // also show the player the enemy is aggrod via UI
    // and make wasAggrod true since enemy was just aggrod
    
    void Update()
    {
        distFromPlayer = Vector3.Distance(playerTransform.position, transform.position);
        isAggrod = CheckAggro();
        if(isAggrod)
        {
            animator.SetBool("isShooting", true);
            animator.SetBool("isReturning", false);
            animator.SetBool("isIdle", false);

            if(distFromPlayer > stopRange)
            {
                animator.SetBool("isFollowingPlayer", true);
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
                animator.SetBool("isFollowingPlayer", false);
            }

            aggroIndicatorImage.enabled = true;
            wasAggrod = true;
        }

        /*
        // if the enemy is not aggrod, check if it was aggrod before.
        // if it was then disable the UI telling the player it is aggrod
        // make it look towards where it was before aggro, and head towards there
        // when/if the enemy arrives there, make the enemy look at where it was originally
        // looking and make wasAggrod to false for next time.
        else
        {
            
            if (wasAggrod)
            {
                animator.SetBool("isReturning", true);
                animator.SetBool("isShooting", false);
                animator.SetBool("isFollowingPlayer", false);

                aggroIndicatorImage.enabled = false;

                
                transform.LookAt(startPos);
                agent.SetDestination(startPos);

                wasAggrod = false;
                
            }
            if (transform.position == startPos)
                {
                    transform.rotation = startRot;
                    wasAggrod = false;
                    animator.SetBool("isReturning", false);
                    animator.SetBool("isShooting", false);
                    animator.SetBool("isFollowingPlayer", false);
                    animator.SetBool("isIdle", true);
                }
        }
        */
        
    }

    // if the player is in the aggro range and the player is in a field of view 
    // range (checked by finding the angle between the local forward of the enemy
    // and the position of the player), look at the player and return true.
    private bool CheckAggro()
    {
        if(distFromPlayer <= aggroRange)
        {
            Vector3 lookAtPoint = (playerTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, lookAtPoint) <= fieldOfViewAngle)
            {
                lookAtPoint.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPoint), lookAtSmoothness * Time.unscaledDeltaTime);
                
                return true;
            }
        }
        /*
        else
        {
            agent.SetDestination(transform.position);

            return false;
        }
        */

        return false;
    }

    // returns true if there is nothing between the player and the enemy
    public bool CanSeePlayer()
    {
        if (!Physics.Linecast(transform.position, playerTransform.position, environmentLayer))
        {
            return true;
        }

        return false;
    }
}

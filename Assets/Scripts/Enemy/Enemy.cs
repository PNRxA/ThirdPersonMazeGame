using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    public Menu menu;
    public Animator playerAnimator;
    private Animator anim;
    private bool attacking;
    private bool stunned = false;

    void Awake()
    {
        // Set navmesh agent 
        agent = GetComponent<NavMeshAgent>();
        // Get menu script
        menu = FindObjectOfType<Menu>();
        // Set the character animator
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;

        // If agent has reaches the player
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //menu.gameOver = true;
                    // Attack the player if he's alive and cooldown for a few seconds
                    if (attacking == false && Menu.health > 0 && stunned == false)
                    {
                        // Enemy is attacking
                        attacking = true;
                        // Attack function
                        Attack();
                        // Stop moving till after attack
                        if (agent.isActiveAndEnabled)
                        {
                            agent.Stop();
                        }
                    }
                }
            }
        }
        //If the agent is moving then show the moving animation
        if (agent.velocity.magnitude > Vector3.zero.magnitude)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    void Attack()
    {
        if (attacking == true)
        {
            // Play attack animation
            anim.SetTrigger("attack");
            // Damage player
            Menu.health--;
            // Slight delay then player hurt animation
            Invoke("HurtPlayer", .5f);
            // Resume moving after a short delay
            Invoke("EndAttack", 2);
        }
    }

    void HurtPlayer()
    {
        // Player hurt animation on player
        if (playerAnimator.isActiveAndEnabled)
        {
            playerAnimator.SetTrigger("hurt");
        }
    }

    void EndAttack()
    {
        // End attack and resume moving
        attacking = false;
        if (agent.isActiveAndEnabled)
        {
            agent.Resume();
        }
    }

    // Enter stunned state and unstun after 2 seconds
    public void StunAgent()
    {
        stunned = true;
        if (agent.isActiveAndEnabled)
        {
            agent.Stop();
        }
        Invoke("UnstunAgent", 5);
        if (anim.isActiveAndEnabled)
        {
            anim.SetTrigger("hurt");
            anim.SetBool("stunned", true);
        }
    }

    //Leave the stunned state
    public void UnstunAgent()
    {
        stunned = false;
        if (anim.isActiveAndEnabled)
        {
            anim.SetBool("stunned", false);
        }
        if (agent.isActiveAndEnabled)
        {
            agent.Resume();
        }
    }
}

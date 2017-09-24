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

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //menu.gameOver = true;
                    if (attacking == false && Menu.health > 0)
                    {
                        attacking = true;
                        Attack();
                        agent.Stop();
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
            anim.SetTrigger("attack");
            playerAnimator.SetTrigger("hurt");
            Menu.health--;
            Invoke("EndAttack", 2);
        }
    }

    void EndAttack()
    {
        attacking = false;
        agent.Resume();
    }
}

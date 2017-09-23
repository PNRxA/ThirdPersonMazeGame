using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    public Menu menu;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        menu = FindObjectOfType<Menu>();
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
                    menu.gameOver = true;
                }
            }
        }
    }
}

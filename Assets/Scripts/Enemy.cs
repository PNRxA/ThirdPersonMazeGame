using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();
	public Animator enemyAnim;
    public Enemy enemy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		Attack();
    }

    //Attack and trigger the enemy's stunned animation
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
			if (targets.Count > 0)
			{
				enemy.StunAgent();
			}
        }
    }

    // Add the enemy to a list when they enter the attack range
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targets.Add(other.gameObject);
        }
    }

    // Remove the enemy when they exit attack range
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targets.Remove(other.gameObject);
        }
    }
}

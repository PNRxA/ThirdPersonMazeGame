using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public Menu menu;
    // Use this for initialization
    void Start()
    {
        menu = FindObjectOfType<Menu>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //If the player enters this trigger, they win the game
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			menu.win = true;
	        menu.gameOver = true;
        }
    }
}

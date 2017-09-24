using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool controllable = true;
    public float speed = 7.0f;
    public float jumpSpeed = 6.0f;
    public float gravity = 20.0f;
    public Animator enemyAnimator;

    private Menu menu;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
    }

    void Awake()
    {
        // Set menu
        menu = FindObjectOfType<Menu>();
        // Set character controller 
        controller = GetComponent<CharacterController>();
        // Set the character animator
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // movement functions
        Movement();
        // allow player to attack
        Attack();
        // check if dead
        CheckDeath();
    }

    void CheckDeath()
    {
        // If there is no health left, kill off the player
        if (Menu.health <= 0)
        {
            if (isDead == false)
            {
                anim.SetBool("isDead", true);
                Invoke("GameOver", 5f);
            }
            isDead = true;
        }
        if (Menu.health > 0)
        {
            isDead = false;
        }
    }

    void GameOver()
    {
        menu.gameOver = true;
    }

    void Movement()
    {
        // Move character based on axis movement
        if (controller.isGrounded && controllable && Menu.health > 0)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                anim.SetTrigger("jump");
            }

        }

        if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") > 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

    }

    void Attack()
    {
        // Play attack animation if the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }
}
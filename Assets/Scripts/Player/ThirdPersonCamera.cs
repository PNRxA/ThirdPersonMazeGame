using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Camera cam;

    public bool locked = false;

    void FixedUpdate()
    {
        //If the camera is not locked, allow player to move it
        if (!locked)
        {
            cam = GetComponent<Camera>();
            Rotation();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Rotation()
    {
        // Move camera based on getAxis
        yaw += speedH * Input.GetAxis("Mouse X");
        // Lock camera so you can't 360
        pitch = Mathf.Min(80, Mathf.Max(-80, pitch + -Input.GetAxis("Mouse Y")));
        // Rotate parent with camera if alive
        if (Menu.health > 0)
        {
            transform.parent.eulerAngles = new Vector3(0, yaw, 0);
        }
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
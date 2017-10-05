using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    bool doorDown = true;
    Vector3 startPos;
    Vector3 endPos;
    float raiseHeight = 3f;
    float speed = 3f;

    void Awake()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, raiseHeight, transform.position.z);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.health <= 0)
        {
            StartCoroutine("MoveDoor", startPos);
        }
    }

    //Function to trigger doors to close
    public void Close()
    {
        StartCoroutine("MoveDoor", startPos);
    }

    //Open door when player enters trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            StartCoroutine("MoveDoor", endPos);
        }
    }

    //Slerp door transform from current to specified location
    IEnumerator MoveDoor(Vector3 endPos)
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        //Move door to endPos from current position (startpos)
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Slerp(startPos, endPos, t);
            yield return null;
        }
    }
}

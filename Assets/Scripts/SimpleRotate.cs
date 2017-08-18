using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
	public float xrot, yrot, zrot;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.Rotate(new Vector3(xrot,yrot,zrot));
    }
}

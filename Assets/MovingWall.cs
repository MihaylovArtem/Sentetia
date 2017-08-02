﻿using System.Security.Cryptography;
using UnityEngine;
using System.Collections;


public class MovingWall : MonoBehaviour
{
    public static bool isActivated;
	// Use this for initialization
	void Start ()
	{
	    isActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (isActivated)
	    {
            Destroy(gameObject,15);
	        if (!gameObject.GetComponent<AudioSource>().isPlaying)
	        {
	            gameObject.GetComponent<AudioSource>().Play(0);
	        }
	        gameObject.transform.position -= new Vector3(Time.deltaTime/3, 0, 0);
	    }
	}

    void OnCollisionEnter (Collision other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "ExternalWall") {
            Destroy(gameObject);
        }

    }
}
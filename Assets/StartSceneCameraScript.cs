using UnityEngine;
using System.Collections;

public class StartSceneCameraScript : MonoBehaviour {

    private float timer = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    timer -= Time.deltaTime;
        if (timer <= 7)
        {
            Camera.main.fieldOfView += Time.deltaTime*10;
        }
        if (timer <= 0) {
            Camera.main.nearClipPlane = 1500;
        }
        if (timer <= -5)
        {
            foreach (var audioObject in GameObject.FindGameObjectsWithTag("Audio Source"))
            {
                audioObject.GetComponent<AudioSource>().Stop();
            }
	        Application.LoadLevel("main");
        }
	}
}

using UnityEngine;
using System.Collections;

public class EndSceneGoodMusic : MonoBehaviour {

    [SerializeField]
    private AudioClip closingDoor;
    [SerializeField]
    private AudioClip birds;

    private AudioSource source;


	// Use this for initialization
	void Start () {
	    source = GetComponent<AudioSource>();
	    source.clip = closingDoor;
        source.Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!source.isPlaying) {
	        source.clip = birds;
            source.Play();
	    }
	}
}

using UnityEngine;
using System.Collections;

public class TriggerWithSound : MonoBehaviour {
    [SerializeField]
    private bool repeatable;
    [SerializeField]
    private bool isActivated;
    [SerializeField] private AudioClip sound;  
    private AudioSource source;

	// Use this for initialization
	void Start () {
	    source = GetComponent<AudioSource>();
	    source.clip = sound;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (repeatable)
            {
                source.Play(0);
            }
            else if (!isActivated)
            {
                isActivated = true;
                source.Play(0);
                if (name == "ArrowPlatform")
                {
                    Invoke("InvokeArrowTrap", 1.5f);
                }
            }
        }
    }

    void InvokeArrowTrap() {
        GameObject arrowTrap = GameObject.Find("ArrowTrap");
        arrowTrap.GetComponent<AudioSource>().Play();
        arrowTrap.GetComponent<BoxCollider>().enabled = true;
        Destroy(arrowTrap, 3f);
    }
}

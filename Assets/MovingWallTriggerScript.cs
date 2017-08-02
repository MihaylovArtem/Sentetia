using UnityEngine;
using System.Collections;

public class MovingWallTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.name == "MovingWallTrigger")
            {
                GameObject.Find("MovingWallDoor").GetComponent<BoxCollider>().enabled = true;
                MovingWall.isActivated = true;
                GameObject.Find("MovingWallDoor").GetComponent<AudioSource>().clip =
                    GameObject.Find("MovingWallDoor").GetComponent<DoorScript>().m_DoorClosedSound;
                GameObject.Find("MovingWallDoor").GetComponent<AudioSource>().Play();
                Destroy(gameObject);
            }
            else if (gameObject.name == "MovingBallTrigger")
            {
                GameObject.Find("MovingBall").GetComponent<BoxCollider>().enabled = true;
                MovingBall.isActivated = true;
                GameObject.Find("MovingBall").GetComponent<AudioSource>().Play();
                Destroy(gameObject);
                
            }
        }

    }
}

using UnityEngine;
using System.Collections;

public class FinalRoomMonk : MonoBehaviour
{

    [SerializeField] private AudioClip m_DrinkIt;
    [SerializeField]
    private AudioClip m_ComeHere;
    private AudioSource source;

	// Use this for initialization
	void Start ()
	{
	    source = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            source.clip = m_DrinkIt;
            source.Play();
        }
    }

    private void OnTrigger(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space))
        {
            source.Stop();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            source.clip = m_ComeHere;
            source.Play();
        }
    }
}

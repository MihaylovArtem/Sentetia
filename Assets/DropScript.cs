using System.Threading;
using UnityEngine;
using System.Collections;

public class DropScript : MonoBehaviour
{

    private double timer = 0;
    private double nextDropTime = 0;
    [SerializeField] private float minNextDropTime;
    [SerializeField] private float maxNextDropTime;
    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;
    public bool isRandomPitch=true;

	// Use this for initialization
	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = sound;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    timer += Time.deltaTime;
	    if (timer > nextDropTime)
	    {
            if (isRandomPitch) audioSource.pitch = Random.Range(0.6f, 1.1f);
            audioSource.Play(0);
            nextDropTime = Random.Range(minNextDropTime, maxNextDropTime);
	        timer = 0;
	    }
	}
}

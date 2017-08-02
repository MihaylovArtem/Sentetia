using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ChainsScript : MonoBehaviour {

    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip[] m_ChainSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]
    private AudioClip m_ChainFallingSound;    // an array of footstep sounds that will be randomly selected from.

    private bool isChained = true;

	// Use this for initialization
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isChained)
	    {
            if (Input.GetButtonDown("Jump"))
            {
                m_AudioSource.clip = m_ChainFallingSound;
                m_AudioSource.Play(0);
                isChained = false;
                Debug.Log("Sound");
            }
            else
            {
                PlayChainAudio();
                
            }
	    }
	}
    private void PlayChainAudio()
    {
        if ((CrossPlatformInputManager.GetAxis("Vertical") == 0 && CrossPlatformInputManager.GetAxis("Horizontal") == 0) || m_AudioSource.isPlaying)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        m_AudioSource.clip = m_ChainSounds[0];
        m_AudioSource.pitch = Random.Range(0.8f, 1.0f);
        m_AudioSource.Play(0);
        // move picked sound to index 0 so it's not picked next time
        m_ChainSounds[0] = m_AudioSource.clip;
    }
}

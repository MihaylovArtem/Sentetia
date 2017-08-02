using UnityEngine;
using System.Collections;

public class EndSceneCameraScript : MonoBehaviour {

    public static bool  isCloseUpStarted = false;
    private float timer = 10.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 5) {
            isCloseUpStarted = true;
            gameObject.transform.localRotation = new Quaternion(0/360,10/360,0,0);
            Camera.main.fieldOfView -= Time.deltaTime * 10;
        }
        if (timer <= 0)
        {
            Camera.main.nearClipPlane = 1500;
        }
        if (timer <= -5)
        {
            foreach (var audioObject in GameObject.FindGameObjectsWithTag("Audio Source"))
            {
                audioObject.GetComponent<AudioSource>().Stop();
            }
            isCloseUpStarted = false;
            Application.LoadLevel("Main Menu");
        }
    }
}

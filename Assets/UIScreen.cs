using UnityEngine;
using System.Collections;

public class UIScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void OnGUI() {
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
	        Application.Quit();
	    }
        else if (Event.current.type == EventType.KeyDown)
        {
            Application.LoadLevel("start_scene");
        }
}
    
}

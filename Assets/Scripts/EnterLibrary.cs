using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLibrary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.tag == "Door")
        {
            SceneManager.LoadScene(1);
        }
    }
}

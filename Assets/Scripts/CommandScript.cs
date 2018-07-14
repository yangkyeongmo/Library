using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour {

    public GameObject commandLine;
    public Text commandText;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.F11)){
            if (!commandLine.activeSelf)
            {
                commandLine.SetActive(true);
                DealWithCommand();
            }
            else
            {
                commandLine.SetActive(false);
            }
        }
	}

    void DealWithCommand()
    {

    }
}

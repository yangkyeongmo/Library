using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour {

    public GameObject commandLine;
    public Text commandText;
    public Camera firstPersonCam;

    private bool isFrozen = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F11)){
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
        if (isFrozen)
        {
            FreezeScreen();
        }
	}

    void DealWithCommand()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (commandText.text.Length != 0)
                {
                    commandText.text = commandText.text.Substring(0, commandText.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                print("User entered their name: " + commandText.text);
                if(commandText.text == "freeze screen")
                {
                    isFrozen = true;
                }
                else if(commandText.text == "defrost")
                {
                    isFrozen = false;
                }
            }
            else
            {
                commandText.text += c;
            }
        }
    }

    void FreezeScreen()
    {
        Vector3 firstPosition = firstPersonCam.transform.position;
        Quaternion firstRotation = firstPersonCam.transform.rotation;
        firstPersonCam.transform.position = firstPosition;
        firstPersonCam.transform.rotation = firstRotation;
    }
}

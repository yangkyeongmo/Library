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
                    FreezeScreen();
                }
            }
            else
            {
                commandText.text += c;
            }
        }
    }
}

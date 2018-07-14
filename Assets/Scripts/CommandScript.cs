using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour {

    public GameObject commandLine;
    public Text commandText;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsCtrl;

    private bool isFPSDeactivated = false;
    private bool isBlinkCoroutineStarted = false;
    private GameObject colon;

	// Use this for initialization
	void Start () {
        colon = commandLine.transform.Find("BlinkColon").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F11)){
            if (!commandLine.activeSelf)
            {
                commandLine.SetActive(true);
            }
            else
            {
                commandLine.SetActive(false);
            }
        }
        if (commandLine.activeSelf)
        {
            if (!isBlinkCoroutineStarted)
            {
                StartCoroutine("Blink");
            }
            DealWithCommand();
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
                if(commandText.text == "deactivate FPS")
                {
                    fpsCtrl.enabled = false;
                }
                else if(commandText.text == "activate FPS")
                {
                    fpsCtrl.enabled = true;
                }
                commandText.text = "";
            }
            else
            {
                commandText.text += c;
            }
        }
    }

    IEnumerator Blink()
    {
        isBlinkCoroutineStarted = true;
        print("Blinking");
        if (colon.activeSelf) { colon.SetActive(false); }
        else { colon.SetActive(true); }
        yield return new WaitForSeconds(1.0f);
        StopCoroutine("Blink");
        isBlinkCoroutineStarted = false;
    }
}

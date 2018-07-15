using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryControll : MonoBehaviour {

    public List<string> dialoguelist;
    public string password;
    public float openSpeed;

    //private GameObject passwordBox;
    private TextMesh currentPassword;
    private GameObject dialogueBubble;
    //private GameObject entryBox;
    private TextMesh dialogue;
    private bool correct;
    private string userInput = "";
    private GameObject door_l;
    private GameObject door_r;
    private CloseGate cg;

    private bool isOpening = false;
    //private bool isClosing = false;
    private bool isInTheBox = false;

    OpeningBook ob;
    int number;

    // Use this for initialization
    void Start ()
    {
        cg = GameObject.Find("EntryBorder").GetComponent<CloseGate>();
        //passwordBox = GameObject.Find("PasswordBox");
        currentPassword = GameObject.Find("CurrentPassword").GetComponent<TextMesh>();
        dialogueBubble = GameObject.Find("dialoguebubble");
        //entryBox = GameObject.Find("EntryBox");
        door_l = GameObject.Find("EntryFrontWall_L");
        door_r = GameObject.Find("EntryFrontWall_R");

        dialogue = dialogueBubble.GetComponentInChildren<TextMesh>();
        dialogue.text = "Hello";
        StartCoroutine(ChangeDialogue());
	}
	
	// Update is called once per frame
	void Update () {
        GetPassword();
        currentPassword.text = userInput;
    }

    void GetPassword()
    {
        if (int.TryParse(Input.inputString, out number))
        {
            if (userInput.Length < 4)
                userInput += Input.inputString;
            else if(userInput.Length >= 4)
                userInput = Input.inputString;
            Debug.Log(userInput);
        }

        if (userInput.Contains(password))
        {
            isOpening = true;
            OpenGate();
        }

        if (cg.GetCloseDoor() || isInTheBox == false)
        {
            CloseGate();
        }
    }

    void OpenGate()
    {
        if (isOpening)
        {
            if (door_l.transform.position.x > -2.65f)
            {
                door_l.transform.position += Vector3.left * openSpeed;
                door_r.transform.position += Vector3.right * openSpeed;
            }
            else if (door_l.transform.position.x <= -2.65)
            {
                isOpening = false;
                userInput = "";
            }
        }
    }

    void CloseGate()
    {
        if(isOpening == false)
        {
            if (door_l.transform.position.x < -1.25f)
            {
                door_l.transform.position -= Vector3.left * openSpeed;
                door_r.transform.position -= Vector3.right * openSpeed;
            }
            else if (door_l.transform.position.x >= -1.25f)
                cg.SetCloseDoorFalse();
        }
    }

    IEnumerator ChangeDialogue()
    {
        yield return new WaitForSeconds(5.0f);

        string newDialogue = dialoguelist[Random.Range(1, dialoguelist.Count)];
        if(newDialogue != dialogue.text) dialogue.text = newDialogue;
        else if(newDialogue == dialogue.text) {
            newDialogue = dialoguelist[Random.Range(1, dialoguelist.Count)];
            dialogue.text = newDialogue;
        }
        Debug.Log("Changed Dialogue");
        StartCoroutine(ChangeDialogue());
    }

    private void OnTriggerStay(Collider other)
    {
        isInTheBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInTheBox = false;
    }
}

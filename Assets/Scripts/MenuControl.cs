using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    public Text readerTxt;
    public GameObject commandLine;
    
    private GameObject menu;
    private RectTransform clrt;
    private RectTransform rt;

	// Use this for initialization
	void Start () {
        menu = GameObject.Find("Menu");
        clrt = commandLine.GetComponent<RectTransform>();
        rt = menu.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        //Activate or deactivate menu
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(true);
            }
        }
        //Relocate menu with respect to commandline
        if (menu.activeSelf)
        {
            if (commandLine.activeSelf)
            {
                rt.position = new Vector2(rt.position.x, clrt.rect.height / 2 + clrt.position.y + rt.rect.height / 2);
            }
            else
            {
                rt.position = new Vector2(rt.position.x, rt.rect.height / 2);
            }
        }
	}

    public void ShowBookList()
    {
        print("BookList Button Is Pushed");
        List<string> titlelist = GameObject.Find("GameController").GetComponent<AllocateText>().GetTextList();
        readerTxt.text = "";
        for(int i=0; i < titlelist.Count; i++)
        {
            readerTxt.text += i + ": " + titlelist[i] + "\n";
        }
        GetComponent<OpeningBook>().EnlargeContent();
    }
}

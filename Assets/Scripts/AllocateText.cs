using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class AllocateText : MonoBehaviour {

    private List<GameObject> booklist;
    private List<TextAsset> textlist;
    private TextMesh title;
    private FileInfo info;
    private string[][] propertyArr;

    // Use this for initialization
    void Start ()
    {
        textlist = new List<TextAsset>();
        textlist.AddRange((TextAsset[])Resources.LoadAll(("Texts")));                                                       //list of texts, in "Resources/Texts"
        booklist = new List<GameObject>(GameObject.FindGameObjectsWithTag("Book"));                                         //list of books, GameObject
        info = new FileInfo("property.txt");                                                                                //property.txt
        propertyArr = new string[textlist.Count][];                                                                         //2D array composed of text's name and its tag

        Debug.Log("Total Books: " + booklist.Count);

        //add void GameObject to avoid error
        if (textlist.Count > booklist.Count)                                                                                
        {
            for(int i = 0; i<textlist.Count - booklist.Count; i++)
            {
                booklist.Add(new GameObject("Void"));
            }
        }

        //Sort books in list
        //least x position comes first, then z.
        //if same x and z position, least y position comes first
        booklist.Sort(delegate(GameObject A, GameObject B)
        {
            if (A.transform.position.x > B.transform.position.x) return 1;
            else if(A.transform.position.x < B.transform.position.x) return -1;
            if (A.transform.position.z > B.transform.position.z) return 1;
            else if (A.transform.position.z < B.transform.position.z) return -1;
            if(A.transform.position.x == B.transform.position.x && A.transform.position.z == B.transform.position.z)
            {
                if (A.transform.position.y > B.transform.position.y) return 1;
                else if (A.transform.position.y < B.transform.position.y) return -1;
            }
            return 0;
        });


        for(int i = 0; i < booklist.Count; i++)
        {
            Debug.Log(i + " : " + booklist[i].gameObject.name + ", Position: " +booklist[i].gameObject.transform.position);
        }
        GetProperty();
        Allocation();
	}

    void GetProperty()
    {
        StreamReader sr = info.OpenText();
        string thisLine;
        string[] splited;
        char spliter = ':';
        int reslt;
        int idx = 0;
        while (sr.Peek() > -1)
        {
            thisLine = sr.ReadLine();
            splited = thisLine.Split(spliter);
            reslt = IsNameOrTag(splited[0]);
            if (reslt == 1)
            {
                propertyArr[idx][0] = splited[1];
                if (sr.Peek() > -1)
                {
                    thisLine = sr.ReadLine();
                    splited = thisLine.Split(spliter);
                    reslt = IsNameOrTag(splited[0]);
                    if (reslt == 2)
                    {
                        propertyArr[idx][1] = splited[1];
                    }
                }
            }
            idx++;
        }
    }

    int IsNameOrTag(string str)
    {
        if(str.Contains("이름"))
        {
            return 1;
        }
        else if (str.Contains("태그"))
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    void Allocation()
    {
        //Allocate text to book
        for (int i = 0; i < booklist.Count; i++)
        {
            TextMesh bookTextMesh = booklist[i].transform.Find("BookText").GetComponent<TextMesh>();
            if (bookTextMesh == null)
            {
                Debug.Log("ERROR: NO TEXTMESH ON BOOK" + i);
            }
            else
            {
                if (textlist.Count > i) //if text exists
                {
                    Debug.Log("textlist[" + i + "] exist");
                    string txt = textlist[i].ToString();
                    if (bookTextMesh == null)
                    {
                        Debug.Log(i + "th Book doesn't exist");
                    }
                    else
                    {
                        bookTextMesh.text = txt;
                    }
                    title = booklist[i].transform.Find("Title").GetComponent<TextMesh>();

                    title.text = textlist[i].name;
                    Debug.Log(i + "th Book's title: " + title.text);
                    Debug.Log(i + "th Book's text: \n" + bookTextMesh.text);
                }
                else //if text doesn't exist
                {
                    Debug.Log(i + "th Book Doesn't exist");
                    booklist[i].GetComponentInChildren<TextMesh>(true).text = "Book " + booklist[i].transform.Find("Title").GetComponent<TextMesh>().text + "doesn't exist";
                    booklist[i].transform.Find("Title").GetComponent<TextMesh>().text = "EMPTY";
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    
    public List<string> GetTextList()
    {
        List<string> titlelist = new List<string>();
        for(int i=0; i<textlist.Count; i++)
        {
            titlelist.Add(textlist[i].name);
        }
        return titlelist;
    }
}

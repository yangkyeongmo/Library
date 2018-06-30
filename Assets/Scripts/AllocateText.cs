using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AllocateText : MonoBehaviour {

    public GameObject book;

    private List<GameObject> list;
    private List<Object> textlist;
    private GameObject[] bookShelf;
    private List<GameObject[]> bookShelfList;
    private TextMesh bookTextMesh;
    private TextMesh title;

    // Use this for initialization
    void Start ()
    {
        bookShelf = new GameObject[5];
        bookShelfList = new List<GameObject[]>();
        textlist = new List<Object>();
        textlist.AddRange(Resources.LoadAll(("Texts"), typeof(TextAsset)));
        //list = new List<GameObject>();
        list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Book"));
        Debug.Log("Total Books: " + list.Count);
        
        if (textlist.Count > list.Count)
        {
            for(int i = 0; i<textlist.Count - list.Count; i++)
            {
                list.Add(new GameObject("Void"));
            }
        }

        //Sort books in list

        list.Sort(delegate(GameObject A, GameObject B)
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
        //CreateBooks();
        /*
        for(int i = 0; i<bookShelfList.Count; i++)
        {
            for(int j= 0; j < 5; j++)
            {
                list[i * 5 + j] = bookShelfList[i][j];
            }
        }*/


        for(int i = 0; i < list.Count; i++)
        {
            Debug.Log(i + " : " + list[i].gameObject.name + ", Position: " +list[i].gameObject.transform.position);
        }

        //Allocate text to book
        for(int i=0; i < list.Count; i++)
        {
            bookTextMesh = list[i].transform.Find("BookText").GetComponent<TextMesh>();
            if(bookTextMesh == null)
            {
                Debug.Log("ERROR: NO TEXTMESH ON BOOK");
            }
            else
            {
                if (textlist.Count > i)
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
                    title = list[i].transform.Find("Title").GetComponent<TextMesh>();
                    
                    title.text = textlist[i].name;
                    Debug.Log(i + "th Book's title: " + title.text);
                    Debug.Log(i + "th Book's text: \n" + bookTextMesh.text);
                }
                else
                {
                    Debug.Log(i + "th Book Deosnt exist");
                    list[i].GetComponentInChildren<TextMesh>(true).text = "Book " + list[i].transform.Find("Title").GetComponent<TextMesh>().text + "doesn't exist";
                    list[i].transform.Find("Title").GetComponent<TextMesh>().text = "EMPTY";
                }
            }
        }
	}

    void CreateBooks()
    {
        if(textlist.Count < 6)
        {
            GameObject bookShelfObject = Instantiate(new GameObject());
            bookShelfObject.name = "BookShelf (1)";
            for(int i = 0; i<textlist.Count; i++)
            {
                bookShelf[i] = Instantiate(book, new Vector3(2, 1 + i * 0.55f, 13), Quaternion.identity);
                bookShelf[i].name = "Book1" + "(" + i + ")";
                bookShelf[i].transform.parent = bookShelfObject.transform;
            }
        }
        else if(textlist.Count >= 6)
        {
            List<GameObject> temp_bookList = new List<GameObject>();
            for(int i = 0; i<textlist.Count; i++)
            {
                temp_bookList.Add(Instantiate(book));
            }
            for (int i = 0; i < System.Math.Truncate((double)(textlist.Count / 5)) + 1; i++)
            {
                GameObject[] bookShelfObjects = new GameObject[i + 1];
                bookShelfObjects[i].name = "BookShelf " + "(" + i + ")";
                for (int j = 0; j < (System.Math.Truncate((double)(11 / 5)) == 0 ? 11 % 5 : 5); j++)
                {
                    bookShelf[j] = Instantiate(book, new Vector3(2 + 1.6f * i, 1 + j * 0.55f, 13), Quaternion.identity);
                    bookShelf[j].name = "Book" + i + " (" + j + ")";
                    bookShelf[j].transform.parent = GameObject.Find("Books").transform;
                }
                bookShelfList.Add(bookShelf);
                //bookShelf = new GameObject[5];
                /*
                for (int k = 0; k < 5; k++)
                {
                    if (temp_bookList[k + 5] != null)
                        temp_bookList[k] = temp_bookList[k + 5];
                    else if (temp_bookList == null)
                        temp_bookList[k] = null;
                }*/
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

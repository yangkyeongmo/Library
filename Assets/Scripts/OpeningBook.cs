using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningBook : MonoBehaviour {

    public GameObject whiteboard;

    private ScrollRect scrollView;
    private Ray ray;
    private RaycastHit hitInfo;
    private bool hit;
    private Text txt;

	// Use this for initialization
	void Start () {
        scrollView = whiteboard.transform.Find("ScrollView").GetComponent<ScrollRect>();
        txt = whiteboard.transform.Find("ReaderText").GetComponent<Text>();
        whiteboard.SetActive(false);
        Debug.Log(txt.text);
    }
	
	// Update is called once per frame
	void Update () {

        GetText();
        //ScrollText();

        if (whiteboard.activeSelf == false && Input.GetMouseButtonDown(1)) whiteboard.SetActive(true);
        else if (whiteboard.activeSelf == true && Input.GetMouseButtonDown(1)) whiteboard.SetActive(false);
	}

    void GetText()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();

        hit = Physics.Raycast(ray, out hitInfo);

        if (hit && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ray Hit" + hitInfo.transform.gameObject.name);
            if (hitInfo.transform.tag == "Book")
            {
                TextMesh book_text = hitInfo.transform.Find("BookText").GetComponent<TextMesh>();
                txt.text =
                    book_text.text;
            }
        }
    }
    /*
    void ScrollText()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            scrollView.verticalNormalizedPosition += Input.GetAxis("Mouse ScrollWheel");
        }
    }*/

    public RaycastHit GethitInfo()
    {
        return hitInfo;
    }
    
    public bool GetRayhit()
    {
        return hit;
    }
}

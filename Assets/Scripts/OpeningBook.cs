using UnityEngine;
using UnityEngine.UI;

public class OpeningBook : MonoBehaviour {

    public GameObject whiteboard;
    
    private RectTransform contentRect;
    private Scrollbar sBar;
    private Ray ray;
    private RaycastHit hitInfo;
    private bool hit;
    private Text txt;

	// Use this for initialization
	void Start () {
        txt = whiteboard.transform.Find("BookReader").Find("Content").Find("ReaderText").GetComponent<Text>(); //get 'ReaderText's text component
        contentRect = whiteboard.transform.Find("BookReader").Find("Content").GetComponent<RectTransform>();
        sBar = whiteboard.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
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
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hitInfo = new RaycastHit();

        hit = Physics.Raycast(ray, out hitInfo);

        if (hit && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ray Hit" + hitInfo.transform.gameObject.name);
            if (hitInfo.transform.tag == "Book")
            {
                TextMesh book_text = hitInfo.transform.Find("BookText").GetComponent<TextMesh>();
                txt.text = book_text.text;
                EnlargeContent();                       //Resize content
                sBar.value = 1.0f;                      //Reset scrollbar value
            }
        }
    }

    public void EnlargeContent()
    {
        contentRect.sizeDelta = new Vector2(txt.transform.parent.parent.parent.GetComponent<RectTransform>().rect.width - 15.0f, txt.preferredHeight + 30.0f);
    }

    public RaycastHit GethitInfo()
    {
        return hitInfo;
    }
    
    public bool GetRayhit()
    {
        return hit;
    }
}

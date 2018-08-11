using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 Application.dataPath = D:/User/문서/UnityPersonal/Library/Assets
*/

public class AllocateTextsToBooks : MonoBehaviour {
    private string appPath, idListPath;
    private string textRefPath;
    private List<TextAsset> textassetList = new List<TextAsset>();
    private List<List<object>> textList;
    private List<List<object>> tagList;
    private List<int> IDList = new List<int>();
    //Add List of Books

    System.IO.StreamReader textRef;

	// Use this for initialization
	void Start () {
        appPath = Application.dataPath;
        idListPath = appPath + "/Resources/References/idlist.txt";
        //Set text reference file's path
        textRefPath = appPath + "/Resources/References/TextReference.txt";
        //If text reference file doesn't exist
        if (!System.IO.File.Exists(textRefPath))
        {
            //Create text reference file
            System.IO.File.CreateText(textRefPath);
            //Write text infos in text reference file
            //Fetch textassets internally and externally
            textassetList = FetchTextAssets();
            //Fetch all infos in textassets
            textList = FetchTextInfos();
            //Write text infos
        }
        //If text reference file exists
        else
        {
            //Read and save infos from text reference file

        }
	}

    //Returns List of TextAssets
    List<TextAsset> FetchTextAssets()
    {
        //List to Fetch texts internally
        List<TextAsset> internalTexts = new List<TextAsset>();
        internalTexts.AddRange(Resources.LoadAll<TextAsset>("/Texts"));
        //List to fetch texts externally
        List<TextAsset> externalTexts = new List<TextAsset>();
        foreach(string txtPath in System.IO.Directory.GetFiles(appPath + "/Resources/Text/"))
        {
            //Check directory just to be sure
            if (System.IO.File.Exists(txtPath))
            {
                //Add each texts
                externalTexts.Add((TextAsset)AssetDatabase.LoadAssetAtPath(txtPath, typeof(TextAsset)));
            }
            else
            {
                print("UNWANTED SITUATION: " + txtPath + " doesn't exist!");
            }
        }
        //List of internal and external texts, list to return
        List<TextAsset> totalTexts = new List<TextAsset>();
        totalTexts.AddRange(internalTexts);
        totalTexts.AddRange(externalTexts);

        return totalTexts;
    }

    //Returns List of List
    //List<(TextAsset, TextAsset's title, TextAsset's ID, TextAsset's tag1, tag2, ...)>
    List<List<object>> FetchTextInfos()
    {
        //List to return
        List<List<object>> allTextList = new List<List<object>>();
        string content, thisline;
        List<object> newText;
        //if IDList doesn't exist
        if(!System.IO.Directory.Exists(idListPath))
        {
            CreateIDList(idListPath, textassetList);
        }
        foreach (TextAsset txt in textassetList)
        {
            //Create new list of objects
            newText = new List<object>();
            //Add textasset to first item
            newText[0] = txt;
            //Add title to second item
            newText[1] = txt.name;
            content = txt.text;
            thisline = GetFirstLineFromString(content);
            //If there's ID
            if (thisline.Contains("<ID>"))
            {
                //Add ID to third item
                newText[2] = GetStringBetweenInfoFromString(thisline, "<ID>", "</ID>");
            }
            //If there's no ID
            else
            {
                //create ID
                //write ID tag and ID in first line
            }
            thisline = GetStringExceptFirstLine(thisline);
            while (thisline.Contains("<tag>"))
            {
                //Add each tag to next item
                newText.Add(GetStringBetweenInfoFromString(thisline, "<tag>", "</tag>"));
                //Move to next line
                thisline = GetStringExceptFirstLine(thisline);
            }
            //Add this list to textList
            allTextList.Add(newText);
        }//end of foreach
        return allTextList;
    }
    //check firstline, if ID exists, write ID
    void CreateIDList(string path, List<TextAsset> textAssetList)
    {
        System.IO.StreamWriter sWriter = new System.IO.StreamWriter(path);
        System.IO.File.CreateText(path);
        string content, id;
        foreach(TextAsset txt in textAssetList)
        {
            content = txt.text;
            //check first line
            id = GetStringBetweenInfoFromString(GetFirstLineFromString(content), "<ID>", "</ID>");
            if(id != null)
            {
                sWriter.WriteLine(id);
            }
        }
    }
    string GetFirstLineFromString(string content)
    {
        int index = content.IndexOfAny(new[] { '\r', '\n' });
        return index == 1 ? content : content.Substring(0, index);
    }
    string GetStringBetweenInfoFromString(string content, string info1, string info2)
    {
        int index1 = content.IndexOf(info1);
        int index2 = content.IndexOf(info2);
        return (index1 == -1 || index2 == -1) ? null : content.Substring(index1, index2 - index1);
    }
    string GetStringExceptFirstLine(string content)
    {
        return content.Split(new[] { '\r', '\n' })[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

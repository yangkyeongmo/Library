﻿using System.Collections;
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
        //make list of ID
        List<string> idList = GetListOfID(idListPath);
        //foreach textasset
        //Add TextAsset to first, title to second, ID to third, tags to last
        foreach (TextAsset txt in textassetList)
        {
            content = txt.text;
            //Create new list of objects
            newText = new List<object>();
            //Add infos
            newText[0] = txt;
            newText[1] = txt.name;
            newText[2] = GetIDFromThisLine(GetFirstLineFromString(content), idList);

            thisline = GetFirstLineFromString(GetStringExceptFirstLine(ref content));
            //Add each tag to next item
            while (thisline.Contains("<tag>"))
            {
                newText.Add(GetStringBetweenInfoFromString(thisline, "<tag>", "</tag>"));
                //Move to next line
                thisline = GetFirstLineFromString(GetStringExceptFirstLine(ref content));
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
    List<string> GetListOfID(string path)
    {
        List<string> idList = new List<string>();
        string content = System.IO.File.ReadAllText(path);
        string thisline = GetFirstLineFromString(content);
        while (thisline != null)
        {
            idList.Add(thisline);
            thisline = GetFirstLineFromString(GetStringExceptFirstLine(content));
        }
        return idList;
    }
    string GetFirstLineFromString(string content)
    {
        int index = content.IndexOfAny(new[] { '\r', '\n' });
        return index == -1 ? content : content.Substring(0, index);
    }
    string GetStringBetweenInfoFromString(string content, string info1, string info2)
    {
        int index1 = content.IndexOf(info1);
        int index2 = content.IndexOf(info2);
        return (index1 == -1 || index2 == -1) ? null : content.Substring(index1, index2 - index1);
    }
    string GetIDFromThisLine(string line, List<string> idList)
    {
        string id;
        //If there's ID
        if (line.Contains("<ID>"))
        {
            //Add ID to third item
            id = GetStringBetweenInfoFromString(line, "<ID>", "</ID>");
        }
        //If there's no ID
        else
        {
            //create ID
            id = CreateID(idList);
            AddIDtoIDList(id);
        }
        return id;
    }
    string GetStringExceptFirstLine(string content)
    {
        return content.Split(new[] { '\r', '\n' })[1];
    }
    string GetStringExceptFirstLine(ref string content)
    {
        content = content.Split(new[] { '\r', '\n' })[1];
        return content;
    }
    string CreateID(List<string> idList)
    {
        string id = Random.Range(0, 10000).ToString();
        bool notOverlapping = false;
        while (!notOverlapping)
        {
            foreach(string existingID in idList)
            {
                if (existingID == id)
                {
                    break;
                }
                if (existingID == idList[idList.Capacity - 1])
                {
                    notOverlapping = true;
                }
            }
        }
        return id;
    }
    void AddIDtoIDList(string id)
    {
        if (!System.IO.Directory.Exists(idListPath))
        {
            print("No ID LIST");
            CreateIDList(idListPath, textassetList);
        }
        System.IO.StreamWriter sWriter = new System.IO.StreamWriter(idListPath);
        sWriter.WriteLine(id);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

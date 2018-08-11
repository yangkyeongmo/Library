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
        //List to fetch texts externally
        List<TextAsset> externalTexts = new List<TextAsset>();
        //List of internal and external texts, list to return
        List<TextAsset> totalTexts = new List<TextAsset>();
        //Load all text assets from "Resources/Texts"
        internalTexts.AddRange(Resources.LoadAll<TextAsset>("/Texts"));
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
        //Add internal and external texts to totalTexts list
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
        System.IO.StreamReader txtReader;
        string thisline;
        List<object> newText;
        //foreach TextAssets
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
            txtReader = new System.IO.StreamReader(appPath + "/Resources/Texts/" + txt.name + ".txt");
            //Read first line
            thisline = txtReader.ReadLine();
            //If there's ID
            if (thisline.Contains("<ID>"))
            {
                //Add ID to third item
                newText[2] = thisline.Split(new[] { "<ID>" }, System.StringSplitOptions.None)[1].Split(new[] { "</ID>" }, System.StringSplitOptions.None)[0];
            }
            //If there's no ID
            else
            {
                //create ID
                //write ID tag and ID in first line
            }
            thisline = txtReader.ReadLine();
            while (thisline.Contains("<tag>"))
            {
                //Add each tag to next item
                newText.Add(thisline.Split(new[] { "<tag>" }, System.StringSplitOptions.None)[1].Split(new[] { "</tag>" }, System.StringSplitOptions.None)[0]);
                //Move to next line
                thisline = txtReader.ReadLine();
            }
            //Add this list to textList
            allTextList.Add(newText);
        }//end of foreach
        return allTextList;
    }
    void CreateIDList(string path, List<TextAsset> textAssetList)
    {
        System.IO.StreamWriter sWriter = new System.IO.StreamWriter(path);
        System.IO.File.CreateText(path);
        string content, id;
        //check firstline, if ID exists, write ID
        foreach(TextAsset txt in textAssetList)
        {
            content = txt.text;
            //check first line
            id = GetIDFromString(GetFirstLineFromString(content));
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
    string GetIDFromString(string content)
    {
        int index1 = content.IndexOf("<ID>");
        int index2 = content.IndexOf("</ID>");
        return (index1 == -1 || index2 == -1) ? null : content.Substring(index1, index2 - index1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/*
 Application.dataPath = D:/User/문서/UnityPersonal/Library/Assets
*/

public class MakeReferenceTexts : MonoBehaviour {
    private string appPath, idListPath;
    private string textRefPath;
    private List<TextAsset> textassetList = new List<TextAsset>();
    private List<List<object>> textList;
    private List<List<object>> tagList;
    private List<string> IDList = new List<string>();

	// Use this for initialization
	void Start () {
        appPath = Application.dataPath;
        idListPath = appPath + "\\Resources\\References\\idlist.txt";
        //Set text reference file's path
        textRefPath = appPath + "\\Resources\\References\\TextReference.txt";
        if (!File.Exists(textRefPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(appPath + "\\Resources\\References\\");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            File.Create(textRefPath).Close();
            textassetList = FetchTextAssets();
            textList = FetchTextInfos();
            WriteTextInfo(textList, textRefPath);
        }
        else
        {
            textList = ReadTextInfo(textRefPath);
        }
	}

    //Returns List of TextAssets
    List<TextAsset> FetchTextAssets()
    {
        //List to Fetch texts internally
        List<TextAsset> internalTexts = new List<TextAsset>();
        internalTexts.AddRange(Resources.LoadAll<TextAsset>("Texts"));
        //List to fetch texts externally
        List<TextAsset> externalTexts = new List<TextAsset>();
        if(Directory.Exists(appPath + "\\Resources\\Text\\"))
        {
            foreach (string txtPath in Directory.GetFiles("\\Resources\\Text\\"))
            {
                //Check directory just to be sure
                if (File.Exists(txtPath))
                {
                    //Add each texts
                    externalTexts.Add((TextAsset)AssetDatabase.LoadAssetAtPath(txtPath, typeof(TextAsset)));
                }
                else
                {
                    print("UNWANTED SITUATION: " + txtPath + " doesn't exist!");
                }
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
        string content, thisline, txtPath;
        List<object> newText;
        //if IDList doesn't exist
        if(!File.Exists(idListPath))
        {
            CreateIDList(idListPath, textassetList);
        }
        //make list of ID
        IDList = GetListOfID(idListPath);
        //foreach textasset
        //Add TextAsset to first, title to second, ID to third, tags to last
        foreach (TextAsset txt in textassetList)
        {
            content = txt.text;
            txtPath = AssetDatabase.GetAssetPath(txt);
            //Create new list of objects
            newText = new List<object>();
            //Add infos
            newText.Add(txt);
            newText.Add(txt.name);
            newText.Add(GetIDFromThisLine(ProcessText.GetFirstLineFromString(content), IDList));
            if (!ProcessText.GetFirstLineFromString(content).Contains("<ID>"))
            {
                ProcessText.AddStringAtFirstLine("<ID>" + (string)newText[2] + "</ID>", txtPath);
            }
            
            thisline = ProcessText.GetFirstLineFromString(ProcessText.GetStringExceptFirstLine(ref content));
            //Add each tag to next item
            while (thisline.Contains("<tag>"))
            {
                newText.Add(ProcessText.GetStringBetweenInfoFromString(thisline, "<tag>", "</tag>"));
                //Move to next line
                thisline = ProcessText.GetFirstLineFromString(ProcessText.GetStringExceptFirstLine(ref content));
            }
            //Add this list to textList
            allTextList.Add(newText);
        }//end of foreach
        return allTextList;
    }
    //check firstline, if ID exists, write ID
    void CreateIDList(string idListPath, List<TextAsset> textAssetList)
    {
        if (!File.Exists(idListPath))
        {
            File.Create(idListPath).Close();
        }
        
        string content, id;
        foreach (TextAsset txt in textAssetList)
        {
            content = txt.text;
            //check first line
            id = ProcessText.GetStringBetweenInfoFromString(ProcessText.GetFirstLineFromString(content), "<ID>", "</ID>");
            if (id != null)
            {
                ProcessText.AddStringAtLastLine(id, idListPath);
            }
        }
    }
    List<string> GetListOfID(string path)
    {
        List<string> idList = new List<string>();
        string content;
        using (StreamReader sReader = new StreamReader(path))
        {
            content = sReader.ReadToEnd();
        }
        
        string thisline = ProcessText.GetFirstLineFromString(content);
        while (thisline != null && thisline != "")
        {
            idList.Add(thisline);
            thisline = ProcessText.GetFirstLineFromString(ProcessText.GetStringExceptFirstLine(ref content));
        }
        return idList;
    }
   
    string GetIDFromThisLine(string line, List<string> idList)
    {
        string id;
        //If there's ID
        if (line.Contains("<ID>"))
        {
            //Add ID to third item
            id = ProcessText.GetStringBetweenInfoFromString(line, "<ID>", "</ID>");
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
    string CreateID(List<string> idList)
    {
        string id = Random.Range(0, 10000).ToString();
        bool notOverlapping = false;
        while (!notOverlapping)
        {
            if(idList.Count != 0)
            {
                foreach (string existingID in idList)
                {
                    if (existingID == id)
                    {
                        break;
                    }
                    if (existingID == idList[idList.Count - 1])
                    {
                        notOverlapping = true;
                    }
                }
            }
            else
            {
                notOverlapping = true;
            }
        }
        return id;
    }
    void AddIDtoIDList(string id)
    {
        if (!File.Exists(idListPath))
        {
            print("No ID LIST");
            CreateIDList(idListPath, textassetList);
        }
        ProcessText.AddStringAtLastLine(id, idListPath);
        IDList.Add(id);
    }

    void WriteTextInfo(List<List<object>> textList, string path)
    {
        using(StreamWriter sWriter = new StreamWriter(path))
        {
            foreach (List<object> txtInfo in textList)
            {
                sWriter.Write("<Path>" + AssetDatabase.GetAssetPath((TextAsset)txtInfo[0]) + "</Path> <Title>" + txtInfo[1] + "</Title> <ID>" + txtInfo[2] + "</ID>");
                for (int i = 3; i < txtInfo.Count; i++)
                {
                    sWriter.Write("<tag" + (i - 3) + ">" + txtInfo[i] + "</tag" + (i - 3) + ">");
                }
                sWriter.WriteLine();
            }
        }
    }

    List<List<object>> ReadTextInfo(string path)
    {
        List<List<object>> infoList = new List<List<object>>();
        List<object> item;
        using (StreamReader sReader = new StreamReader(path))
        {
            string line = sReader.ReadLine();
            int i = 0;
            while (!sReader.EndOfStream)
            {
                item = new List<object>();
                item.Add(AssetDatabase.LoadAssetAtPath(ProcessText.GetStringBetweenInfoFromString(line, "<Path>", "</Path>"), typeof(TextAsset)));
                item.Add((string)ProcessText.GetStringBetweenInfoFromString(line, "<Title>", "</Title>"));
                item.Add((string)ProcessText.GetStringBetweenInfoFromString(line, "<ID>", "</ID>"));
                line = line.Split(new[] { "</ID>" }, System.StringSplitOptions.None)[1];
                while (line.Contains("<tag"))
                {
                    item.Add(ProcessText.GetStringBetweenInfoFromString(line, "<tag", "</tag").Split(new[] { '>' })[1]);
                    line = line.Split(new[] { "</ID>" }, System.StringSplitOptions.None)[1];
                }
                infoList.Add(item);
                line = sReader.ReadLine();
                i++;
            }
        }
        return infoList;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetTextReferencePath()
    {
        return textRefPath;
    }
}

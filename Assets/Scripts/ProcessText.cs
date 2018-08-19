using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProcessText : MonoBehaviour {
    public static string GetFirstLineFromString(string content)
    {
        int index = content.IndexOfAny(new[] { '\r', '\n' });
        return index == -1 ? content : content.Substring(0, index);
    }
    public static string GetStringBetweenInfoFromString(string content, string info1, string info2)
    {
        int index1 = content.IndexOf(info1[info1.Length - 1]) + 1;
        int index2 = content.IndexOf(info2);
        return (index1 == -1 || index2 == -1) ? null : content.Substring(index1, index2 - index1);
    }
    public static void AddStringAtFirstLine(string addedLine, string originalPath)
    {
        string tempFile = Path.GetTempFileName();
        string fullContent = File.ReadAllText(originalPath);
        string thisLine;
        using (StreamWriter sWriter = new StreamWriter(tempFile))
        using (StreamReader sReader = new StreamReader(originalPath))
        {
            sWriter.WriteLine(addedLine);
            while (!sReader.EndOfStream)
            {
                thisLine = sReader.ReadLine();
                sWriter.WriteLine(thisLine);
            }
        }
        File.Copy(tempFile, originalPath, true);
    }
    public static void AddStringAtLastLine(string addedLine, string originalPath)
    {
        string tempFile = Path.GetTempFileName();
        string fullContent = File.ReadAllText(originalPath);
        string thisLine;
        using (StreamWriter sWriter = new StreamWriter(tempFile))
        using (StreamReader sReader = new StreamReader(originalPath))
        {
            while (!sReader.EndOfStream)
            {
                thisLine = sReader.ReadLine();
                sWriter.WriteLine(thisLine);
            }
            sWriter.WriteLine(addedLine);
        }
        File.Copy(tempFile, originalPath, true);
    }
    public static string GetStringExceptFirstLine(string content)
    {
        return content.Split(new[] { '\r', '\n' })[1];
    }
    public static string GetStringExceptFirstLine(ref string content)
    {
        string[] temp = content.Split(new[] { '\r', '\n' });
        content = "";
        for (int i = 1; i < temp.Length; i++)
        {
            if (temp[i] != "")
            {
                content += temp[i];
            }
            else
            {
                if (i != 1)
                {
                    content += '\n';
                }

            }
        }
        return content;
    }
}

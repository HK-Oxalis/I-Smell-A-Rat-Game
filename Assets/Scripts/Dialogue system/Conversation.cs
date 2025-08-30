
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using UnityEditor;


[Serializable]
public class Dialogue_Line
{
    public string text;
    public string keyphrase = "";
    public string topic = "";
    public string new_Information = "";
    public float volume = 5;
    public string audio_Path = "";
    public float length_Seconds;
    public int speaker_Number;

    public void Add_To_Notebook()
    {
        Debug.Log("Adding line " + text);
        TextAsset jsonFile = Resources.Load<TextAsset>("SavedNotebook"); // no .json extension
        string path = "Assets/Resources/SavedNotebook.json";

        // Deserialize into C# objects
        EntriesWrapper data = JsonUtility.FromJson<EntriesWrapper>(jsonFile.text);

        foreach (Entry entry in data.entries)
        {
            if (topic == entry.name)
            {
                Debug.Log("Added line");
                entry.information.Add(new_Information);
                entry.unlocked = true;

                if(entry.og_textbox == " "){ entry.og_textbox = this.text; }

                break;
            }
        }

        File.WriteAllText(path, JsonUtility.ToJson(data));
        AssetDatabase.ImportAsset(path);

        
    }
}

[CreateAssetMenu(fileName = "Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Dialogue_Line[] lines;
    public string file_Name = "";
    public int source_Count = 1;


    public string Save_JSON()
    {
        string json = "{\"name\":" + file_Name + "}";
        json += "{\"speaker_Count\":" + source_Count + "}";

        foreach (Dialogue_Line line in lines)
        {
            json += JsonUtility.ToJson(line);
        }

        return json;
    }

    public void load_JSON(string json)
    {
        string[] strings = json.Split('}');

        this.file_Name = strings[0].Substring(strings[0].IndexOf(":"));
        file_Name = file_Name.Trim(new Char[] { '\"', ':' });

        this.source_Count = (int)strings[1][(strings[1].IndexOf(":") + 1)] - (int)'0';

        List<Dialogue_Line> json_Lines = new List<Dialogue_Line>();

        for (int i = 2; i < strings.Length - 1; i++) {
            //The split function removes the delimiter character, so I'm adding it back for the Json
            strings[i] += "}";
            json_Lines.Add(JsonUtility.FromJson<Dialogue_Line>(strings[i]));
        }

        this.lines = json_Lines.ToArray();
    }
}

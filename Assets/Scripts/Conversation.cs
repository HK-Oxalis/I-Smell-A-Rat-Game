
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


public class Dialogue_Line
{
    public string name;
    public string text;
    public string keyphrase;
    public float volume;
    public float length_Seconds;
    public int speaker_Number;
}

[CreateAssetMenu(fileName = "Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Dialogue_Line[] lines;
    public int source_Count = 1;


    public string Save_JSON()
    {
        string json = "{\"speaker_Count\":" + source_Count + "}";

        foreach (Dialogue_Line line in lines)
        {
            json += JsonUtility.ToJson(line);
        }

        return json;
    }

    public void load_JSON(string json)
    {
        string[] strings = json.Split('}');

        this.source_Count = (int)strings[0][(strings[0].IndexOf(":") + 1)] - (int)'0';

        List<Dialogue_Line> json_Lines = new List<Dialogue_Line>();

        for (int i = 1; i < strings.Length - 1; i++) {
            //The split function removes the delimiter character, so I'm adding it back for the Json
            strings[i] += "}";
            Debug.Log(strings[i]);
            json_Lines.Add(JsonUtility.FromJson<Dialogue_Line>(strings[i]));
        }

        this.lines = json_Lines.ToArray();
    }
}


using UnityEngine;
using System.Collections.Generic;


public class Dialogue_Line
{
    public string text;
    public string keyphrase;
    public float volume;
    public float length_Seconds;
    public int source_Index;
}

[CreateAssetMenu(fileName = "Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Dialogue_Line[] lines;
    public int source_Count = 1;

    public string Save_JSON()
    {
        string json = "{\"source_Count\":" + source_Count + "} ";

        foreach (Dialogue_Line line in lines)
        {
            json += JsonUtility.ToJson(line) + " ";
        }

        return json;
    }

    public void load_JSON(string json)
    {
        string[] strings = json.Split(' ');

        this.source_Count = strings[0][(strings[0].IndexOf(":") + 1)];

        List<Dialogue_Line> json_Lines = new List<Dialogue_Line>();

        for (int i = 1; i < strings.Length; i++) {
            json_Lines.Add(JsonUtility.FromJson<Dialogue_Line>(strings[i]));
        }
    }
}

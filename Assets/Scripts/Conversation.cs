using UnityEngine;


public class Dialogue_Line
{
    public string text;
    public float volume;
    public float length_Seconds;
    public int source_Index;
}

[CreateAssetMenu(fileName = "Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Dialogue_Line[] lines;
    public int source_Count = 1;
}

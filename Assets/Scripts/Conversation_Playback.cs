using System.Collections;
using UnityEngine;


[ExecuteAlways]
public class Conversation_Playback : MonoBehaviour
{
    [SerializeField] Conversation conversation;
    [SerializeField] Transform[] sources;
    private int line_Index = 0;

    private void Awake()
    {
        if (!Application.IsPlaying(gameObject)) 
        {
            sources = new Transform[conversation.source_Count];
        }
        else
        {
            StartCoroutine(Start_Conversation());
        }
    }

    public IEnumerator Start_Conversation()
    {
        while (line_Index < conversation.lines.Length) {
            Dialogue_Line current_Line = conversation.lines[line_Index];
            Debug.Log(current_Line.text);

            yield return new WaitForSeconds(current_Line.length_Seconds);

            line_Index++;
        }
    }
}

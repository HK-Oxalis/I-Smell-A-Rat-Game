using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


[ExecuteAlways]
public class Conversation_Playback : MonoBehaviour
{
    [SerializeField] TextAsset conversation_Asset; 
    private Conversation conversation;
    [SerializeField] Transform[] speakers;
    private UIDocument document;
    private int line_Index = 0;

    private void Awake()
    {
        if (!Application.IsPlaying(gameObject)) 
        {
            //sources = new Transform[conversation.source_Count];
        }
        else
        {
            //StartCoroutine(Start_Conversation());
        }
    }

    private void OnValidate()
    {
        if (conversation_Asset != null) {
            if(!conversation) { conversation = ScriptableObject.CreateInstance<Conversation>(); }
            conversation.load_JSON(conversation_Asset.text);

            if(speakers == null || speakers.Length != conversation.source_Count) { 
                speakers = new Transform[conversation.source_Count];
            }
            
        }
    }

    public IEnumerator Start_Conversation()
    {
        line_Index = 0;

        while (line_Index < conversation.lines.Length) {
            Dialogue_Line current_Line = conversation.lines[line_Index];


            //Check if the speaker is within earshot
            //TODO: scale volume according to planned room size
            Transform speaker = speakers[current_Line.speaker_Number];
            Collider[] overlaps = Physics.OverlapSphere(speaker.position, current_Line.volume * 1.2f);
            
            bool overlaps_Player = false;

            foreach (Collider coll in overlaps) 
            {
                if (coll.gameObject.GetComponent<Clicker_Player>() != null) 
                {
                    overlaps_Player = true;
                }
            }

            if (overlaps_Player) 
            {
                Debug.Log(current_Line.text);
            }
            else
            {
                Debug.Log("Too quiet to hear");
            }



                yield return new WaitForSeconds(current_Line.length_Seconds);

            line_Index++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[ExecuteAlways]
public class Conversation_Playback : MonoBehaviour
{
    public const float Room_Earshot_Scale = 2.3f;

    [SerializeField] TextAsset conversation_Asset;
    private Conversation conversation;
    [SerializeField] Transform[] speakers;
    [SerializeField] Texture2D speech_Bubble;
    private UIDocument document;
    private int line_Index = 0;
    private Label[] active_Bubbles;

    private void Start()
    {
        document = GetComponent<UIDocument>();
        document.enabled = false;
    }

    private void OnValidate()
    {
        if (conversation_Asset != null)
        {
            if (!conversation) { conversation = ScriptableObject.CreateInstance<Conversation>(); }
            conversation.load_JSON(conversation_Asset.text);

            if (speakers == null || speakers.Length != conversation.source_Count)
            {
                speakers = new Transform[conversation.source_Count];

            }

        }
    }

    public IEnumerator Start_Conversation(bool from_Beginning = true)
    {
        document.enabled = true;

        if (active_Bubbles != null) { Remove_Speech_Bubbles(); }

        active_Bubbles = new Label[conversation.source_Count];

        if (from_Beginning) { line_Index = 0; }


        while (line_Index < conversation.lines.Length && active_Bubbles != null)
        {
            Dialogue_Line current_Line = conversation.lines[line_Index];


            //Check if the speaker is within earshot
            Transform speaker = speakers[current_Line.speaker_Number];
            Collider[] overlaps = Physics.OverlapSphere(speaker.position, current_Line.volume * Room_Earshot_Scale);

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
                bool has_Keyphrase = current_Line.keyphrase != "";
                Add_Speech_Bubble(Add_Keyword_Style(current_Line.text, current_Line.keyphrase), current_Line.speaker_Number, has_Keyphrase);
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

    private void Add_Speech_Bubble(string speech, int speaker, bool has_Keyphrase = false)
    {
        VisualElement root = document.rootVisualElement;

        Label bubble;

        if (active_Bubbles[speaker] != null) { root.Remove(active_Bubbles[speaker]); }

        bubble = new Label();
        active_Bubbles[speaker] = bubble;
        bubble.text = speech;
        root.Add(bubble);
        bubble.style.backgroundImage = speech_Bubble;
        bubble.style.unityTextAlign = TextAnchor.MiddleCenter;
        bubble.style.whiteSpace = WhiteSpace.Normal;
        bubble.style.paddingBottom = (10);
        bubble.style.paddingLeft = (70);
        bubble.style.paddingRight = (50);
        bubble.style.paddingTop = (10);

        Vector2 pos = Get_Bubble_Point(Camera.main, speakers[speaker].position);

        bubble.style.position = Position.Absolute;

        bubble.style.left = pos.x;
        bubble.style.top = pos.y;

        if (has_Keyphrase)
        {
            bubble.RegisterCallbackOnce<ClickEvent>((evt) => conversation.lines[line_Index].Add_To_Notebook());
        }


    }
    


    private Vector2 Get_Bubble_Point(Camera cam, Vector3 position)
    {

        Vector3 viewport_Pos = cam.WorldToViewportPoint(position);


        viewport_Pos.x = Mathf.Clamp(viewport_Pos.x, 0.1f, 0.9f);
        viewport_Pos.y = Random.Range(0.1f, 0.9f);
        viewport_Pos.z = Mathf.Abs(viewport_Pos.z);

        return cam.ViewportToScreenPoint(viewport_Pos);


    }

    public void Remove_Speech_Bubbles()
    {
        if (active_Bubbles == null) { return; }
        VisualElement root = document.rootVisualElement;

        foreach (Label bubble in active_Bubbles)
        {
            if (bubble == null) { continue; }
            root.Remove(bubble);
        }

        active_Bubbles = null;
    }

    private string Add_Keyword_Style(string text, string keyphrase)
    {
        if(keyphrase == ""){ return text; }

        int index = text.IndexOf(keyphrase);

        int end_Index = index + keyphrase.Length;

        //Add the end index before the start so the initial tag doesn't change the index
        text = text.Insert(end_Index, "</b>");
        text = text.Insert(index, "<b>");
        

        return text;
    }
}

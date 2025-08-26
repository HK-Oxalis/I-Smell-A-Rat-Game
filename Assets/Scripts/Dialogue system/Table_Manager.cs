using UnityEngine;

public class Table_Manager : MonoBehaviour
{
    private Conversation_Playback playback;
    void Awake()
    {
        playback = GetComponent<Conversation_Playback>();

        Clicker_Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Clicker_Player>();

        player.entering_Dialogue_Mode.AddListener(Start_Conversation);
        player.entering_Map_Mode.AddListener(Stop_Conversation);
    }

    public void Start_Conversation()
    {
        StartCoroutine(playback.Start_Conversation());
    }

    public void Stop_Conversation()
    {
        StopCoroutine(playback.Start_Conversation());
        playback.Remove_Speech_Bubbles();
    }
}

using UnityEngine;

public class Table_Manager : MonoBehaviour
{
    private Conversation_Playback playback;
    void Awake()
    {
        playback = GetComponent<Conversation_Playback>();
    }

    public void Start_Conversation()
    {
        Debug.Log("Trying to start conversation");
        StartCoroutine(playback.Start_Conversation());
    }
}

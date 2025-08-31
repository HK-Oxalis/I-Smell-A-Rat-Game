using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Bartender : MonoBehaviour
{
    private Conversation_Playback playback;
    [SerializeField] float bartender_Conversation_Length = 5;
    void Awake()
    {
        playback = GetComponent<Conversation_Playback>();

        Clicker_Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Clicker_Player>();

        player.entering_Dialogue_Mode.AddListener(Start_Conversation);
    }


    private void Start_Conversation()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, 5 * Conversation_Playback.Room_Earshot_Scale);

        bool overlaps_Player = false;

        foreach (Collider coll in overlaps)
        {
            if (coll.gameObject.GetComponent<Clicker_Player>() != null)
            {
                overlaps_Player = true;
            }
        }

        if (!overlaps_Player) { return; }

        StartCoroutine(playback.Start_Conversation());
        StartCoroutine(Wait_For_Conversation_End());
    }

    private IEnumerator Wait_For_Conversation_End()
    {
        yield return new WaitForSeconds(bartender_Conversation_Length);

        SceneManager.LoadSceneAsync(2);

    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [Tooltip("Assign the audio clips you want to play in order.")]
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    private int currentClipIndex = 0;
    //private bool isPlaying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioClips != null && audioClips.Length > 0)
        {
            StartCoroutine(PlaySequentially());
        }
        else
        {
            Debug.LogWarning("No audio clips assigned to SequentialAudioPlayer on " + gameObject.name);
        }
    }

    IEnumerator PlaySequentially()
    {
        //isPlaying = true;

        while (currentClipIndex < audioClips.Length)
        {
            AudioClip clip = audioClips[currentClipIndex];

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();

                // Wait until the clip is done playing
                yield return new WaitForSeconds(clip.length);
            }

            currentClipIndex++;
            if (currentClipIndex >= audioClips.Length)
            {
                currentClipIndex = 0;
            }
        }

        //isPlaying = false;
    }
}

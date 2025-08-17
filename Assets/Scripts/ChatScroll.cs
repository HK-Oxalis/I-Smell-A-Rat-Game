using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OW_ChatBubble : MonoBehaviour
{
    [SerializeField] TMP_Text textField;
    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;
    
    Coroutine textCoroutine;

    string dialogue;
    [HideInInspector] public bool typing = false;

    // Use this for initialization
    void Start()
    {
        //_tmpProText = GetComponent<TMP_Text>()!;

        //if (textField != null)
        //{
        //    dialogue = textField.text;
        //    textField.text = "";

        //    if(textCoroutine != null)
        //        StopCoroutine(textCoroutine);
        //    textCoroutine = StartCoroutine("TypeWriterTMP");
        //}
    }

    public void StartTextScroll(string newDialogue)
    {
        if (typing == true)
            return;

        
        textField.transform.parent.gameObject.SetActive(true);
        typing = true;
        dialogue = newDialogue;
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine("TypeWriterTMP");
    }

    IEnumerator TypeWriterTMP()
    {
        textField.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in dialogue)
        {
            if (textField.text.Length > 0)
            {
                textField.text = textField.text.Substring(0, textField.text.Length - leadingChar.Length);
            }
            textField.text += c;
            textField.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            textField.text = textField.text.Substring(0, textField.text.Length - leadingChar.Length);
        }

        typing = false;
        textCoroutine = null;
    }

    public void EndTextScroll()
    {
        typing = false;
        textField.transform.parent.gameObject.SetActive(false);
    }
}

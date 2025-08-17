using TMPro;
using UnityEngine;

public class NotebookEntry : MonoBehaviour
{
    public string entryName = "Name";
    public string entryDialogueRef = "This is the text that was originally in the text box you saved.";
    public string entryExplanation = "This is a quick musing or explanation of the entry as explained in the characters head.";

    NotebookManager nm;

    private void Awake()
    {
        nm = GameObject.FindGameObjectWithTag("NotebookManager").GetComponent<NotebookManager>();
        GetComponentInChildren<TextMeshProUGUI>().text = entryName;
    }

    public void EntryClicked ()
    {
        nm.DisplayExplanation(entryName, entryDialogueRef, entryExplanation);
    }
}

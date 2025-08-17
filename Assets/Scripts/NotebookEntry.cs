using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotebookEntry : MonoBehaviour
{

    public int entryId = 1;
    string entryName = "Name";
    string entryDialogueRef = "This is the text that was originally in the text box you saved.";
    string entryExplanation = "This is a quick musing or explanation of the entry as explained in the characters head.";
    List<string> connectedEntries = new List<string>();

    NotebookManager nm;

    private void Awake()
    {
        nm = GameObject.FindGameObjectWithTag("NotebookManager").GetComponent<NotebookManager>();
        //GetComponentInChildren<TextMeshProUGUI>().text = entryName;
    }

    public void EntryClicked ()
    {
        nm.DisplayExplanation(entryName, entryDialogueRef, entryExplanation);
    }

    public void UpdateEntry (string name, string diaRef, string expl, List<string> ce)
    {
        entryName = name;
        entryDialogueRef = diaRef;
        entryExplanation = expl;
        connectedEntries = ce;
        GetComponentInChildren<TextMeshProUGUI>().text = entryName;
    }
}

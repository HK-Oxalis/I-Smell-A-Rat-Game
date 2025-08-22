using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotebookEntry : MonoBehaviour
{

    public int entryId = 1;
    string entryName = "Name";
    string entryDialogueRef = "This is the text that was originally in the text box you saved.";
    List<string> entryInformation = new List<string>{ "This is a quick musing or explanation of the entry as explained in the characters head." };
    List<string> connectedEntries = new List<string>();
    bool unlocked = false;

    NotebookManager nm;

    private void Awake()
    {
        nm = GameObject.FindGameObjectWithTag("NotebookManager").GetComponent<NotebookManager>();
        //GetComponentInChildren<TextMeshProUGUI>().text = entryName;
    }

    public void EntryClicked ()
    {
        string finalInformation = entryInformation[0];
        for (int i = 1; i < entryInformation.Count; i++)
        {
            finalInformation += "\n\n" + entryInformation[i];
        }
        nm.DisplayExplanation(entryName, entryDialogueRef, finalInformation);
    }

    public void UpdateEntry (string name, string diaRef, List<string> expl, List<string> ce, bool unlocked)
    {
        entryName = name;
        entryDialogueRef = diaRef;
        entryInformation = expl;
        connectedEntries = ce;
        GetComponentInChildren<TextMeshProUGUI>().text = entryName;
        this.unlocked = unlocked;
    }
}

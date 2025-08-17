using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public GameObject explanationDividerGO;
    public Transform entryParent;
    public TextMeshProUGUI entryNameTB;
    public TextMeshProUGUI entryDiaRefTB;
    public TextMeshProUGUI entryExplanationTB;

    List<NotebookEntry> allEntries = new List<NotebookEntry>();

    private void Awake()
    {
        explanationDividerGO.SetActive(false);
        foreach (Transform index in entryParent)
        {
            allEntries.Add(index.gameObject.GetComponent<NotebookEntry>());
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadJSON();
    }

    void LoadJSON ()
    {
        // Load the JSON file (place it in Resources folder in Unity)
        TextAsset jsonFile = Resources.Load<TextAsset>("SavedNotebook"); // no .json extension
        Debug.Log("Opened JSON");

        // Deserialize into C# objects
        EntriesWrapper data = JsonUtility.FromJson<EntriesWrapper>(jsonFile.text);

        foreach (Entry e in data.entries)
        {
            Debug.Log("Name: " + e.name + " Loaded");
            foreach (NotebookEntry f in allEntries)
            {
                if (f.entryId == e.id)
                {
                    f.UpdateEntry(e.name, e.og_textbox, e.explanation, e.connected_entries);
                }
            }
        }
    }

    public void DisplayExplanation (string entryName, string entryDialogueRef, string entryExplanation)
    {
        explanationDividerGO.SetActive(true);
        entryNameTB.text = entryName;
        entryDiaRefTB.text = entryDialogueRef;
        entryExplanationTB.text = entryExplanation;
    }
}

[Serializable]
public class Entry
{
    public int id;
    public string name;
    public string og_textbox;
    public string explanation;
    public List<string> connected_entries;
}

[Serializable]
public class EntriesWrapper
{
    public List<Entry> entries;
}


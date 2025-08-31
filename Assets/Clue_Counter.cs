using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class Clue_Counter : MonoBehaviour
{
    private UIDocument document;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        document = gameObject.GetComponent<UIDocument>();

        Label text = document.rootVisualElement.Q("Clue_Text") as Label;

        byte[] file = File.ReadAllBytes(Application.dataPath + "/Resources/SavedNotebook.json");

        TextAsset jsonFile = new TextAsset(file);

        EntriesWrapper data = JsonUtility.FromJson<EntriesWrapper>(jsonFile.text);

        int counter = 0;

        foreach (Entry e in data.entries)
        {
            counter += e.information.Count;
        }

        text.text = "You found " + counter + " clues!";
    }

}

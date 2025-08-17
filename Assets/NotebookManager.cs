using TMPro;
using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public GameObject explanationDividerGO;
    public TextMeshProUGUI entryNameTB;
    public TextMeshProUGUI entryDiaRefTB;
    public TextMeshProUGUI entryExplanationTB;

    private void Awake()
    {
        explanationDividerGO.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayExplanation (string entryName, string entryDialogueRef, string entryExplanation)
    {
        explanationDividerGO.SetActive(true);
        entryNameTB.text = entryName;
        entryDiaRefTB.text = entryDialogueRef;
        entryExplanationTB.text = entryExplanation;
    }
}

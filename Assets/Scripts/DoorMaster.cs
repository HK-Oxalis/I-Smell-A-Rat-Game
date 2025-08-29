using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorMaster : MonoBehaviour
{
    public bool startWithKnock = true;
    public AudioSource knockSFX;
    public GameObject[] activateAfterKnock;
    public GameObject[] deactivateAfterKnock;
    public GameObject answerPrefab;
    public Transform answerBank;
    public Animator doorAnim;

    List<string> incorrectAnswerList = new List<string>
            { "Herbert", "Hunky", "Howitzer" };
    string correctAnswer = "Hoover";
    bool isKnocking = false;
    int maxKnocks = 3;
    int curKnocks = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int answerInt = Random.Range(0, 3);
        for (int i = 0; i < 3; i++)
        {
            GameObject newAnswer = Instantiate(answerPrefab, answerBank);
            if (i == answerInt)
            {
                newAnswer.GetComponentInChildren<TextMeshProUGUI>().text = "<color=yellow>" + correctAnswer + "</color>";
                newAnswer.GetComponent<DoorChoiceButton>().correctAnswer = true;
            }
            else
            {
                string ans = incorrectAnswerList[Random.Range(0, incorrectAnswerList.Count)];
                newAnswer.GetComponentInChildren<TextMeshProUGUI>().text = ans;
                newAnswer.GetComponent<DoorChoiceButton>().correctAnswer = false;
                incorrectAnswerList.Remove(ans);
            }
        }
        isKnocking = startWithKnock;
        if (!isKnocking)
        {
            DoormanAnswers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isKnocking)
        {
            Knock();
        }
    }

    void Knock ()
    {
        curKnocks++;
        if (knockSFX) knockSFX.Play();
        if (curKnocks >= maxKnocks)
        {
            isKnocking = false;
            DoormanAnswers();
        }
    }

    void DoormanAnswers ()
    {
        if (doorAnim) doorAnim.SetTrigger("Open");
        foreach (GameObject index in activateAfterKnock)
            index.SetActive(true);
        foreach (GameObject index in deactivateAfterKnock)
            index.SetActive(false);
    }

    public void AnswerClicked (bool correctAnswer)
    {
        if (correctAnswer)
        {
            Debug.Log("Player Chose Right!");
            CorrectAnswerChosen();
        }
        else
        {
            Debug.Log("Player Chose Wrong!");
        }
    }

    void CorrectAnswerChosen ()
    {
        foreach (GameObject index in activateAfterKnock)
            index.SetActive(false);

        Clicker_Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Clicker_Player>();

        player.Enter_Map_Mode();
        if (doorAnim) doorAnim.SetTrigger("Close");
    }
}

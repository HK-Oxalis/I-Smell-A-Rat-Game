using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMaster : MonoBehaviour
{
    public bool startWithKnock = true;
    public AudioSource knockSFX;
    public GameObject[] activateAfterKnock;
    public GameObject[] deactivateAfterKnock;
    public GameObject answerPrefab;
    public Transform answerBank;
    public Animator doorAnim;
    public GameObject notepadHUD;
    public TextMeshPro doormanText;
    public Texture2D knockHand;
    public Texture2D pointHand;

    List<string> incorrectAnswerList = new List<string>
            { "Herbert", "Hunky", "Howitzer" };
    string correctAnswer = "Hoover";
    bool isKnocking = false;
    int maxKnocks = 3;
    int curKnocks = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.SetCursor(knockHand, Vector2.zero, CursorMode.Auto);
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
        Cursor.SetCursor(pointHand, Vector2.zero, CursorMode.Auto);
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
            notepadHUD.SetActive(true);
        }
        else
        {
            Debug.Log("Player Chose Wrong!");
            doormanText.text = "Wrong Answer. Security!";
            Invoke("ReloadScene", 3);
        }
    }

    void ReloadScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

using UnityEngine;

public class DoorChoiceButton : MonoBehaviour
{
    public bool correctAnswer = false;

    DoorMaster dm;

    private void Awake()
    {
        dm = GameObject.FindGameObjectWithTag("DoorMaster").GetComponent<DoorMaster>();
        Debug.Log(dm);
    }

    public void ClickedButton ()
    {
        Debug.Log("Button clicked");
        dm.AnswerClicked(correctAnswer);
    }
}

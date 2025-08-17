using UnityEngine;

public class DoorChoiceButton : MonoBehaviour
{
    public bool correctAnswer = false;

    DoorMaster dm;

    private void Awake()
    {
        dm = GameObject.FindGameObjectWithTag("DoorMaster").GetComponent<DoorMaster>();
    }

    public void ClickedButton ()
    {
        dm.AnswerClicked(correctAnswer);
    }
}

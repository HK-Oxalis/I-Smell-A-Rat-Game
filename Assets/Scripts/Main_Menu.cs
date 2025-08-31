using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void Start_Game()
    {
        SceneManager.LoadSceneAsync(1);
    }

}

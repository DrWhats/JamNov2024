using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Game_Controller : MonoBehaviour
{

/*    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }*/
    public void StartAct()
    {
        ActManager.Instance.StartAct();
    }

    public void StartQuest()
    {
        ActManager.Instance.StartQuest();
    }

    public void GoSleep()
    {
        SceneManager.LoadScene("Sleep");
    }
}

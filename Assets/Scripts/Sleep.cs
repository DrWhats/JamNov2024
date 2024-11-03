using UnityEngine;
using UnityEngine.SceneManagement;

public class Sleep : MonoBehaviour
{
    void Awake()
    {
        ActManager.Instance.NextAct();
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void GoNext()
    {
        SceneManager.LoadScene("Test_NPC");
    }
}

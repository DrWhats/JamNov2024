using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SuccessMiniGame(int actId)
    {
        PlayerPrefs.SetInt("LastAct", actId);
        SceneManager.LoadScene("Forge_Main");
    }

    public void FailedMiniGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

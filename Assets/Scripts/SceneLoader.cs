using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SuccessMiniGame(int actId)
    {
        PlayerPrefs.SetInt("LastAct", actId);
        ActManager.Instance.SetActState(ActManager.ActState.ActDone);
        SceneManager.LoadScene("Forge_Main");
    }

    public void FailedMiniGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

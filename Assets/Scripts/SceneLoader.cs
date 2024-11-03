using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SuccessMiniGame(int actId)
    {
        PlayerPrefs.SetInt("LastAct", actId);
        ActManager.Instance.SetActState(ActManager.ActState.ActDone);
        SceneManager.LoadScene("Test_NPC");
    }

    public void FailedMiniGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

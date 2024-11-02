using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Этот метод будет вызываться при нажатии на кнопку
    public void RestartScene()
    {
        // Получаем имя текущей сцены
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Перезагружаем текущую сцену
        SceneManager.LoadScene(currentSceneName);
    }
}
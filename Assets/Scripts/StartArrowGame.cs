using UnityEngine;
using UnityEngine.UI;

public class StartArrowGame : MonoBehaviour
{
    public ArrowManager arrowManager; // Ссылка на ArrowManager

    private void Start()
    {
        // Получаем компонент кнопки
        Button button = GetComponent<Button>();
        // Добавляем слушатель на событие нажатия кнопки
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Запускаем спавн стрелок
        arrowManager.StartSpawning();
        // Отключаем объект кнопки
        gameObject.SetActive(false);
    }
}
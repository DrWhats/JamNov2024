using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Mono.Cecil;

public class ArrowGame : MonoBehaviour
{
    // Ссылки на объекты и UI элементы
    public GameObject circle;
    public GameObject arrowUpPrefab;
    public GameObject arrowDownPrefab;
    public GameObject arrowLeftPrefab;
    public GameObject arrowRightPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText; // Добавьте этот компонент в ваш Canvas

    // Настройки
    public float arrowSpeed = 400f;  // Скорость стрелок
    public float arrowInterval = 1f; // Интервал между появлением стрелок
    public float arrowDistance = 10f; // Расстояние, на которое стрелка должна пройти до круга
    public float scaleFactor = 1.5f; // Фактор масштабирования круга

    // Списки для хранения последовательности стрелок
    private List<string> arrowSequence = new List<string>();
    private List<string> currentSequence = new List<string>();

    // Очки
    private int score = 0;

    // Ссылка на Canvas
    private Canvas canvas;

    // Флаг для отслеживания состояния игры
    private bool isGameOver = false;

    // Исходный масштаб круга
    private Vector3 originalScale;

    void Start()
    {
        // Получаем ссылку на Canvas
        canvas = GetComponentInParent<Canvas>();

        // Получаем исходный масштаб круга
        originalScale = circle.transform.localScale;

        // Заполните arrowSequence случайной последовательностью стрелок и пропусков
        // Например:
        arrowSequence.Add("ArrowUp");
        arrowSequence.Add("ArrowDown");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowLeft");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowLeft");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowLeft");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowLeft");
        arrowSequence.Add("Skip");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowUp");
        arrowSequence.Add("ArrowDown");
        arrowSequence.Add("ArrowUp");
        arrowSequence.Add("ArrowDown");
        arrowSequence.Add("Skip");
        arrowSequence.Add("ArrowRight");
        arrowSequence.Add("ArrowUp");
        arrowSequence.Add("ArrowDown");
        arrowSequence.Add("ArrowLeft");
        // ... и т.д. 

        // Инициализируем currentSequence
        currentSequence = new List<string>(arrowSequence);

        // Запустите поток для создания стрелок
        StartCoroutine(SpawnArrows());
    }

    // Создание стрелок
    IEnumerator SpawnArrows()
    {
        while (!isGameOver)
        {
            // Получите следующий элемент последовательности
            string nextArrow = GetNextArrow();

            // Создайте стрелку
            GameObject arrow = CreateArrow(nextArrow);

            if (arrow != null)
            {
                // Запустите анимацию движения стрелки
                StartCoroutine(MoveArrow(arrow));
            }

            yield return new WaitForSeconds(arrowInterval);

            // Проверка, закончилась ли последовательность
            if (currentSequence.Count == 0)
            {
                isGameOver = true;
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "Игра завершена! Ваш счет: " + score;
            }
        }
    }

    // Получение следующего элемента последовательности
    private string GetNextArrow()
    {
        if (currentSequence.Count == 0)
        {
            currentSequence = new List<string>(arrowSequence);
        }

        string nextArrow = currentSequence[0];
        currentSequence.RemoveAt(0);
        return nextArrow;
    }

    // Создание стрелки
    private GameObject CreateArrow(string arrowType)
    {
        GameObject arrow = null;
        Vector3 spawnPosition = new Vector3(Screen.width, circle.transform.position.y, 0);

        switch (arrowType)
        {
            case "ArrowUp":
                arrow = Instantiate(arrowUpPrefab, spawnPosition, Quaternion.identity, canvas.transform);
                break;
            case "ArrowDown":
                arrow = Instantiate(arrowDownPrefab, spawnPosition, Quaternion.identity, canvas.transform);
                break;
            case "ArrowLeft":
                arrow = Instantiate(arrowLeftPrefab, spawnPosition, Quaternion.identity, canvas.transform);
                break;
            case "ArrowRight":
                arrow = Instantiate(arrowRightPrefab, spawnPosition, Quaternion.identity, canvas.transform);
                break;
            case "Skip":
                // Не создавать стрелку
                break;
            default:
                Debug.LogError("Неизвестный тип стрелки: " + arrowType);
                break;
        }
        // Вставьте эту строку, чтобы убедиться, что стрелка создана
        Debug.Log("Создана стрелка: " + arrowType);
        return arrow;
    }

    // Анимация движения стрелки
    IEnumerator MoveArrow(GameObject arrow)
    {
        bool isCorrectKeyPressed = false;

        // Движение стрелки 
        while (arrow.transform.position.x > circle.transform.position.x)
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x - arrowSpeed * Time.deltaTime, circle.transform.position.y, 0);

            // Проверка, прошла ли стрелка мимо круга
            if (arrow.transform.position.x < circle.transform.position.x - arrowDistance)
            {
                // Стрелка ушла, ничего не делаем
                break;
            }

            // Проверка нажатия клавиши
            if (!isCorrectKeyPressed)
            {
                // Изменение масштаба круга по Y, когда стрелка в зоне нажатия
                if (arrow.transform.position.x <= circle.transform.position.x + arrowDistance &&
                    arrow.transform.position.x >= circle.transform.position.x - arrowDistance)
                {
                    circle.transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleFactor, originalScale.z);
                    Debug.Log("Scale");
                }
                else
                {
                    circle.transform.localScale = originalScale; // Возвращаем исходный масштаб
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && arrow.name == "ArrowUp(Clone)" ||
                    Input.GetKeyDown(KeyCode.DownArrow) && arrow.name == "ArrowDown(Clone)" ||
                    Input.GetKeyDown(KeyCode.LeftArrow) && arrow.name == "ArrowLeft(Clone)" ||
                    Input.GetKeyDown(KeyCode.RightArrow) && arrow.name == "ArrowRight(Clone)")
                {
                    // Верный ответ
                    score++;
                    isCorrectKeyPressed = true;
                }
                else if (Input.anyKeyDown)
                {
                    // Неверный ответ
                    score--;
                    isCorrectKeyPressed = true;
                }

                // Обновление счета
                scoreText.text = "Счет: " + score;
            }

            yield return null;
        }

        // Возвращаем исходный масштаб круга после удаления стрелки
        circle.transform.localScale = originalScale;

        // Удалите стрелку
        Destroy(arrow);
    }
}
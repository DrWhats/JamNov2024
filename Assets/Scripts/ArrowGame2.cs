using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class ArrowGame2 : MonoBehaviour
{
    public GameObject circle;
    public GameObject rightArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private float arrowSpeed = 400f;
    private float arrowSpawnInterval = 2f;
    private float lastArrowSpawnTime = 0f;
    private List<GameObject> arrows = new List<GameObject>();
    private List<string> arrowSequence = new List<string>();

    void Start()
    {
        // Начальная генерация последовательности стрелок
        GenerateArrowSequence(10);
    }

    void Update()
    {
        // Создание стрелок
        if (Time.time - lastArrowSpawnTime >= arrowSpawnInterval)
        {
            lastArrowSpawnTime = Time.time;
            SpawnArrow();
        }

        // Перемещение стрелок
        for (int i = 0; i < arrows.Count; i++)
        {
            GameObject arrow = arrows[i];
            arrow.transform.Translate(Vector3.left * arrowSpeed * Time.deltaTime);

            // Проверка, пересекла ли стрелка круг
            if (CheckCollision(arrow, circle))
            {
                HandleArrowCollision(arrow);
            }

            // Проверка, вышла ли стрелка за пределы экрана
            if (arrow.transform.position.x < -10f) //  -10f - значение для проверки выхода за пределы
            {
                HandleArrowOffScreen(arrow);
            }
        }

        // Обновление текста счета
        scoreText.text = "Score: " + score;

        // Обработка нажатия клавиш
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckArrowInput("Right");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckArrowInput("Left");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckArrowInput("Up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckArrowInput("Down");
        }
    }

    // Генерация последовательности стрелок
    void GenerateArrowSequence(int length)
    {
        arrowSequence.Clear();
        string[] directions = { "Right", "Left", "Up", "Down" };
        for (int i = 0; i < length; i++)
        {
            arrowSequence.Add(directions[Random.Range(0, directions.Length)]);
        }
    }

    // Создание новой стрелки
    void SpawnArrow()
    {
        if (arrowSequence.Count > 0)
        {
            string direction = arrowSequence[0];
            arrowSequence.RemoveAt(0);

            GameObject newArrow = null;

            switch (direction)
            {
                case "Right":
                    newArrow = Instantiate(rightArrowPrefab);
                    break;
                case "Left":
                    newArrow = Instantiate(leftArrowPrefab);
                    break;
                case "Up":
                    newArrow = Instantiate(upArrowPrefab);
                    break;
                case "Down":
                    newArrow = Instantiate(downArrowPrefab);
                    break;
            }

            if (newArrow != null)
            {
                newArrow.name = direction;
                newArrow.transform.SetParent(transform);
                newArrow.transform.position = new Vector3(Screen.width, Random.Range(Screen.height / 4, Screen.height * 3 / 4), 0); //  Позиция за правой границей
                arrows.Add(newArrow);
            }
        }
    }

    // Проверка столкновения стрелки с кругом
    bool CheckCollision(GameObject arrow, GameObject circle)
    {
        Rect arrowRect = GetRect(arrow);
        Rect circleRect = GetRect(circle);
        return arrowRect.Overlaps(circleRect);
    }

    // Обработка столкновения стрелки с кругом
    void HandleArrowCollision(GameObject arrow)
    {
        arrows.Remove(arrow);
        Destroy(arrow);
        if (arrow.name == GetCorrectArrowName())
        {
            score++;
        }
        else
        {
            score--;
        }
    }

    // Обработка выхода стрелки за пределы экрана
    void HandleArrowOffScreen(GameObject arrow)
    {
        arrows.Remove(arrow);
        Destroy(arrow);
        score--;
    }

    // Проверка правильности нажатой клавиши
    void CheckArrowInput(string direction)
    {
        if (direction == GetCorrectArrowName())
        {
            score++;
        }
        else
        {
            score--;
        }
    }

    // Получение имени правильной стрелки
    string GetCorrectArrowName()
    {
        if (arrows.Count > 0)
        {
            return arrows[0].name;
        }
        return "";
    }

    // Получение Rect объекта
    Rect GetRect(GameObject obj)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        return new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.rect.width, rectTransform.rect.height);
    }
}
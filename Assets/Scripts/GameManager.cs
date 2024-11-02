using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject[] arrowPrefabs; // Массив префабов стрелок
    public float arrowSpeed = 500f;
    public float spawnInterval = 1f;
    public int score = 0;

    private List<GameObject> activeArrows = new List<GameObject>();
    private Text scoreText;
    private RectTransform canvasRect;

    void Start()
    {
        // Получаем RectTransform Canvas
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Создаем круг в центре экрана
        GameObject circle = Instantiate(circlePrefab, canvasRect);
        circle.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        circle.tag = "Circle"; // Добавляем тег для круга

        // Находим Text объект для отображения счета
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        UpdateScoreText();

        // Запускаем корутину для создания стрелок
        StartCoroutine(SpawnArrows());

        Debug.Log("Game started!");
    }

    void Update()
    {
        // Проверяем нажатие клавиш
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("UpArrow pressed!");
            CheckArrowHit(KeyCode.UpArrow);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("DownArrow pressed!");
            CheckArrowHit(KeyCode.DownArrow);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow pressed!");
            CheckArrowHit(KeyCode.RightArrow);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("LeftArrow pressed!");
            CheckArrowHit(KeyCode.LeftArrow);
        }
    }

    IEnumerator SpawnArrows()
    {
        while (true)
        {
            // Выбираем случайную стрелку из массива
            int randomIndex = Random.Range(0, arrowPrefabs.Length);
            GameObject arrowPrefab = arrowPrefabs[randomIndex];

            // Создаем стрелку справа от экрана
            GameObject arrow = Instantiate(arrowPrefab, canvasRect);
            arrow.transform.position = new Vector3(Screen.width, Screen.height / 2, 0);
            activeArrows.Add(arrow);

            Debug.Log("Arrow spawned at position: " + arrow.transform.position);

            // Запускаем корутину для движения стрелки
            StartCoroutine(MoveArrow(arrow));

            // Ждем интервал перед созданием следующей стрелки
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveArrow(GameObject arrow)
    {
        while (arrow != null && arrow.transform.position.x > 0)
        {
            arrow.transform.position -= new Vector3(arrowSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }

        // Если стрелка прошла мимо круга, удаляем ее и уменьшаем счет
        if (arrow != null)
        {
            activeArrows.Remove(arrow);
            Destroy(arrow);
            DecreaseScore();
            Debug.Log("Arrow missed the circle!");
        }
    }

    void CheckArrowHit(KeyCode key)
    {
        // Проверяем, есть ли активная стрелка, которая соответствует нажатой клавише
        foreach (var arrow in activeArrows)
        {
            if (arrow != null && arrow.name.Contains(key.ToString()))
            {
                // Если стрелка соприкасается с кругом, увеличиваем счет и удаляем стрелку
                if (IsArrowTouchingCircle(arrow))
                {
                    activeArrows.Remove(arrow);
                    Destroy(arrow);
                    IncreaseScore();
                    Debug.Log("Correct arrow hit!");
                    return;
                }
            }
        }

        // Если нажата неправильная клавиша, уменьшаем счет
        DecreaseScore();
        Debug.Log("Wrong arrow pressed!");
    }

    bool IsArrowTouchingCircle(GameObject arrow)
    {
        // Проверяем, соприкасается ли стрелка с кругом
        GameObject circle = GameObject.Find("Circle(Clone)");
        if (circle != null)
        {
            Collider2D circleCollider = circle.GetComponent<Collider2D>();
            Collider2D arrowCollider = arrow.GetComponent<Collider2D>();

            bool isTouching = circleCollider.bounds.Intersects(arrowCollider.bounds);
            if (isTouching)
            {
                Debug.Log("Arrow is touching the circle!");
            }
            else
            {
                Debug.Log("Arrow is NOT touching the circle!");
            }
            return isTouching;
        }
        return false;
    }

    void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        Debug.Log("Score increased: " + score);
    }

    void DecreaseScore()
    {
        score--;
        UpdateScoreText();
        Debug.Log("Score decreased: " + score);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
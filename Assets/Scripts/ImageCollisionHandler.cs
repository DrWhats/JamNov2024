using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // Добавляем ссылку на SceneManager

public class ImageCollisionHandler : MonoBehaviour
{
    private int score = 0; // Счетчик очков
    private GameObject currentCollidingArrow; // Текущая пересекаемая стрелка
    public TextMeshProUGUI scoreText; // Ссылка на TextMeshProUGUI для отображения счета
    public LayerMask arrowLayer; // Слой, на котором находятся стрелки
    public InputActionAsset inputActions;
    private InputAction moveAction;
    public GameObject[] HitSFX;
    public GameObject[] visualFeedbackObjects; // Массив объектов для визуальной обратной связи
    public Button returnButton; // Ссылка на кнопку "Return"
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] Animator animator;

    private bool wasArrowMissed = false; // Флаг, указывающий, что стрелка была пропущена

    private void Start()
    {
        UpdateScoreText();
        moveAction = inputActions.FindAction("Player/Move"); // Убедитесь, что это правильный путь к действию
        moveAction.Enable();
        moveAction.performed += ctx => CheckForInput(); // Подписываемся на событие performed

        // Скрываем кнопку "Return" и надпись "Win" при старте
        if (returnButton != null)
        {
            returnButton.gameObject.SetActive(false);
            returnButton.onClick.AddListener(RestartScene); // Привязываем метод RestartScene к событию нажатия кнопки
        }

        // Деактивируем все объекты визуальной обратной связи при старте
        foreach (var obj in visualFeedbackObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        moveAction.performed -= ctx => CheckForInput(); // Отписываемся от события при уничтожении объекта
    }

    private void OnDisable()
    {
        moveAction.performed -= ctx => CheckForInput(); // Отписываемся от события при отключении объекта
    }

    void CheckForInput()
    {
        // Получаем вектор движения
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Проверяем, не является ли animator null
        if (animator != null)
        {
            animator.Play("Hit");
        }

        if (currentCollidingArrow != null)
        {
            string arrowName = currentCollidingArrow.name;
            // Проверяем направление
            if ((moveInput.x < 0) && arrowName.Contains("Left"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: LeftArrow");
                Instantiate(HitSFX[0]);
                UpdateVisualFeedback(0);
            }
            else if ((moveInput.x > 0) && arrowName.Contains("Right"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: RightArrow");
                Instantiate(HitSFX[1]);
                UpdateVisualFeedback(1);
            }
            else if ((moveInput.y > 0) && arrowName.Contains("Up"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: UpArrow");
                Instantiate(HitSFX[2]);
                UpdateVisualFeedback(2);
            }
            else if ((moveInput.y < 0) && arrowName.Contains("Down"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: DownArrow");
                Instantiate(HitSFX[3]);
                UpdateVisualFeedback(3);
            }
            else
            {
                DestroyArrow(currentCollidingArrow);
                score--;
                UpdateScoreText();
                Debug.Log("Incorrect key pressed");
            }
        }
    }

    private void DestroyArrow(GameObject arrow)
    {
        if (arrow != null)
        {
            Destroy(arrow);
            currentCollidingArrow = null;
            ArrowManager arrowManager = FindAnyObjectByType<ArrowManager>();
            if (arrowManager != null)
            {
                arrowManager.totalArrowsDestroyed++;
            }
        }
    }

    private void CheckForCollisions()
    {
        // Создаем луч из центра объекта Circle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, arrowLayer);
        Debug.DrawRay(transform.position, Vector2.zero, Color.red);
        if (hit.collider != null)
        {
            // Если луч попал в коллайдер стрелки
            if (hit.collider.gameObject.name.Contains("Up") ||
                hit.collider.gameObject.name.Contains("Down") ||
                hit.collider.gameObject.name.Contains("Right") ||
                hit.collider.gameObject.name.Contains("Left"))
            {
                currentCollidingArrow = hit.collider.gameObject;
                Debug.Log("Arrow entered: " + currentCollidingArrow.name);
                wasArrowMissed = false; // Сбрасываем флаг, так как стрелка в зоне
            }
        }
        else
        {
            // Если луч не попал в коллайдер, сбрасываем текущую пересекаемую стрелку
            if (currentCollidingArrow != null)
            {
                Debug.Log("Arrow exited: " + currentCollidingArrow.name);
                if (!wasArrowMissed) // Проверяем, была ли стрелка пропущена
                {
                    score--;
                    UpdateScoreText();
                    Debug.Log("Arrow missed");
                    wasArrowMissed = true; // Устанавливаем флаг, что стрелка была пропущена
                }
                currentCollidingArrow = null;
            }
        }
    }

    private void Update()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CheckForCollisions();

        // Проверяем, все ли стрелки были созданы и уничтожены
        ArrowManager arrowManager = FindAnyObjectByType<ArrowManager>();
        if (arrowManager != null && arrowManager.totalArrowsSpawned > 0 && arrowManager.totalArrowsSpawned == arrowManager.totalArrowsDestroyed)
        {
            // Проверяем счет
            int maxScore = arrowManager.CalculateMaxScore();
            if (score < 0.75f * maxScore)
            {
                // Показываем кнопку "Return"
                if (returnButton != null)
                {
                    failPanel.SetActive(true);
                }
            }
            else
            {
                WinPanel.SetActive(true);
            }
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Находим действие Jump в вашем Action Map
        var gameplayActions = inputActions.FindActionMap("Player"); // Замените "Actions" на точное название вашего Action Map
        var moveAction = gameplayActions.FindAction("Move");
    }

    // Метод для перезагрузки сцены
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Метод для обновления визуальной обратной связи
    private void UpdateVisualFeedback(int index)
    {
        for (int i = 0; i < visualFeedbackObjects.Length; i++)
        {
            if (visualFeedbackObjects[i] != null)
            {
                visualFeedbackObjects[i].SetActive(i == index);
            }
        }
    }
}
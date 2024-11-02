using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ImageCollisionHandler : MonoBehaviour
{
    private int score = 0; // Счетчик очков
    private GameObject currentCollidingArrow; // Текущая пересекаемая стрелка
    public TextMeshProUGUI scoreText; // Ссылка на TextMeshProUGUI для отображения счета
    public LayerMask arrowLayer; // Слой, на котором находятся стрелки
    public InputActionAsset inputActions;
    private InputAction moveAction;

    private bool wasArrowMissed = false; // Флаг, указывающий, что стрелка была пропущена

    private void Start()
    {
        UpdateScoreText();
        moveAction = inputActions.FindAction("Player/Move"); // Убедитесь, что это правильный путь к действию
        moveAction.Enable();
        moveAction.performed += ctx => CheckForInput(); // Подписываемся на событие performed
    }

    private void OnDestroy()
    {
        moveAction.performed -= ctx => CheckForInput(); // Отписываемся от события при уничтожении объекта
    }

    void CheckForInput()
    {
        // Получаем вектор движения
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (currentCollidingArrow != null)
        {
            string arrowName = currentCollidingArrow.name;
            // Проверяем направление
            if ((moveInput.x < 0) && arrowName.Contains("Left"))
            {
                Destroy(currentCollidingArrow);
                currentCollidingArrow = null;
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: LeftArrow");
            }
            else if ((moveInput.x > 0) && arrowName.Contains("Right"))
            {
                Destroy(currentCollidingArrow);
                currentCollidingArrow = null;
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: RightArrow");
            }
            else if ((moveInput.y > 0) && arrowName.Contains("Up"))
            {
                Destroy(currentCollidingArrow);
                currentCollidingArrow = null;
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: UpArrow");
            }
            else if ((moveInput.y < 0) && arrowName.Contains("Down"))
            {
                Destroy(currentCollidingArrow);
                currentCollidingArrow = null;
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: DownArrow");
            }
            else
            {
                score--;
                UpdateScoreText();
                Debug.Log("Incorrect key pressed");
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
            Debug.Log("хУНйя");
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
        CheckForCollisions();
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
        // Находим действие Jump в вашем Action Map
        var gameplayActions = inputActions.FindActionMap("Player"); // Замените "Actions" на точное название вашего Action Map
        var moveAction = gameplayActions.FindAction("Move");
    }
}
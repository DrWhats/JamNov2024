using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // ��������� ������ �� SceneManager

public class ImageCollisionHandler : MonoBehaviour
{
    private int score = 0; // ������� �����
    private GameObject currentCollidingArrow; // ������� ������������ �������
    public TextMeshProUGUI scoreText; // ������ �� TextMeshProUGUI ��� ����������� �����
    public LayerMask arrowLayer; // ����, �� ������� ��������� �������
    public InputActionAsset inputActions;
    private InputAction moveAction;
    public GameObject[] HitSFX;
    public Button returnButton; // ������ �� ������ "Return"
    public TextMeshProUGUI winText; // ������ �� TextMeshProUGUI ��� ����������� ������� "Win"

    private bool wasArrowMissed = false; // ����, �����������, ��� ������� ���� ���������

    private void Start()
    {
        UpdateScoreText();
        moveAction = inputActions.FindAction("Player/Move"); // ���������, ��� ��� ���������� ���� � ��������
        moveAction.Enable();
        moveAction.performed += ctx => CheckForInput(); // ������������� �� ������� performed

        // �������� ������ "Return" � ������� "Win" ��� ������
        if (returnButton != null)
        {
            returnButton.gameObject.SetActive(false);
            returnButton.onClick.AddListener(RestartScene); // ����������� ����� RestartScene � ������� ������� ������
        }
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        moveAction.performed -= ctx => CheckForInput(); // ������������ �� ������� ��� ����������� �������
    }

    void CheckForInput()
    {
        // �������� ������ ��������
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (currentCollidingArrow != null)
        {
            string arrowName = currentCollidingArrow.name;
            // ��������� �����������
            if ((moveInput.x < 0) && arrowName.Contains("Left"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: LeftArrow");
                Instantiate(HitSFX[0]);
            }
            else if ((moveInput.x > 0) && arrowName.Contains("Right"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: RightArrow");
                Instantiate(HitSFX[1]);
            }
            else if ((moveInput.y > 0) && arrowName.Contains("Up"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: UpArrow");
                Instantiate(HitSFX[2]);
            }
            else if ((moveInput.y < 0) && arrowName.Contains("Down"))
            {
                DestroyArrow(currentCollidingArrow);
                score++;
                UpdateScoreText();
                Debug.Log("Correct key pressed: DownArrow");
                Instantiate(HitSFX[3]);
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
        // ������� ��� �� ������ ������� Circle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, arrowLayer);
        Debug.DrawRay(transform.position, Vector2.zero, Color.red);
        if (hit.collider != null)
        {
            Debug.Log("�����");
            // ���� ��� ����� � ��������� �������
            if (hit.collider.gameObject.name.Contains("Up") ||
                hit.collider.gameObject.name.Contains("Down") ||
                hit.collider.gameObject.name.Contains("Right") ||
                hit.collider.gameObject.name.Contains("Left"))
            {
                currentCollidingArrow = hit.collider.gameObject;
                Debug.Log("Arrow entered: " + currentCollidingArrow.name);
                wasArrowMissed = false; // ���������� ����, ��� ��� ������� � ����
            }
        }
        else
        {
            // ���� ��� �� ����� � ���������, ���������� ������� ������������ �������
            if (currentCollidingArrow != null)
            {
                Debug.Log("Arrow exited: " + currentCollidingArrow.name);
                if (!wasArrowMissed) // ���������, ���� �� ������� ���������
                {
                    score--;
                    UpdateScoreText();
                    Debug.Log("Arrow missed");
                    wasArrowMissed = true; // ������������� ����, ��� ������� ���� ���������
                }
                currentCollidingArrow = null;
            }
        }
    }

    private void Update()
    {
        CheckForCollisions();

        // ���������, ��� �� ������� ���� ������� � ����������
        ArrowManager arrowManager = FindAnyObjectByType<ArrowManager>();
        if (arrowManager != null && arrowManager.totalArrowsSpawned > 0 && arrowManager.totalArrowsSpawned == arrowManager.totalArrowsDestroyed)
        {
            // ��������� ����
            int maxScore = arrowManager.CalculateMaxScore();
            if (score < 0.75f * maxScore)
            {
                // ���������� ������ "Return"
                if (returnButton != null)
                {
                    returnButton.gameObject.SetActive(true);
                }
            }
            else
            {
                // ���������� ������� "Win"
                if (winText != null)
                {
                    winText.gameObject.SetActive(true);
                }
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
        // ������� �������� Jump � ����� Action Map
        var gameplayActions = inputActions.FindActionMap("Player"); // �������� "Actions" �� ������ �������� ������ Action Map
        var moveAction = gameplayActions.FindAction("Move");
    }

    // ����� ��� ������������ �����
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
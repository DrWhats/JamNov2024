using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ImageCollisionHandler : MonoBehaviour
{
    private int score = 0; // ������� �����
    private GameObject currentCollidingArrow; // ������� ������������ �������
    public TextMeshProUGUI scoreText; // ������ �� TextMeshProUGUI ��� ����������� �����
    public LayerMask arrowLayer; // ����, �� ������� ��������� �������
    public InputActionAsset inputActions;
    private InputAction moveAction;

    private bool wasArrowMissed = false; // ����, �����������, ��� ������� ���� ���������

    private void Start()
    {
        UpdateScoreText();
        moveAction = inputActions.FindAction("Player/Move"); // ���������, ��� ��� ���������� ���� � ��������
        moveAction.Enable();
        moveAction.performed += ctx => CheckForInput(); // ������������� �� ������� performed
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
}
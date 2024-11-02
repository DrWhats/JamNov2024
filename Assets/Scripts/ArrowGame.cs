using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Mono.Cecil;

public class ArrowGame : MonoBehaviour
{
    // ������ �� ������� � UI ��������
    public GameObject circle;
    public GameObject arrowUpPrefab;
    public GameObject arrowDownPrefab;
    public GameObject arrowLeftPrefab;
    public GameObject arrowRightPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText; // �������� ���� ��������� � ��� Canvas

    // ���������
    public float arrowSpeed = 400f;  // �������� �������
    public float arrowInterval = 1f; // �������� ����� ���������� �������
    public float arrowDistance = 10f; // ����������, �� ������� ������� ������ ������ �� �����
    public float scaleFactor = 1.5f; // ������ ��������������� �����

    // ������ ��� �������� ������������������ �������
    private List<string> arrowSequence = new List<string>();
    private List<string> currentSequence = new List<string>();

    // ����
    private int score = 0;

    // ������ �� Canvas
    private Canvas canvas;

    // ���� ��� ������������ ��������� ����
    private bool isGameOver = false;

    // �������� ������� �����
    private Vector3 originalScale;

    void Start()
    {
        // �������� ������ �� Canvas
        canvas = GetComponentInParent<Canvas>();

        // �������� �������� ������� �����
        originalScale = circle.transform.localScale;

        // ��������� arrowSequence ��������� ������������������� ������� � ���������
        // ��������:
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
        // ... � �.�. 

        // �������������� currentSequence
        currentSequence = new List<string>(arrowSequence);

        // ��������� ����� ��� �������� �������
        StartCoroutine(SpawnArrows());
    }

    // �������� �������
    IEnumerator SpawnArrows()
    {
        while (!isGameOver)
        {
            // �������� ��������� ������� ������������������
            string nextArrow = GetNextArrow();

            // �������� �������
            GameObject arrow = CreateArrow(nextArrow);

            if (arrow != null)
            {
                // ��������� �������� �������� �������
                StartCoroutine(MoveArrow(arrow));
            }

            yield return new WaitForSeconds(arrowInterval);

            // ��������, ����������� �� ������������������
            if (currentSequence.Count == 0)
            {
                isGameOver = true;
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "���� ���������! ��� ����: " + score;
            }
        }
    }

    // ��������� ���������� �������� ������������������
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

    // �������� �������
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
                // �� ��������� �������
                break;
            default:
                Debug.LogError("����������� ��� �������: " + arrowType);
                break;
        }
        // �������� ��� ������, ����� ���������, ��� ������� �������
        Debug.Log("������� �������: " + arrowType);
        return arrow;
    }

    // �������� �������� �������
    IEnumerator MoveArrow(GameObject arrow)
    {
        bool isCorrectKeyPressed = false;

        // �������� ������� 
        while (arrow.transform.position.x > circle.transform.position.x)
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x - arrowSpeed * Time.deltaTime, circle.transform.position.y, 0);

            // ��������, ������ �� ������� ���� �����
            if (arrow.transform.position.x < circle.transform.position.x - arrowDistance)
            {
                // ������� ����, ������ �� ������
                break;
            }

            // �������� ������� �������
            if (!isCorrectKeyPressed)
            {
                // ��������� �������� ����� �� Y, ����� ������� � ���� �������
                if (arrow.transform.position.x <= circle.transform.position.x + arrowDistance &&
                    arrow.transform.position.x >= circle.transform.position.x - arrowDistance)
                {
                    circle.transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleFactor, originalScale.z);
                    Debug.Log("Scale");
                }
                else
                {
                    circle.transform.localScale = originalScale; // ���������� �������� �������
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && arrow.name == "ArrowUp(Clone)" ||
                    Input.GetKeyDown(KeyCode.DownArrow) && arrow.name == "ArrowDown(Clone)" ||
                    Input.GetKeyDown(KeyCode.LeftArrow) && arrow.name == "ArrowLeft(Clone)" ||
                    Input.GetKeyDown(KeyCode.RightArrow) && arrow.name == "ArrowRight(Clone)")
                {
                    // ������ �����
                    score++;
                    isCorrectKeyPressed = true;
                }
                else if (Input.anyKeyDown)
                {
                    // �������� �����
                    score--;
                    isCorrectKeyPressed = true;
                }

                // ���������� �����
                scoreText.text = "����: " + score;
            }

            yield return null;
        }

        // ���������� �������� ������� ����� ����� �������� �������
        circle.transform.localScale = originalScale;

        // ������� �������
        Destroy(arrow);
    }
}
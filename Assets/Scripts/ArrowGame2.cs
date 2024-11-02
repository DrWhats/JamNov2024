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
        // ��������� ��������� ������������������ �������
        GenerateArrowSequence(10);
    }

    void Update()
    {
        // �������� �������
        if (Time.time - lastArrowSpawnTime >= arrowSpawnInterval)
        {
            lastArrowSpawnTime = Time.time;
            SpawnArrow();
        }

        // ����������� �������
        for (int i = 0; i < arrows.Count; i++)
        {
            GameObject arrow = arrows[i];
            arrow.transform.Translate(Vector3.left * arrowSpeed * Time.deltaTime);

            // ��������, ��������� �� ������� ����
            if (CheckCollision(arrow, circle))
            {
                HandleArrowCollision(arrow);
            }

            // ��������, ����� �� ������� �� ������� ������
            if (arrow.transform.position.x < -10f) //  -10f - �������� ��� �������� ������ �� �������
            {
                HandleArrowOffScreen(arrow);
            }
        }

        // ���������� ������ �����
        scoreText.text = "Score: " + score;

        // ��������� ������� ������
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

    // ��������� ������������������ �������
    void GenerateArrowSequence(int length)
    {
        arrowSequence.Clear();
        string[] directions = { "Right", "Left", "Up", "Down" };
        for (int i = 0; i < length; i++)
        {
            arrowSequence.Add(directions[Random.Range(0, directions.Length)]);
        }
    }

    // �������� ����� �������
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
                newArrow.transform.position = new Vector3(Screen.width, Random.Range(Screen.height / 4, Screen.height * 3 / 4), 0); //  ������� �� ������ ��������
                arrows.Add(newArrow);
            }
        }
    }

    // �������� ������������ ������� � ������
    bool CheckCollision(GameObject arrow, GameObject circle)
    {
        Rect arrowRect = GetRect(arrow);
        Rect circleRect = GetRect(circle);
        return arrowRect.Overlaps(circleRect);
    }

    // ��������� ������������ ������� � ������
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

    // ��������� ������ ������� �� ������� ������
    void HandleArrowOffScreen(GameObject arrow)
    {
        arrows.Remove(arrow);
        Destroy(arrow);
        score--;
    }

    // �������� ������������ ������� �������
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

    // ��������� ����� ���������� �������
    string GetCorrectArrowName()
    {
        if (arrows.Count > 0)
        {
            return arrows[0].name;
        }
        return "";
    }

    // ��������� Rect �������
    Rect GetRect(GameObject obj)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        return new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.rect.width, rectTransform.rect.height);
    }
}
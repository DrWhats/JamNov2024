using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject[] arrowPrefabs; // ������ �������� �������
    public float arrowSpeed = 500f;
    public float spawnInterval = 1f;
    public int score = 0;

    private List<GameObject> activeArrows = new List<GameObject>();
    private Text scoreText;
    private RectTransform canvasRect;

    void Start()
    {
        // �������� RectTransform Canvas
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // ������� ���� � ������ ������
        GameObject circle = Instantiate(circlePrefab, canvasRect);
        circle.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        circle.tag = "Circle"; // ��������� ��� ��� �����

        // ������� Text ������ ��� ����������� �����
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        UpdateScoreText();

        // ��������� �������� ��� �������� �������
        StartCoroutine(SpawnArrows());

        Debug.Log("Game started!");
    }

    void Update()
    {
        // ��������� ������� ������
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
            // �������� ��������� ������� �� �������
            int randomIndex = Random.Range(0, arrowPrefabs.Length);
            GameObject arrowPrefab = arrowPrefabs[randomIndex];

            // ������� ������� ������ �� ������
            GameObject arrow = Instantiate(arrowPrefab, canvasRect);
            arrow.transform.position = new Vector3(Screen.width, Screen.height / 2, 0);
            activeArrows.Add(arrow);

            Debug.Log("Arrow spawned at position: " + arrow.transform.position);

            // ��������� �������� ��� �������� �������
            StartCoroutine(MoveArrow(arrow));

            // ���� �������� ����� ��������� ��������� �������
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

        // ���� ������� ������ ���� �����, ������� �� � ��������� ����
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
        // ���������, ���� �� �������� �������, ������� ������������� ������� �������
        foreach (var arrow in activeArrows)
        {
            if (arrow != null && arrow.name.Contains(key.ToString()))
            {
                // ���� ������� ������������� � ������, ����������� ���� � ������� �������
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

        // ���� ������ ������������ �������, ��������� ����
        DecreaseScore();
        Debug.Log("Wrong arrow pressed!");
    }

    bool IsArrowTouchingCircle(GameObject arrow)
    {
        // ���������, ������������� �� ������� � ������
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
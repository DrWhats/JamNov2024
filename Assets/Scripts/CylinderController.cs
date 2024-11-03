using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CylinderController : MonoBehaviour
{
    public GameObject[] cylinders; // ������ ���������
    public Slider powerSlider; // �������� ��� ���� �����
    public float yOffset = 1f; // �������� �������� �� ��� Y
    public float greenZoneWidth = 0.2f; // ������ �������� �������
    public Image greenZoneImage; // ����������� ��� ����������� �������� �������
    public TextMeshProUGUI scoreText; // ��������� ���� ��� ����������� �����
    public TextMeshProUGUI winText; // ��������� ���� ��� ����������� ��������� ���������

    private int currentCylinderIndex = 0; // ������ �������� ��������
    private float power = 0f; // ������� ���� �����
    private Vector3[] originalLocalPositions; // ������������ ��������� ������� ���������
    private float greenZoneStart; // ������ �������� �������
    private float greenZoneEnd; // ����� �������� �������

    void Start()
    {
        // ��������� ������������ ��������� ������� ���������
        originalLocalPositions = new Vector3[cylinders.Length];
        for (int i = 0; i < cylinders.Length; i++)
        {
            originalLocalPositions[i] = cylinders[i].transform.localPosition;
        }

        // ������������� ��������� �����
        UpdateScoreText();
        winText.gameObject.SetActive(false); // �������� �������� ���������

        // ������������� ������� ������
        RandomizeGreenZone();
    }

    void Update()
    {
        if (currentCylinderIndex < cylinders.Length)
        {
            HandlePowerInput();
        }
    }

    void HandlePowerInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // ����������� ���� �����
            power += Time.deltaTime;
            power = Mathf.Clamp(power, 0f, 1f);
            powerSlider.value = power;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // ���������, ������ �� �� � ������� ������
            if (power >= greenZoneStart && power <= greenZoneEnd)
            {
                // �������� ����
                MoveCylinder();
            }
            else
            {
                // ��������� ����, ���������� �������� �� �������� ��������� �������
                ResetCylinders();
            }

            // ���������� ���� �����
            power = 0f;
            powerSlider.value = 0f;

            // ������������� ������� ������ ��� ���������� ��������
            RandomizeGreenZone();
        }
    }

    void MoveCylinder()
    {
        // ���������� ������� ������� �� ����� ��������� ������� �� ��� Y
        Vector3 newLocalPosition = cylinders[currentCylinderIndex].transform.localPosition;
        newLocalPosition.y += yOffset;
        cylinders[currentCylinderIndex].transform.localPosition = newLocalPosition;

        // ��������� � ���������� ��������
        currentCylinderIndex++;

        // ��������� ����� �����
        UpdateScoreText();

        // ���������, ��� �� �������� ������
        if (currentCylinderIndex >= cylinders.Length)
        {
            WinGame();
        }
    }

    void ResetCylinders()
    {
        // ���������� ��� �������� �� �������� ��������� �������
        for (int i = 0; i < currentCylinderIndex; i++)
        {
            cylinders[i].transform.localPosition = originalLocalPositions[i];
        }

        // ���������� ������ �������� ��������
        currentCylinderIndex = 0;

        // ��������� ����� �����
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = $"{currentCylinderIndex}/{cylinders.Length}";
    }

    void WinGame()
    {
        // ���������� �������� ���������
        winText.gameObject.SetActive(true);

        // ��������� ��������
        powerSlider.gameObject.SetActive(false);
    }

    void RandomizeGreenZone()
    {
        // ������������� ������ �������� �������
        greenZoneStart = Random.Range(0f, 1f - greenZoneWidth);
        greenZoneEnd = greenZoneStart + greenZoneWidth;

        // ��������� ������� � ������ �������� �������
        RectTransform greenZoneRect = greenZoneImage.rectTransform;
        greenZoneRect.anchorMin = new Vector2(greenZoneStart, 0f);
        greenZoneRect.anchorMax = new Vector2(greenZoneEnd, 1f);
        greenZoneRect.offsetMin = Vector2.zero;
        greenZoneRect.offsetMax = Vector2.zero;
    }
}
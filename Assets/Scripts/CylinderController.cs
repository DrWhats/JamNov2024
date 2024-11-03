using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CylinderController : MonoBehaviour
{
    public GameObject[] cylinders; // ������ ���������
    public Slider powerSlider; // �������� ��� ���� �����
    public float xOffset = 0f; // �������� �������� �� ��� X
    public float yOffset = 0f; // �������� �������� �� ��� Y
    public float zOffset = 0f; // �������� �������� �� ��� Z
    public float targetZoneWidth = 0.2f; // ������ �������� �������
    public RectTransform targetZone; // ����������� ��� ����������� �������� �������
    public TextMeshProUGUI scoreText; // ��������� ���� ��� ����������� �����
    public GameObject winText; // ��������� ���� ��� ����������� ��������� ���������
    public GameObject[] PinSFX;

    private int currentCylinderIndex = 0; // ������ �������� ��������
    private float power = 0f; // ������� ���� �����
    private Vector3[] originalLocalPositions; // ������������ ��������� ������� ���������

    private void Awake()
    {

        RandomizeTargetPosition();
    }

    void RandomizeTargetPosition()
    {
        Debug.Log("����� �������");
        var new_x = UnityEngine.Random.Range(-50, 50);
        targetZone.transform.localPosition = new Vector3(new_x, targetZone.transform.localPosition.y, targetZone.transform.localPosition.z);

    }

    bool IsSliderInZone()
    {
        RectTransform sliderHandle = powerSlider.handleRect;
        return RectTransformUtility.RectangleContainsScreenPoint(targetZone, sliderHandle.position);
    }

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
            if (IsSliderInZone())
            {
                // �������� ����
                MoveCylinder();
                Instantiate(PinSFX[0]);
            }
            else
            {
                // ��������� ����, ���������� �������� �� �������� ��������� �������
                ResetCylinders();
                Instantiate(PinSFX[1]);
            }

            // ���������� ���� �����
            power = 0f;
            powerSlider.value = 0f;

            // ������������� ������� ������ ��� ���������� ��������
            RandomizeTargetPosition();
        }
    }

    void MoveCylinder()
    {
        // ���������� ������� ������� �� ����� ��������� ������� �� ����
        Vector3 newLocalPosition = cylinders[currentCylinderIndex].transform.localPosition;
        newLocalPosition.x += xOffset;
        newLocalPosition.y += yOffset;
        newLocalPosition.z += zOffset;
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
}
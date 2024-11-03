using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CylinderController : MonoBehaviour
{
    public GameObject[] cylinders; // Массив цилиндров
    public Slider powerSlider; // Ползунок для силы удара
    public float xOffset = 0f; // Значение смещения по оси X
    public float yOffset = 0f; // Значение смещения по оси Y
    public float zOffset = 0f; // Значение смещения по оси Z
    public float targetZoneWidth = 0.2f; // Ширина зеленого сектора
    public RectTransform targetZone; // Изображение для отображения зеленого сектора
    public TextMeshProUGUI scoreText; // Текстовое поле для отображения счета
    public GameObject winText; // Текстовое поле для отображения победного сообщения
    public GameObject[] PinSFX;

    private int currentCylinderIndex = 0; // Индекс текущего цилиндра
    private float power = 0f; // Текущая сила удара
    private Vector3[] originalLocalPositions; // Оригинальные локальные позиции цилиндров

    private void Awake()
    {

        RandomizeTargetPosition();
    }

    void RandomizeTargetPosition()
    {
        Debug.Log("Меняю позицию");
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
        // Сохраняем оригинальные локальные позиции цилиндров
        originalLocalPositions = new Vector3[cylinders.Length];
        for (int i = 0; i < cylinders.Length; i++)
        {
            originalLocalPositions[i] = cylinders[i].transform.localPosition;
        }

        // Инициализация текстовых полей
        UpdateScoreText();
        winText.gameObject.SetActive(false); // Скрываем победное сообщение
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
            // Увеличиваем силу удара
            power += Time.deltaTime;
            power = Mathf.Clamp(power, 0f, 1f);
            powerSlider.value = power;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Проверяем, попали ли мы в зеленый сектор
            if (IsSliderInZone())
            {
                // Успешный удар
                MoveCylinder();
                Instantiate(PinSFX[0]);
            }
            else
            {
                // Неудачный удар, возвращаем цилиндры на исходные локальные позиции
                ResetCylinders();
                Instantiate(PinSFX[1]);
            }

            // Сбрасываем силу удара
            power = 0f;
            powerSlider.value = 0f;

            // Рандомизируем зеленый сектор для следующего цилиндра
            RandomizeTargetPosition();
        }
    }

    void MoveCylinder()
    {
        // Перемещаем текущий цилиндр на новую локальную позицию по осям
        Vector3 newLocalPosition = cylinders[currentCylinderIndex].transform.localPosition;
        newLocalPosition.x += xOffset;
        newLocalPosition.y += yOffset;
        newLocalPosition.z += zOffset;
        cylinders[currentCylinderIndex].transform.localPosition = newLocalPosition;

        // Переходим к следующему цилиндру
        currentCylinderIndex++;

        // Обновляем текст счета
        UpdateScoreText();

        // Проверяем, все ли цилиндры забиты
        if (currentCylinderIndex >= cylinders.Length)
        {
            WinGame();
        }
    }

    void ResetCylinders()
    {
        // Возвращаем все цилиндры на исходные локальные позиции
        for (int i = 0; i < currentCylinderIndex; i++)
        {
            cylinders[i].transform.localPosition = originalLocalPositions[i];
        }

        // Сбрасываем индекс текущего цилиндра
        currentCylinderIndex = 0;

        // Обновляем текст счета
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = $"{currentCylinderIndex}/{cylinders.Length}";
    }

    void WinGame()
    {
        // Отображаем победное сообщение
        winText.gameObject.SetActive(true);

        // Выключаем ползунок
        powerSlider.gameObject.SetActive(false);
    }
}
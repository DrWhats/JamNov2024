using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CylinderController : MonoBehaviour
{
    public GameObject[] cylinders; // Массив цилиндров
    public Slider powerSlider; // Ползунок для силы удара
    public float yOffset = 1f; // Значение смещения по оси Y
    public float greenZoneWidth = 0.2f; // Ширина зеленого сектора
    public Image greenZoneImage; // Изображение для отображения зеленого сектора
    public TextMeshProUGUI scoreText; // Текстовое поле для отображения счета
    public TextMeshProUGUI winText; // Текстовое поле для отображения победного сообщения

    private int currentCylinderIndex = 0; // Индекс текущего цилиндра
    private float power = 0f; // Текущая сила удара
    private Vector3[] originalLocalPositions; // Оригинальные локальные позиции цилиндров
    private float greenZoneStart; // Начало зеленого сектора
    private float greenZoneEnd; // Конец зеленого сектора

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

        // Рандомизируем зеленый сектор
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
            // Увеличиваем силу удара
            power += Time.deltaTime;
            power = Mathf.Clamp(power, 0f, 1f);
            powerSlider.value = power;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Проверяем, попали ли мы в зеленый сектор
            if (power >= greenZoneStart && power <= greenZoneEnd)
            {
                // Успешный удар
                MoveCylinder();
            }
            else
            {
                // Неудачный удар, возвращаем цилиндры на исходные локальные позиции
                ResetCylinders();
            }

            // Сбрасываем силу удара
            power = 0f;
            powerSlider.value = 0f;

            // Рандомизируем зеленый сектор для следующего цилиндра
            RandomizeGreenZone();
        }
    }

    void MoveCylinder()
    {
        // Перемещаем текущий цилиндр на новую локальную позицию по оси Y
        Vector3 newLocalPosition = cylinders[currentCylinderIndex].transform.localPosition;
        newLocalPosition.y += yOffset;
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

    void RandomizeGreenZone()
    {
        // Рандомизируем начало зеленого сектора
        greenZoneStart = Random.Range(0f, 1f - greenZoneWidth);
        greenZoneEnd = greenZoneStart + greenZoneWidth;

        // Обновляем позицию и размер зеленого сектора
        RectTransform greenZoneRect = greenZoneImage.rectTransform;
        greenZoneRect.anchorMin = new Vector2(greenZoneStart, 0f);
        greenZoneRect.anchorMax = new Vector2(greenZoneEnd, 1f);
        greenZoneRect.offsetMin = Vector2.zero;
        greenZoneRect.offsetMax = Vector2.zero;
    }
}
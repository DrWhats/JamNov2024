using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Массив Prefab'ов стрелок (0 - пусто, 1 - Up, 2 - Down, 3 - Right, 4 - Left)
    public Transform targetCircle; // Целевой объект Circle
    public float spawnInterval = 0.4f; // Интервал появления стрелок
    public float arrowSpeed = 400f; // Скорость движения стрелок
    public string sequence = "01234"; // Строка с последовательностью появления стрелок
    public Transform canvasTransform; // Ссылка на Canvas, где будут появляться стрелки
    private int currentIndex = 0; // Индекс текущей стрелки в последовательности

    private Coroutine spawnCoroutine; // Ссылка на корутину спавна стрелок
    public int totalArrowsSpawned = 0; // Общее количество созданных стрелок
    public int totalArrowsDestroyed = 0; // Общее количество уничтоженных стрелок

    public int CalculateMaxScore()
    {
        int maxScore = 0;
        foreach (char c in sequence)
        {
            if (c != '0')
            {
                maxScore++;
            }
        }
        return maxScore;
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnArrows());
        }
    }

    private IEnumerator SpawnArrows()
    {
        while (currentIndex < sequence.Length)
        {
            // Получаем текущую цифру из последовательности
            int arrowIndex = int.Parse(sequence[currentIndex].ToString());

            // Если это не пустое место (0), создаем стрелку
            if (arrowIndex != 0)
            {
                GameObject arrowPrefab = arrowPrefabs[arrowIndex];
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, canvasTransform);
                totalArrowsSpawned++;
                StartCoroutine(MoveArrow(arrow));
            }

            // Переходим к следующей стрелке в последовательности
            currentIndex++;

            // Ждем интервал перед следующим появлением
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveArrow(GameObject arrow)
    {
        while (arrow != null && Vector2.Distance(arrow.transform.position, targetCircle.position) > 0.1f)
        {
            // Двигаем стрелку к цели
            arrow.transform.position = Vector2.MoveTowards(arrow.transform.position, targetCircle.position, arrowSpeed * Time.deltaTime);
            yield return null;
        }

        // Уничтожаем стрелку, когда она достигнет цели
        if (arrow != null)
        {
            Destroy(arrow);
            totalArrowsDestroyed++;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    public GameObject[] arrows; // Массив спрайтов стрелок
    public Transform targetCircle; // Спрайт круга
    public float speed = 5f; // Скорость движения стрелок
    public int score = 0; // Счет игры

    private void Update()
    {
        // Проверка нажатий клавиш
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CheckArrow(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CheckArrow(KeyCode.RightArrow);
        if (Input.GetKeyDown(KeyCode.UpArrow)) CheckArrow(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow)) CheckArrow(KeyCode.DownArrow);

        // Движение стрелок
        foreach (var arrow in arrows)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position, targetCircle.position, speed * Time.deltaTime);

            // Проверка, если стрелка прошла мимо круга
            if (Vector3.Distance(arrow.transform.position, targetCircle.position) > 100f)
            {
                Destroy(arrow);
                score--;
            }
        }
    }

    private void CheckArrow(KeyCode key)
    {
        foreach (var arrow in arrows)
        {
            if (Vector3.Distance(arrow.transform.position, targetCircle.position) < 50f)
            {
                if ((key == KeyCode.LeftArrow && arrow.name == "ArrowLeft") ||
                    (key == KeyCode.RightArrow && arrow.name == "ArrowRight") ||
                    (key == KeyCode.UpArrow && arrow.name == "ArrowUp") ||
                    (key == KeyCode.DownArrow && arrow.name == "ArrowDown"))
                {
                    Destroy(arrow);
                    score++;
                    return;
                }
            }
        }
        score--;
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowChecker : MonoBehaviour
{
    // Счетчик очков
    public int score = 0;
    public TextMeshProUGUI scoreText;

    // Скрипт спавнера стрелочек
    //public ArrowSpawner arrowSpawner;

    void Update()
    {
        // Проверка коллизии с стрелочкой
        if (Physics2D.Raycast(transform.position, Vector2.zero, 0.01f, LayerMask.GetMask("Arrow")))
        {
            // Получение коллайдера
            Collider2D collider = Physics2D.Raycast(transform.position, Vector2.zero, 0.01f).collider;

            // Получение имени стрелочки
            string arrowName = collider.gameObject.GetComponent<Image>().sprite.name;

            // Проверка нажатия на клавиатуру
            if (Input.GetKeyDown(KeyCode.DownArrow) && arrowName == "ArrowDownSprite")
            {
                // Удаление стрелочки
                Destroy(collider.gameObject);

                // Увеличение счета
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && arrowName == "ArrowUpSprite")
            {
                // Удаление стрелочки
                Destroy(collider.gameObject);

                // Увеличение счета
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && arrowName == "ArrowLeftSprite")
            {
                // Удаление стрелочки
                Destroy(collider.gameObject);

                // Увеличение счета
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && arrowName == "ArrowRightSprite")
            {
                // Удаление стрелочки
                Destroy(collider.gameObject);

                // Увеличение счета
                score++;
                scoreText.text = "Score: " + score;
            }
            else
            {
                // Удаление стрелочки
                Destroy(collider.gameObject);

                // Уменьшение счета
                score--;
                scoreText.text = "Score: " + score;
            }
        }
    }
}
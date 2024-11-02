using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowChecker : MonoBehaviour
{
    // ������� �����
    public int score = 0;
    public TextMeshProUGUI scoreText;

    // ������ �������� ���������
    //public ArrowSpawner arrowSpawner;

    void Update()
    {
        // �������� �������� � ����������
        if (Physics2D.Raycast(transform.position, Vector2.zero, 0.01f, LayerMask.GetMask("Arrow")))
        {
            // ��������� ����������
            Collider2D collider = Physics2D.Raycast(transform.position, Vector2.zero, 0.01f).collider;

            // ��������� ����� ���������
            string arrowName = collider.gameObject.GetComponent<Image>().sprite.name;

            // �������� ������� �� ����������
            if (Input.GetKeyDown(KeyCode.DownArrow) && arrowName == "ArrowDownSprite")
            {
                // �������� ���������
                Destroy(collider.gameObject);

                // ���������� �����
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && arrowName == "ArrowUpSprite")
            {
                // �������� ���������
                Destroy(collider.gameObject);

                // ���������� �����
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && arrowName == "ArrowLeftSprite")
            {
                // �������� ���������
                Destroy(collider.gameObject);

                // ���������� �����
                score++;
                scoreText.text = "Score: " + score;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && arrowName == "ArrowRightSprite")
            {
                // �������� ���������
                Destroy(collider.gameObject);

                // ���������� �����
                score++;
                scoreText.text = "Score: " + score;
            }
            else
            {
                // �������� ���������
                Destroy(collider.gameObject);

                // ���������� �����
                score--;
                scoreText.text = "Score: " + score;
            }
        }
    }
}
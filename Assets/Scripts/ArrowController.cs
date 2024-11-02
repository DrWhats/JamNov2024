using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    public GameObject[] arrows; // ������ �������� �������
    public Transform targetCircle; // ������ �����
    public float speed = 5f; // �������� �������� �������
    public int score = 0; // ���� ����

    private void Update()
    {
        // �������� ������� ������
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CheckArrow(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CheckArrow(KeyCode.RightArrow);
        if (Input.GetKeyDown(KeyCode.UpArrow)) CheckArrow(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow)) CheckArrow(KeyCode.DownArrow);

        // �������� �������
        foreach (var arrow in arrows)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position, targetCircle.position, speed * Time.deltaTime);

            // ��������, ���� ������� ������ ���� �����
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
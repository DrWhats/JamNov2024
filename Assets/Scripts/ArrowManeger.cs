using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // ������ Prefab'�� ������� (0 - �����, 1 - Up, 2 - Down, 3 - Right, 4 - Left)
    public Transform targetCircle; // ������� ������ Circle
    public float spawnInterval = 1f; // �������� ��������� �������
    public float arrowSpeed = 5f; // �������� �������� �������
    public string sequence = "12340"; // ������ � ������������������� ��������� �������
    public Transform canvasTransform; // ������ �� Canvas, ��� ����� ���������� �������

    private int currentIndex = 0; // ������ ������� ������� � ������������������

    private void Start()
    {
        StartCoroutine(SpawnArrows());
    }

    private IEnumerator SpawnArrows()
    {
        while (currentIndex < sequence.Length)
        {
            // �������� ������� ����� �� ������������������
            int arrowIndex = int.Parse(sequence[currentIndex].ToString());

            // ���� ��� �� ������ ����� (0), ������� �������
            if (arrowIndex != 0)
            {
                GameObject arrowPrefab = arrowPrefabs[arrowIndex];
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, canvasTransform);
                StartCoroutine(MoveArrow(arrow));
            }

            // ��������� � ��������� ������� � ������������������
            currentIndex++;

            // ���� �������� ����� ��������� ����������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveArrow(GameObject arrow)
    {
        while (arrow != null && Vector2.Distance(arrow.transform.position, targetCircle.position) > 0.1f)
        {
            // ������� ������� � ����
            arrow.transform.position = Vector2.MoveTowards(arrow.transform.position, targetCircle.position, arrowSpeed * Time.deltaTime);
            yield return null;
        }

        // ���������� �������, ����� ��� ��������� ����
        if (arrow != null)
        {
            Destroy(arrow);
        }
    }
}
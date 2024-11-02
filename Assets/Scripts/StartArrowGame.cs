using UnityEngine;
using UnityEngine.UI;

public class StartArrowGame : MonoBehaviour
{
    public ArrowManager arrowManager; // ������ �� ArrowManager

    private void Start()
    {
        // �������� ��������� ������
        Button button = GetComponent<Button>();
        // ��������� ��������� �� ������� ������� ������
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // ��������� ����� �������
        arrowManager.StartSpawning();
        // ��������� ������ ������
        gameObject.SetActive(false);
    }
}
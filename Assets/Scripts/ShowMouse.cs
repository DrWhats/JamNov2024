using UnityEngine;

public class ShowMouse: MonoBehaviour
{
    void Start()
    {
        // �������� ������ � ������ ��� �������
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

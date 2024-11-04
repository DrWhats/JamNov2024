using UnityEngine;

public class ShowMouse: MonoBehaviour
{
    void Start()
    {
        // ¬ключаем курсор и делаем его видимым
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

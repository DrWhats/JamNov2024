using UnityEngine;

public class SmoothCameraOrbit : MonoBehaviour
{
    // Объект, вокруг которого будет вращаться камера
    public Transform target;

    // Скорость вращения камеры
    public float rotationSpeed = 20f;

    // Расстояние от камеры до объекта
    public float distance = 5f;

    // Высота камеры над объектом
    public float height = 2f;

    // Сглаживание вращения
    public float smoothSpeed = 0.125f;

    // Текущий угол поворота камеры
    private float currentRotationAngle;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }

        // Автоматически увеличиваем угол поворота камеры
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // Вычисляем новую позицию камеры
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

        // Сглаживаем движение камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Смотрим на объект
        transform.LookAt(target.position + Vector3.up * height);
    }
}
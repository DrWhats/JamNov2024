using UnityEngine;

public class SmoothCameraOrbit : MonoBehaviour
{
    // ������, ������ �������� ����� ��������� ������
    public Transform target;

    // �������� �������� ������
    public float rotationSpeed = 20f;

    // ���������� �� ������ �� �������
    public float distance = 5f;

    // ������ ������ ��� ��������
    public float height = 2f;

    // ����������� ��������
    public float smoothSpeed = 0.125f;

    // ������� ���� �������� ������
    private float currentRotationAngle;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }

        // ������������� ����������� ���� �������� ������
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // ��������� ����� ������� ������
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

        // ���������� �������� ������
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ������� �� ������
        transform.LookAt(target.position + Vector3.up * height);
    }
}
using UnityEngine;
using System;

public class SwordPhase : MonoBehaviour
{
    [SerializeField] private GameObject ForgeSFX;
    [SerializeField] private float delayBeforeEnableCollider = 2.0f; // �������� ����� ���������� ����������

    public static event Action OnSwordPhase;

    private Collider swordCollider;

    private void Awake()
    {
        // �������� ��������� ����������
        swordCollider = GetComponent<Collider>();

        // ��������� ��������� ��� ������
        swordCollider.enabled = false;

        // �������� ����� EnableCollider ����� �������� �����
        Invoke("EnableCollider", delayBeforeEnableCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlaySound();
    }

    void PlaySound()
    {
        Instantiate(ForgeSFX);
        ChangePhase();
    }

    void ChangePhase()
    {
        OnSwordPhase?.Invoke();
    }

    void EnableCollider()
    {
        // �������� ���������
        swordCollider.enabled = true;
    }
}
using UnityEngine;
using System;

public class SwordPhase : MonoBehaviour
{
    [SerializeField] private GameObject ForgeSFX;
    [SerializeField] private float delayBeforeEnableCollider = 2.0f; // Задержка перед включением коллайдера

    public static event Action OnSwordPhase;

    private Collider swordCollider;

    private void Awake()
    {
        // Получаем компонент коллайдера
        swordCollider = GetComponent<Collider>();

        // Отключаем коллайдер при старте
        swordCollider.enabled = false;

        // Вызываем метод EnableCollider через заданное время
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
        // Включаем коллайдер
        swordCollider.enabled = true;
    }
}
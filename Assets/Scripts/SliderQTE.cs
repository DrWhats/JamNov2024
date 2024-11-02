using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SliderQTE : MonoBehaviour
{
    public Slider slider;
    public RectTransform targetZone;
    public float speed = 100f;
    private bool movingRight = true;

    public static event Action SuccessHit;

    public InputActionAsset inputActions; // ������ �� ��� Input Action Asset
    private InputAction jumpAction;


    private void Awake()
    {
        // ������� �������� Jump � ����� Action Map
        var gameplayActions = inputActions.FindActionMap("Player"); // �������� "Actions" �� ������ �������� ������ Action Map
        jumpAction = gameplayActions.FindAction("Jump"); // �������� "Jump" �� ������ �������� ������ ��������
    }

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        if (jumpAction != null)
        {
            jumpAction.Disable();
        }
    }

    void Update()
    {
        MoveSlider();
        CheckForInput();
    }

    void MoveSlider()
    {
        // �������� ��������
        if (movingRight)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue)
            {
                movingRight = false;
            }
        }
        else
        {
            slider.value -= speed * Time.deltaTime;
            if (slider.value <= slider.minValue)
            {
                movingRight = true;
            }
        }
    }

    void CheckForInput()
    {
        // ���������, ���� ������ � ������ ���� � ���� �������� ������ ���������
        if (IsSliderInZone() && jumpAction.WasPerformedThisFrame())
        {
            Debug.Log("Success");
            SuccessHit?.Invoke();
        }
    }

    bool IsSliderInZone()
    {
        RectTransform sliderHandle = slider.handleRect;
        return RectTransformUtility.RectangleContainsScreenPoint(targetZone, sliderHandle.position);
    }
}

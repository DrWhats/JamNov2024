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


    private void Awake()
    {

        RandomizeTargetPosition();
    }

    void RandomizeTargetPosition()
    {
        Debug.Log("Меняю позицию");
        var new_x = UnityEngine.Random.Range(-50, 50);
        targetZone.transform.localPosition = new Vector3(new_x, targetZone.transform.localPosition.y, targetZone.transform.localPosition.z);

    }

    void Update()
    {
        MoveSlider();
        //CheckForInput();
    }

    public void OnJump(InputValue value)
    {
        Debug.Log("JUMP");
        CheckForInput();
    }

    void MoveSlider()
    {
        // Движение ползунка
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
        // Проверяем, если объект в нужной зоне и если действие прыжка выполнено
        if (IsSliderInZone())
        {
            Debug.Log("Success");
            SuccessHit?.Invoke();
            RandomizeTargetPosition();
        }
    }


    bool IsSliderInZone()
    {
        RectTransform sliderHandle = slider.handleRect;
        return RectTransformUtility.RectangleContainsScreenPoint(targetZone, sliderHandle.position);
    }
}

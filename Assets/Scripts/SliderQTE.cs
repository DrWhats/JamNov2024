using UnityEngine;
using UnityEngine.UI;

public class SliderQTE : MonoBehaviour
{
    public Slider slider;
    public RectTransform targetZone;
    public float speed = 100f;
    private bool movingRight = true;

    void Update()
    {
        MoveSlider();
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
        
        if (IsSliderInZone())
        {
            //Debug.Log("IN ZONE");
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Успех!");
                
            }
        }
    }

    bool IsSliderInZone()
    {
        RectTransform sliderHandle = slider.handleRect;
        return RectTransformUtility.RectangleContainsScreenPoint(targetZone, sliderHandle.position);
    }
}

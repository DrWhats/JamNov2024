using System.Collections;
using UnityEngine;

public class ObjectVisibilityController : MonoBehaviour
{
    public GameObject objectToHide; // Объект, который нужно скрыть и показать
    public GameObject animatedObject; // Объект с анимацией
    public string animationName; // Имя анимации, которую нужно дождаться

    private Animator animator;

    void Start()
    {
        // Получаем компонент Animator у объекта с анимацией
        animator = animatedObject.GetComponent<Animator>();

        // Скрываем объект
        objectToHide.SetActive(false);

        // Запускаем ожидание окончания анимации
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        // Ждем, пока анимация не начнется
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // Ждем, пока анимация не закончится
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Показываем объект после окончания анимации
        objectToHide.SetActive(true);
    }
}
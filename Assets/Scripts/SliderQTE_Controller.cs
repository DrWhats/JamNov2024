using System.Collections;
using UnityEngine;

public class SliderQTE_Controller : MonoBehaviour
{


    [SerializeField] Animator Hammer;
    [SerializeField] GameObject slider;

    [SerializeField] Transform root;
    [SerializeField] GameObject[] ObjectSteps;
    [SerializeField] GameObject currentObject;
    [SerializeField] int stepIndex = 0;

    private Animator animator;

    private void Awake()
    {
        currentObject = Instantiate(ObjectSteps[stepIndex], root);
        animator = currentObject.GetComponent<Animator>();
        slider = GameObject.Find("SliderQTE");
    }

    private void OnEnable()
    {
        SliderQTE.SuccessHit += Hit;
        SwordPhase.OnSwordPhase += ChangePrefab;
    }

    private void OnDisable()
    {
        SliderQTE.SuccessHit -= Hit;
        SwordPhase.OnSwordPhase -= ChangePrefab;
    }

    void Hit()
    {
        Hammer.SetTrigger("Hit");

    }

    void ChangePrefab()
    {
        if (stepIndex < ObjectSteps.Length - 1)
        {
            var prevObj = currentObject;
            currentObject = Instantiate(ObjectSteps[stepIndex + 1]);
            currentObject.transform.SetParent(transform, false);
            stepIndex += 1;
            Destroy(prevObj);
            slider.gameObject.SetActive(false);
            animator = currentObject.GetComponent<Animator>();
            animator.Play("GoToFire");
            StartCoroutine(WaitForAnimation());

        }
        else
        {
            Debug.Log("Game Over");
            slider.SetActive(false);
        }

        
    }

    IEnumerator WaitForAnimation()
    {
        // Ждем, пока анимация не начнется
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("GoToFire"))
        {
            yield return null;
        }

        // Ждем, пока анимация не закончится
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("GoToFire") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Показываем объект после окончания анимации
        slider.gameObject.SetActive(true);
    }
}

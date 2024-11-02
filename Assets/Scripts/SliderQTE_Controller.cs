using UnityEngine;

public class SliderQTE_Controller : MonoBehaviour
{


    [SerializeField] Animator Hammer;
    [SerializeField] GameObject slider;

    [SerializeField] Transform root;
    [SerializeField] GameObject[] ObjectSteps;
    [SerializeField] GameObject currentObject;
    [SerializeField] int stepIndex = 0;

    private void Awake()
    {
        currentObject = Instantiate(ObjectSteps[stepIndex], root);
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
            currentObject = Instantiate(ObjectSteps[stepIndex + 1], root);
            stepIndex += 1;
            Destroy(prevObj);

        }
        else
        {
            Debug.Log("Game Over");
            slider.SetActive(false);
        }
    }
}

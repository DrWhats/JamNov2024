using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SliderQTE_Controller : MonoBehaviour
{

    [SerializeField] int actId = 0;
    [SerializeField] Animator Hammer;
    [SerializeField] GameObject slider;
    [SerializeField] GameObject Panel;
    [SerializeField] Transform root;
    [SerializeField] GameObject[] ObjectSteps;
    [SerializeField] GameObject finalStep;

    private GameObject currentObject;
    private int stepIndex = 0;
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
            Panel.SetActive(true);
            var prevObj = currentObject;
            currentObject = Instantiate(finalStep);
            currentObject.transform.SetParent(root, false);
            Destroy(prevObj);
            slider.gameObject.SetActive(false);
            
        }

    }

    IEnumerator WaitForAnimation()
    {
        // ����, ���� �������� �� ��������
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("GoToFire"))
        {
            yield return null;
        }

        // ����, ���� �������� �� ����������
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("GoToFire") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // ���������� ������ ����� ��������� ��������
        slider.gameObject.SetActive(true);
    }

    public void SuccessMiniGame()
    {
        PlayerPrefs.SetInt("LastAct", actId);
        SceneManager.LoadScene("Forge_Main");
    }
}

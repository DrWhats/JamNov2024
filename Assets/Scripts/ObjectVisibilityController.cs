using System.Collections;
using UnityEngine;

public class ObjectVisibilityController : MonoBehaviour
{
    public GameObject objectToHide; // ������, ������� ����� ������ � ��������
    public GameObject animatedObject; // ������ � ���������
    public string animationName; // ��� ��������, ������� ����� ���������

    private Animator animator;

    void Start()
    {
        // �������� ��������� Animator � ������� � ���������
        animator = animatedObject.GetComponent<Animator>();

        // �������� ������
        objectToHide.SetActive(false);

        // ��������� �������� ��������� ��������
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        // ����, ���� �������� �� ��������
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // ����, ���� �������� �� ����������
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // ���������� ������ ����� ��������� ��������
        objectToHide.SetActive(true);
    }
}
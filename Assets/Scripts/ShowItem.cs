using UnityEngine;

public class ShowItem: MonoBehaviour
{
    [SerializeField] int myAct = 0;

    void Start()
    {

        if (myAct < ActManager.Instance.CurrentAct)
        {
            transform.gameObject.SetActive(false);
        }
    }
}

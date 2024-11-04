using TMPro;
using UnityEngine;

public class DailyPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DayStart;
    void Start()
    {
        ChangeText();
        DayStart.gameObject.SetActive(true);
        ActManager.Instance.OnActStateChanged += OnActStateChanged;
    }

    private void OnActStateChanged(ActManager.ActState state)
    {
        ChangeText();
    }

    private void OnDestroy()
    {
        ActManager.Instance.OnActStateChanged -= OnActStateChanged;
    }

    void ChangeText()
    {
        if (ActManager.Instance.CurrentState == ActManager.ActState.None)
        {
            DayStart.SetText("Подойдите к стойке, чтобы начать день");
        }

        if (ActManager.Instance.CurrentState == ActManager.ActState.ActStart)
        {
            DayStart.SetText("");
        }


        if (ActManager.Instance.CurrentState == ActManager.ActState.ActInProgress)
        {
            DayStart.SetText("Подойдите к верстаку, чтобы выполнить заказ");
        }

        if (ActManager.Instance.CurrentState == ActManager.ActState.ActDone)
        {
            DayStart.SetText("Завершите заказ");
        }

        if (ActManager.Instance.CurrentState == ActManager.ActState.ActEnd)
        {
            DayStart.SetText("Вам нужно поспать, утро вечера мудренее");
        }
    }
}

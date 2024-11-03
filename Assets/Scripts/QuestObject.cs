using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestObject : MonoBehaviour
{

    public enum ObjectType
    {
        Start,
        Quest,
        End
    }

    public ObjectType objectType;

    void Start()
    {
        ActManager.Instance.OnActStateChanged += OnActStateChanged;
        OnStart();
    }

    public void Interact()
    {
        if (objectType == ObjectType.End) SceneManager.LoadScene("Sleep");
        if (objectType == ObjectType.Quest) ActManager.Instance.StartQuest();
        if (objectType == ObjectType.Start) ActManager.Instance.StartAct();
    }


    void OnDestroy()
    {
        if (ActManager.Instance != null)
        {
            ActManager.Instance.OnActStateChanged -= OnActStateChanged;

        }
    }

    void OnStart()
    {

        if (ActManager.Instance.CurrentState == ActManager.ActState.None && objectType == ObjectType.Start)
        {
            transform.GetComponent<InteractableObject>().enabled = true;
        } else if (ActManager.Instance.CurrentState == ActManager.ActState.ActInProgress && objectType == ObjectType.Quest)
        {
            transform.GetComponent<InteractableObject>().enabled = true;
        } else if (ActManager.Instance.CurrentState == ActManager.ActState.ActEnd && objectType == ObjectType.End)
        {
            transform.GetComponent<InteractableObject>().enabled = true;
        } else
        {
            transform.GetComponent<InteractableObject>().enabled = false;
        }
    }

    private void OnActStateChanged(ActManager.ActState state)
    {
        OnStart();
    }
}

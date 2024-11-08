using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;

    UnityEvent IInteractable.OnInteract
    {
        get => onInteract;
        set => onInteract = value;
    }
    public void Interact()
    {
        if (enabled)
        {
            onInteract.Invoke();
        }
        
    }
}

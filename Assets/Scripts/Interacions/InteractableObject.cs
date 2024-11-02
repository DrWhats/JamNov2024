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
    public void Interact() => onInteract.Invoke();
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public void OnInteract(InputValue value)
    {
        Debug.Log("Interacted");
        DoInteraction();
    }

    void DoInteraction()
    {
        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10f))
        {
            return;
        }

        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }
}

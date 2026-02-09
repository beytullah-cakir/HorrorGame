using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }

    public void OnHoverEnter()
    {
        // Add highlight logic here if needed for this specific object
    }

    public void OnHoverExit()
    {
        // Remove highlight logic here
    }
}

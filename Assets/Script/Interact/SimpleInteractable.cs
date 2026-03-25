using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }

    public bool CanInteract()
    {
        return true;
    }
}

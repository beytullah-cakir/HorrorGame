using QuestSystem;
using UnityEngine;

public class Flashlight : InteractableBase
{
    public override void Interact()
    {
        FlashlightController.Instance.EnableFlashlight();
        Destroy(gameObject);
    }
}

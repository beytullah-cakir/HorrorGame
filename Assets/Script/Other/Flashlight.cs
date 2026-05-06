using QuestSystem;
using UnityEngine;

public class Flashlight : QuestItem
{
    private void Awake()
    {
        // Fener toplamak için kutu gerekmesin
        requiresBox = false;
    }

    public override void Interact()
    {
        FlashlightController.Instance.EnableFlashlight();
        base.Interact();
    }
}

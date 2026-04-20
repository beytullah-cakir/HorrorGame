using QuestSystem;
using UnityEngine;

public class Flashlight : QuestItem
{
    public GameObject flashlight;
    public override void Interact()
    {
        flashlight.SetActive(true);
        base.Interact();
    }
}

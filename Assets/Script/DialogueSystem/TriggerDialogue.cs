using UnityEngine;

namespace DialogueSystem
{
    public class TriggerDialogue : MonoBehaviour
    {
        [Header("Dialogue Content")]
        [SerializeField] private DialogueLine dialogueLine;
        
        [Header("Trigger Options")]
        [SerializeField] private bool triggerOnce = true;
        [SerializeField] private string targetTag = "Player";

        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered && triggerOnce) return;

            if (other.CompareTag(targetTag))
            {
                Trigger();
            }
        }

        public void Trigger()
        {
            if (hasTriggered && triggerOnce) return;

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.PlayDialogue(dialogueLine);
                hasTriggered = true;
                Debug.Log($"[TriggerDialogue] Triggered: {dialogueLine.text.Substring(0, Mathf.Min(20, dialogueLine.text.Length))}...");
            }
            else
            {
                Debug.LogError("[TriggerDialogue] DialogueManager instance not found!");
            }
        }
    }
}

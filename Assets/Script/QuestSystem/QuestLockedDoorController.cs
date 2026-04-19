using UnityEngine;
using QuestSystem;
using DialogueSystem;

public class QuestLockedDoorController : DoorAnimatorController
{
    [Header("Quest Interaction Settings")]
    [Tooltip("Bu kapı kilitliyken hangi görev aktifse diyalog tetiklensin?")]
    [SerializeField] private Quest triggerQuest;
    [Tooltip("Kapı kilitli olduğu için çalacak olan diyalog.")]
    [SerializeField] private DialogueLine lockedDialogue;
    [Tooltip("Diyalog bitince bu görevi otomatik tamamlasın mı?")]
    [SerializeField] private bool completeQuestOnDialogueFinish = true;

    private bool _isQuestTriggered = false;

    public override void Interact()
    {
        isLocked = !Key.Instance.hasKey;
        if (isLocked && !_isQuestTriggered)
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            
            // Atanan görev aktifse diyaloğu başlat
            if (activeQuest != null && activeQuest == triggerQuest)
            {
                TriggerQuestDialogue();
                return;
            }
        }

        // Normal kapı davranışı (kilitliyse açılmaz, değilse açılır)
        base.Interact();
    }

    private void TriggerQuestDialogue()
    {
        _isQuestTriggered = true;
        
        if (DialogueManager.Instance != null && lockedDialogue != null)
        {
            // Diyalog bitişini dinle
            DialogueManager.OnDialogueFinished += HandleDialogueFinished;
            DialogueManager.Instance.PlayDialogue(lockedDialogue);
        }
        else
        {
            // Sistem yoksa veya diyalog atanmamışsa doğrudan tamamla
            HandleDialogueFinished();
        }
    }

    private void HandleDialogueFinished()
    {
        // Event aboneliğini kaldır
        DialogueManager.OnDialogueFinished -= HandleDialogueFinished;
        
        if (completeQuestOnDialogueFinish)
        {
            Debug.Log($"Kapı diyaloğu bitti, '{triggerQuest.title}' görevi tamamlanıyor.");
            QuestManager.Instance.CompleteCurrentQuest();
        }
    }
}

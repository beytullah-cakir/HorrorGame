using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace QuestSystem
{
    public class NoteUI : MonoBehaviour
    {
        public static NoteUI Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private GameObject notePanel;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (notePanel != null)
            {
                notePanel.SetActive(false);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseNote);
            }
        }

        public void OpenNote()
        {
            if (notePanel == null) return;

            notePanel.SetActive(true);
            
            // Oyuncu hareketini ve kamera hareketini kilitle
            if (FirstPersonController.Instance != null)
            {
                FirstPersonController.Instance.LockPlayerAll();
            }

            // Mouse imlecini gizle ve kilitle (Kullanıcı isteği üzerine)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void CloseNote()
        {
            if (notePanel == null) return;

            notePanel.SetActive(false);

            // Oyuncu hareketini ve kamera hareketini aç
            if (FirstPersonController.Instance != null)
            {
                FirstPersonController.Instance.UnlockPlayerAll();
            }

            // Mouse imlecini gizli tutmaya devam et (FPS oyunlarında genellikle böyledir)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // ESC tuşu kontrolü (New Input System)
            if (notePanel != null && notePanel.activeSelf)
            {
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    CloseNote();
                }
            }
        }
    }
}

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
        
        private System.Action _onCloseCallback;

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

        public void OpenNote(System.Action onClose = null)
        {
            if (notePanel == null) return;
            
            // Eğer not zaten açıksa tekrar kilitleme işlemini yapma
            if (notePanel.activeSelf) return;

            _onCloseCallback = onClose;
            notePanel.SetActive(true);
            
            // Oyuncu hareketini ve kamera hareketini kilitle
            if (FirstPersonController.Instance != null)
            {
                FirstPersonController.Instance.LockPlayerAll();
            }

            // Mouse imlecini her durumda gizli ve kilitli tut
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

            // Mouse imlecini gizle ve kilitle (FPS moduna geri dön)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Kapatma callback'ini çalıştır
            _onCloseCallback?.Invoke();
            _onCloseCallback = null;
        }

        private void Update()
        {
            // Sağ tık kontrolü (Notu kapatmak için)
            if (notePanel != null && notePanel.activeSelf)
            {
                if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
                {
                    CloseNote();
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Tooltip("Kontrol edilecek paneli buraya sürükleyin")]
    [SerializeField] private GameObject panel;

    private void Start()
    {
        // İmleci görünür ve serbest yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Başka scriptler (örn: FirstPersonController) imleci gizlemeye çalışırsa diye her zaman görünür tut
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Sağ tık (RMB) basıldığında panel açıksa kapat
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (panel != null && panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
    }

    // Panelin aktifliğini parametre ile ayarlamak için
    public void SetPanelActive(bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel referansı atanmamış!");
        }
    }

    // Panelin mevcut durumunu tersine çevirmek (Toggle) için alternatif bir fonksiyon
    public void TogglePanel()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel referansı atanmamış!");
        }
    }
}

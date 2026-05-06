using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioClip lightbulb_breakSound;

    private void Awake()
    {
        Instance = this;
    }
    
    public void TurnOffAllLights()
    {
        AudioSource.PlayClipAtPoint(lightbulb_breakSound, transform.position);
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in allLights)
        {
            if (light.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                light.enabled = false;
            }
        }
    }
}

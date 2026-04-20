using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void TurnOffAllLights()
    {
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

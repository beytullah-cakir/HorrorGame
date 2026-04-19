using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
   
    public UnityEvent onTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggered?.Invoke();
            Destroy(gameObject);
        }
    }

   
}


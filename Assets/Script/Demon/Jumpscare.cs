using UnityEngine;
using DialogueSystem;

public class Jumpscare : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject triggerArea;
    

    [Header("Settings")]
    public float speed = 10f;
    
    private bool isMoving = false;

    private void Start()
    {
        if (triggerArea != null)
        {
            TriggerArea triggerAreaComponent = triggerArea.GetComponent<TriggerArea>();
            triggerAreaComponent.onTriggered.AddListener(StartJumpscare);
        }
    }

    private void StartJumpscare()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    } 

}

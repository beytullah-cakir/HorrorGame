using UnityEngine;
using DialogueSystem;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private GameObject triggerArea;

    [SerializeField] private AudioSource jumpscareAudio;

    public float speed = 10f;

    private bool isMoving = false;

    private void Start()
    {
        if (triggerArea != null)
        {
            TriggerArea triggerAreaComponent = triggerArea.GetComponent<TriggerArea>();
            triggerAreaComponent.onTriggered.AddListener(StartJumpscare);
        }
        jumpscareAudio = GetComponent<AudioSource>();
    }

    private void StartJumpscare()
    {
        isMoving = true;


        jumpscareAudio.Play();

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
            GameManager.Instance.TurnOffAllLights();
            Destroy(gameObject);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueLine
    {
        [TextArea(3, 10)]
        public string text;
        public AudioClip voiceClip;
        public bool useTypewriter = true;
    }

    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private TMP_Text subtitleText;
        [SerializeField] private CanvasGroup dialogueCanvasGroup;

        [Header("Audio References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private float typewriterSpeed = 0.05f;
        [SerializeField] private float fadeSpeed = 2f;
        [SerializeField] private float defaultDurationPerWord = 0.5f;

        // Events
        public static event Action OnDialogueStarted;
        public static event Action OnDialogueFinished;

        private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
        private bool isPlaying = false;
        private Coroutine dialogueCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Optional: DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if (dialogueCanvasGroup != null)
            {
                dialogueCanvasGroup.alpha = 0f;
            }
        }

        public void PlayDialogue(DialogueLine line)
        {
            if (line == null) return;

            dialogueQueue.Enqueue(line);

            if (!isPlaying)
            {
                ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            if (dialogueQueue.Count > 0)
            {
                DialogueLine nextLine = dialogueQueue.Dequeue();
                dialogueCoroutine = StartCoroutine(ShowDialogue(nextLine));
            }
            else
            {
                isPlaying = false;
                StartCoroutine(FadeCanvasGroup(false));
                OnDialogueFinished?.Invoke();
            }
        }

        private IEnumerator ShowDialogue(DialogueLine line)
        {
            isPlaying = true;
            OnDialogueStarted?.Invoke();

            // Set text and start fading in
            subtitleText.text = line.useTypewriter ? "" : line.text;
            yield return StartCoroutine(FadeCanvasGroup(true));

            // Start Audio
            float duration = 0f;
            if (line.voiceClip != null)
            {
                audioSource.clip = line.voiceClip;
                audioSource.Play();
                duration = line.voiceClip.length;
            }
            else
            {
                // Fallback duration based on word count
                string[] words = line.text.Split(' ');
                duration = Mathf.Max(2f, words.Length * defaultDurationPerWord);
            }

            // Typewriter or Immediate
            if (line.useTypewriter)
            {
                yield return StartCoroutine(TypeText(line.text, duration));
            }
            else
            {
                subtitleText.text = line.text;
                yield return new WaitForSeconds(duration);
            }

            // Wait for audio to finish if it's longer than typewriter
            if (audioSource.isPlaying)
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }

            // Handle next dialogue or finish
            ProcessQueue();
        }

        private IEnumerator TypeText(string text, float maxDuration)
        {
            subtitleText.text = "";
            float startTime = Time.time;
            
            foreach (char c in text.ToCharArray())
            {
                subtitleText.text += c;
                
                // If audio/duration is almost up, speed up or finish immediately
                if (Time.time - startTime >= maxDuration - 0.1f)
                {
                    subtitleText.text = text;
                    break;
                }
                
                yield return new WaitForSeconds(typewriterSpeed);
            }
        }

        private IEnumerator FadeCanvasGroup(bool fadeIn)
        {
            if (dialogueCanvasGroup == null) yield break;

            float targetAlpha = fadeIn ? 1f : 0f;
            while (!Mathf.Approximately(dialogueCanvasGroup.alpha, targetAlpha))
            {
                dialogueCanvasGroup.alpha = Mathf.MoveTowards(dialogueCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
                yield return null;
            }
        }

        public void StopAllDialogues()
        {
            if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
            audioSource.Stop();
            dialogueQueue.Clear();
            isPlaying = false;
            StartCoroutine(FadeCanvasGroup(false));
        }
    }
}

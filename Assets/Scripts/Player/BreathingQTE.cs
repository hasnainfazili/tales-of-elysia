using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class BreathingQTE : MonoBehaviour
{
    public Slider breathingBar; // UI Slider representing breathing progress

    public float timeToPressKey = 3.0f; // Time allowed to press each key
    public int totalBreaths = 3; // Number of successful breath cycles required
    public GameObject InhaleButton; // Change to Image
    public GameObject ExhaleButton;
    private float timer = 0f;
    private int currentStep = 0;
    private int breathsCompleted = 0;
    public bool isActive = false;
    public bool BreathingComplete {get; private set;}
    private enum BreathingPhase { Inhale, Exhale }
    private BreathingPhase currentPhase;
    public static BreathingQTE instance;
    public AudioClip CorrectKeyPressedAudioClip;
    public AudioClip WrongKeyPressedAudioClip;
    public MMF_Player mmf_CorrectPressedFeedback;
    public MMF_Player mmf_WrongPressedFeedback;
    public GameObject container;
    void OnEnable()
    {
        EventsManager.instance.playerEvents.onPanicAttack += StartBreathingQTE;
    }
    void OnDisable()
    {
        EventsManager.instance.playerEvents.onPanicAttack -= StartBreathingQTE;
    }
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        breathingBar.maxValue = totalBreaths;
        breathingBar.value = 0;
        currentPhase = BreathingPhase.Inhale;
        mmf_CorrectPressedFeedback = GetComponent<MMF_Player>();
        mmf_WrongPressedFeedback = GetComponent<MMF_Player>();
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            //Animate Knob
            // Check for key presses based on the current breathing phase
            if (currentPhase == BreathingPhase.Inhale)
            {
                InhaleButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    mmf_CorrectPressedFeedback.PlayFeedbacks();
                    CorrectKeyPressed();
                    InhaleButton.SetActive(false);
                }
            
                else if(Input.anyKeyDown)
                {
                    WrongKeyPressed();
                }
            }
            else if (currentPhase == BreathingPhase.Exhale)
            {
                ExhaleButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    mmf_CorrectPressedFeedback.PlayFeedbacks();
                    ExhaleButton.SetActive(false);
                    CorrectKeyPressed();
                }
                else  if(Input.anyKeyDown)
                {
                    WrongKeyPressed();
                }
            }

            // Fail the QTE if the player doesn't press the key in time
            if (timer > timeToPressKey)
            {
                FailBreath();
                EventsManager.instance.miscEvents.BreathingQTE(false);
            }
        }
    }

    public void StartBreathingQTE()
    {
        container.SetActive(true);

        BreathingComplete = false;
        isActive = true;
        timer = 0f;
        currentStep = 0;
        breathsCompleted = 0;
        currentPhase = BreathingPhase.Inhale;
    }
  

    void CorrectKeyPressed()
    {
        timer = 0f; // Reset the timer
        SoundFXManager.instance.PlaySingleSoundFXClip(CorrectKeyPressedAudioClip, transform.position,1f);
        // Move to the next phase
        if (currentPhase == BreathingPhase.Inhale)
        {
            currentPhase = BreathingPhase.Exhale;
        }
        else if (currentPhase == BreathingPhase.Exhale)
        {
            breathsCompleted++;
            breathingBar.value = breathsCompleted;

            if (breathsCompleted >= totalBreaths)
            {
                CompleteBreathingQTE();
            }
            else
            {
                currentPhase = BreathingPhase.Inhale; // Reset to inhale for the next breath
            }
        }
    }
    void WrongKeyPressed()
    {
        SoundFXManager.instance.PlaySingleSoundFXClip(WrongKeyPressedAudioClip, transform.position,1f);
        mmf_WrongPressedFeedback.PlayFeedbacks();
        breathsCompleted = Mathf.Max(0, breathsCompleted - 1);
        breathingBar.value = breathsCompleted;
    }
    void FailBreath()
    {
        //PlayFadeoutAnimation;
        // GameManager.instance.GameOver();
        //Get PlayerController and end scene
        // You can add logic here to penalize the player or reset the sequence
        container.SetActive(false);

        timer = 0f;
    }

    void CompleteBreathingQTE()
    {
        isActive = false;
        EventsManager.instance.miscEvents.BreathingQTE(true);

        container.SetActive(false);

        //GetPlayerController and set all terrified animations to zero
        // Add logic for what happens when the QTE is successfully completed
    }
}

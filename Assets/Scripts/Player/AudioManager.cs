using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
     [SerializeField] private AudioSource footStepAudioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource playerDamageAudioSource;
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioReverbZone reverb;


    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] thoughts;
    [SerializeField] private AudioClip[] attackSound;
    [SerializeField] private AudioClip[] damageSound;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private AudioClip Footsteps;
    [SerializeField] private AudioClip Scream;
    [SerializeField] private AudioClip Breathing;
    [SerializeField] private AudioClip DeepBreath;

    private bool intrusiveThoughtsPlaying;
    bool IsBreathing = false;


    [SerializeField] float thoughDelay;
    private void OnEnable()
    {
        EventsManager.instance.playerEvents.onHighAnxietyTriggered += Breath;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered += DeepBreathing;
        EventsManager.instance.playerEvents.onPanicAttack += Screaming;
        EventsManager.instance.gameEvents.onPanicAttackTriggered += Screaming;
        EventsManager.instance.miscEvents.onCampfireActivated += Relax;

        EventsManager.instance.miscEvents.BreathingQTEState += TurnOff;

    }
    private void OnDisable()
    {
        EventsManager.instance.playerEvents.onHighAnxietyTriggered -= Breath;
        EventsManager.instance.playerEvents.onPanicAttack -= Screaming;
        EventsManager.instance.gameEvents.onPanicAttackTriggered -= Screaming;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered -= DeepBreathing;
        EventsManager.instance.miscEvents.onCampfireActivated -= Relax;

        EventsManager.instance.miscEvents.BreathingQTEState -= TurnOff;
    }
    bool isPlaying = false;
    void Update()
    {
        if(!intrusiveThoughtsPlaying)
        {
            return;
        }
        else
        StartCoroutine(IntrusiveThoughts());
    }

    IEnumerator IntrusiveThoughts()
    {
        if(!isPlaying)
        {
            isPlaying = true;
            SoundFXManager.instance.PlaySoundFXClip(thoughts, transform.position, 1f);
            yield return new  WaitForSecondsRealtime(thoughDelay);
            isPlaying = false;

        }
    }
    void Screaming()
    {
        if(!playerAudioSource.isPlaying)
        {
        reverb.enabled = true;
        playerAudioSource.clip = Scream;
        intrusiveThoughtsPlaying = true;
        playerAudioSource.Play();
        playerAudioSource.loop = true;
        }
    }
    void Breath()
    {
        if(!playerAudioSource.isPlaying)
        {
            reverb.enabled = true;
            playerAudioSource.clip = Breathing;
            intrusiveThoughtsPlaying = true;
            heartbeatAudioSource.Play();
            playerAudioSource.Play();
        }

    }
   
    void FootSteps()
    {
        footStepAudioSource.clip = Footsteps;
        footStepAudioSource.Play();
    }
    void SwordSwing()
    {
        playerAudioSource.PlayOneShot(attackSound[Random.Range(0, attackSound.Length - 1)]);
        weaponAudioSource.Play();
    }
    void Relax()
    {
        reverb.enabled = false;
        heartbeatAudioSource.Stop();
    }
    void DeepBreathing()
    {
        Relax();
        intrusiveThoughtsPlaying = false;
        playerAudioSource.PlayOneShot(DeepBreath);
    }
    void Hit()
    {
        playerAudioSource.PlayOneShot(damageSound[Random.Range(0, damageSound.Length - 1)]);
    }

    private void TurnOff(bool status)
    {
        if(status) 
        {
            reverb.enabled = false;
            playerAudioSource.clip = null;
            playerAudioSource.Stop();
            playerAudioSource.loop = false;
        }
    }
}   

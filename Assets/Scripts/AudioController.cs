using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeReference] private AudioSource ambientSource;
    [SerializeReference] private AudioSource forestSource;
    [SerializeReference] private AudioSource lysandriaSource;

    [SerializeReference] private AudioClip forestNight;
    [SerializeReference] private AudioClip forestDay;
    [SerializeReference] private AudioClip ambientAudio;
    [SerializeReference] private AudioClip combatMusic;

    
    public static AudioController instance {get; private set;}
    private void Awake()
    {
        instance = this;
    }

    public void SetCombatMusic()
    {
        ambientSource.clip = combatMusic;
        ambientSource.Play();
    }

    public void SetAmbientMusic()
    {
        ambientSource.clip  = ambientAudio;
    }
}

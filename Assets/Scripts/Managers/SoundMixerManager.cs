using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level){
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetPlayerVolume(float level)
    {
        audioMixer.SetFloat("playerVolume", Mathf.Log10(level) * 20f);
    }
    public void SetEnemiesVolume(float level)
    {
        audioMixer.SetFloat("enemiesVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);

    }
    public void SetDialogueVolume(float level)
    {
        audioMixer.SetFloat("dialogueVolume", Mathf.Log10(level) * 20f);

    }
}

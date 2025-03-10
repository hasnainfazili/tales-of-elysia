using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
   public static SoundFXManager instance{get; private set;}
   [SerializeField] private AudioSource soundFXObject;
   
   private void Awake()
   {
        if(instance == null)
        {
            instance = this;
        }
   }

   public void PlaySoundFXClip(AudioClip[] soundFXClips, Vector3 spawnPosition, float volume)
   {   
     AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
     audioSource.clip = soundFXClips[UnityEngine.Random.Range(0, soundFXClips.Length - 1)];
     audioSource.volume = volume;
     audioSource.Play();
     float clipLength = audioSource.clip.length;
     Destroy(audioSource.gameObject, clipLength);
   }
   public void PlaySingleSoundFXClip(AudioClip soundFXClip, Vector3 spawnPosition, float volume)
   {   
     AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
     audioSource.clip = soundFXClip;
     audioSource.volume = volume;
     audioSource.Play();
     float clipLength = audioSource.clip.length;
     Destroy(audioSource.gameObject, clipLength);
   }
}
